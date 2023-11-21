using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WMS
{
    public partial class create : Form
    {
        private SqlConnection connection;

        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        public create()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void create_Load(object sender, EventArgs e)
        {

            try
            {
                
                string query = "SELECT Name FROM Product";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<string> productNames = new List<string>();

                while (reader.Read())
                {
                    productNames.Add(reader["Name"].ToString());
                }

                //connection.Close();

                // Set the data source for the ComboBox
                TypecomboBox.DataSource = productNames;
                
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                MessageBox.Show("Error: " + ex.Message);
            }
           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string productName = TypecomboBox.Text;
                string units = quantityTB.Text;
                string shipping = address.Text;
                string paymentMethod = paymentCB.Text;
                string paymentTerms = termsCB.Text;

                // Validate input data
                if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(units) ||
                    string.IsNullOrEmpty(paymentMethod) || string.IsNullOrEmpty(paymentTerms) || string.IsNullOrEmpty(shipping))
                {
                    MessageBox.Show("Please fill in all the required fields.");
                    this.Hide();
                }

                // Calculate TotalPrice based on ProductUnitPrice and Units
                decimal unitPrice = GetUnitPrice(productName); // Assuming a function to retrieve unit price
                decimal totalPrice = unitPrice * Convert.ToDecimal(units);

                string productValidationQuery = "SELECT StockLevel FROM Product WHERE Name = @Name";

                using (SqlCommand productValidationCmd = new SqlCommand(productValidationQuery, connection))
                {
                    productValidationCmd.Parameters.AddWithValue("@Name", productName);
                    // ExecuteScalar is used to retrieve a single value from the query result.
                    object result = productValidationCmd.ExecuteScalar();
                    if (result == null || Convert.ToInt32(result) < Convert.ToInt32(units) || Convert.ToInt32(units) == 0)
                    {
                        MessageBox.Show("Invalid product unit for the selected product.");                     
                    }
                    else
                    {
                        string insertOrderQuery = "INSERT INTO Orders (ProductName, ProductUnit, PaymentMethod, PaymentTerms, ShippingAddress, TotalPrice, DeliveryDate) " +
                            "VALUES (@ProductName, @ProductUnit, @PaymentMethod, @PaymentTerms, @ShippingAddress, @TotalPrice, DATEADD(DAY, CAST(SUBSTRING(@PaymentTerms, CHARINDEX('-', @PaymentTerms) + 1, LEN(@PaymentTerms) - CHARINDEX('-', @PaymentTerms) - 5) AS INT), GETDATE()))";

                        using (SqlCommand insertOrderCmd = new SqlCommand(insertOrderQuery, connection))
                        {
                            insertOrderCmd.Parameters.AddWithValue("@ProductName", productName);
                            insertOrderCmd.Parameters.AddWithValue("@ProductUnit", units);
                            insertOrderCmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                            insertOrderCmd.Parameters.AddWithValue("@PaymentTerms", paymentTerms);
                            insertOrderCmd.Parameters.AddWithValue("@ShippingAddress", shipping);
                            insertOrderCmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                            int newUnits = Convert.ToInt32(result) - Convert.ToInt32(units);
                            string updateQuery = "UPDATE Product SET StockLevel = @StockLevel where Name = @Name";
                            SqlCommand cmd = new SqlCommand(updateQuery, connection);
                            cmd.Parameters.AddWithValue("@StockLevel", newUnits);
                            cmd.Parameters.AddWithValue("@Name", productName);
                            cmd.ExecuteNonQuery();
                            // Execute the insert query
                            int rows = insertOrderCmd.ExecuteNonQuery();
                            if (rows >= 1)
                            {
                                MessageBox.Show("Order placed successfully.");
                                 this.Hide();
                                this.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show("Order is not placed!");
                                this.DialogResult = DialogResult.Cancel;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private decimal GetUnitPrice(string productName)
        {
            decimal unitPrice = 0;

            try
            {
                string query = "SELECT Price FROM Product WHERE Name = @ProductName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);

                    // ExecuteScalar is used to retrieve a single value from the query result.
                    object result = command.ExecuteScalar();

                    if (result != null && decimal.TryParse(result.ToString(), out unitPrice))
                    {
                        // Successfully retrieved and parsed the unit price
                        return unitPrice;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (logging, displaying error messages, etc.)
                Console.WriteLine("Error in GetUnitPrice: " + ex.Message);
            }

            return unitPrice;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
        }

        private void TypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
