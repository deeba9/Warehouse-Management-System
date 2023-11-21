using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Remoting.Contexts;

namespace WMS
{
    public partial class update2 : Form
    {
        private SqlConnection connection;
        private string skuToUpdate;
        public update2(string sku)
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            skuToUpdate = sku;
        }

        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
           // connection.Open();
        }
        

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void sku_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string name = namee.Text;
            string unit = units.Text;
            string description = descrip.Text;
            string prices = pricee.Text;
            string updateQuery = "UPDATE Product SET " +
                                "Name = ISNULL(@Name, Name), " +
                                "Description = ISNULL(@Description, Description), " +
                                "Price = ISNULL(@Price, Price), " +
                                "StockLevel = ISNULL(@StockLevel, StockLevel) " +
                                "WHERE SKU = @SKU";

            using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
            {
                cmd.Parameters.AddWithValue("@SKU", skuToUpdate);

                // Set parameters based on whether the fields are empty or not
                if (!string.IsNullOrWhiteSpace(name))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                }

                if (!string.IsNullOrWhiteSpace(unit))
                {
                    cmd.Parameters.AddWithValue("@StockLevel", unit);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StockLevel", DBNull.Value);
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    cmd.Parameters.AddWithValue("@Description", description);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                }

                if (!string.IsNullOrWhiteSpace(prices))
                {
                    cmd.Parameters.AddWithValue("@Price", prices);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Price", DBNull.Value);
                }

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Product updated successfully.");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Error: Product updation failed.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
