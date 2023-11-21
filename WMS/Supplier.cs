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
using static Azure.Core.HttpHeader;

namespace WMS
{
    public partial class Supplier : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;
      
        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            //connection.Open();
        }

        private void LoadProductData()
        {
            string query = "SELECT ID, Name, payment, phone,  productType, priceU,units FROM Suppliers";
            dataAdapter = new SqlDataAdapter(query, connection);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Suppliers");
            dataGridView1.DataSource = dataSet.Tables["Suppliers"];
        }

        public Supplier()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            LoadProductData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            connection.Open();
            string descrip = namee.Text;
            string name = idTb.Text;
            string num = number.Text;
            string method = methodTB.Text;  
            string unit = units.Text;
            string item = type.Text;
            string price = prices.Text;


            // Define an SQL query to insert the new product into the database
            string query = "INSERT INTO Suppliers ( Name, phone, payment, units, priceU, productType) VALUES  (@Name, @phone, @payment, @units, @priceU, @productType)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@phone", num);
                command.Parameters.AddWithValue("@payment", method);
                command.Parameters.AddWithValue("@units", int.Parse(unit));
                command.Parameters.AddWithValue("@productType", item);
                command.Parameters.AddWithValue("@priceU", price);
                string query2 = "Insert INTO Product (Name,  Description , Price, StockLevel) Values (@Name, @Description,  @Price, @StockLevel)";
                SqlCommand command1 = new SqlCommand(query2, connection);
                command1.Parameters.AddWithValue("@Name", item);
                command1.Parameters.AddWithValue("@Description", descrip);
                command1.Parameters.AddWithValue("@Price", price);
                command1.Parameters.AddWithValue("@StockLevel", int.Parse(unit));

                int rowsAffected = command.ExecuteNonQuery();
                int rowsAffected2 = command1.ExecuteNonQuery();
                if (rowsAffected > 0 && rowsAffected2 > 0)
                {
                    
                    MessageBox.Show("Supplier Data inserted successfully.");
                    namee.Text = "";
                    idTb.Text = "";
                    number.Text = "";
                    methodTB.Text = "";
                    units.Text = "";
                    type.Text = "";
                    prices.Text = "";

                    LoadProductData();
                   
                }
                else
                {
                    MessageBox.Show("Error: Supplier Data insertion failed.");
                }


            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            connection.Open();
            string deleteQuery = "DELETE FROM Suppliers";
            using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
            {
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    LoadProductData();
                }
                else
                {
                    MessageBox.Show("No records deleted.");
                }
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            deleteSupplier deleteSupplier = new deleteSupplier();
            DialogResult result = deleteSupplier.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadProductData();
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            updateSupplier update = new updateSupplier();

            // Show the insertForm as a dialog
            DialogResult result = update.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadProductData();
            }
        }
    }
}
