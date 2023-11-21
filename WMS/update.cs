using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WMS
{
    public partial class update : Form
    {
        private SqlConnection connection;
        public update()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            //connection.Open();
        }

        private void sku_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string skuCriteria = SKUtb.Text;

            // Start with a base query
            string query = "SELECT SKU FROM Product WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(skuCriteria))
            {
                query += " AND SKU = @sku";
            }

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                if (!string.IsNullOrWhiteSpace(skuCriteria))
                {
                    cmd.Parameters.AddWithValue("@sku", int.Parse(skuCriteria));
                }

                int rowCount = (int)cmd.ExecuteScalar(); // Use ExecuteScalar to get the count of matching rows

                if (rowCount > 0)
                {
                    // Product exists, open the update form
                    update2 up = new update2(skuCriteria);
                    DialogResult result = up.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // If the update form was successful, set this form's DialogResult to OK and close it
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    // Product does not exist, show an error message
                    MessageBox.Show("Error: Product does not exist!");
                }
            }
        }

    }
}
