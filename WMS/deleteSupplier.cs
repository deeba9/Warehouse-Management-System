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
    public partial class deleteSupplier : Form
    {
        private SqlConnection connection;
        public deleteSupplier()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }
        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
           // connection.Open();
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void sku_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            connection.Open();
            string id = SKUtb.Text;

            // Start with a base query
            string query = "DELETE FROM SUPPLIERS WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(id))
            {
                query += " AND Id = @Id";
            }

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    cmd.Parameters.AddWithValue("@Id", int.Parse(id));
                }

                int rowCount = cmd.ExecuteNonQuery();

                if (rowCount > 0)
                {
                    MessageBox.Show("Supplier deleted successfully!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    // Product does not exist, show an error message
                    MessageBox.Show("Error: Supplier does not exist!");
                }
            }
        }
    }
}
