using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace WMS
{
    public partial class insertForm : Form
    {
        public insertForm()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private SqlConnection connection;
      
        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            //connection.Open();
        }

        private void backbuttton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            //string sku = skutext.Text;
            string name = nametb.Text;
            string description = descrip.Text;
            string price = priceTB.Text;
            string stockLevel = stockTB.Text;

           
            // Define an SQL query to insert the new product into the database
            string query = "INSERT INTO Product ( Name, Description, Price, StockLevel) VALUES ( @Name, @Description, @Price, @StockLevel)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Add parameters to the query
               // command.Parameters.AddWithValue("@SKU", int.Parse(sku));
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@StockLevel", stockLevel);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // The insertion was successful
                    MessageBox.Show("Product inserted successfully.");

                    // Close the InsertForm and set the DialogResult to OK
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Error: Product insertion failed.");
                }
               

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
          // Prevent the default behavior (closing the entire application)
           this.Hide();
        }
    }
}
