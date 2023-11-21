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

namespace WMS
{
    public partial class deleteForm : Form
    {
        private SqlConnection connection;
      
        public deleteForm()
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            connection.Open();
            string skuCriteria = SKUtb.Text;

            // Start with a base query
            string query = "DELETE FROM Product WHERE 1 = 1";

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

                int rowCount = cmd.ExecuteNonQuery();

                if (rowCount > 0)
                {
                    MessageBox.Show("Product deleted successfully!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    // Product does not exist, show an error message
                    MessageBox.Show("Error: Product does not exist!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
