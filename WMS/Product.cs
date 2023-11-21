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
using System.Runtime.Remoting.Contexts;

namespace WMS
{
    public partial class Product : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;
        public Product()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            LoadProductData();
        }

        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        private void LoadProductData()
        {
            string query = "SELECT SKU, Name, Description, Price, StockLevel FROM Product";
            dataAdapter = new SqlDataAdapter(query, connection);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Product");
            dataGridView1.DataSource = dataSet.Tables["Product"];
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string skuCriteria = SKtext.Text;
            string nameCriteria = nameText.Text;
            string descriptionCriteria = descripText.Text;

            // Start with a base query
            string query = "SELECT SKU, Name, Description, Price, StockLevel FROM Product WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(skuCriteria))
            {
                query += " AND SKU = @sku";
            }

            if (!string.IsNullOrWhiteSpace(nameCriteria))
            {
                query += " AND Name LIKE @name";
            }

            if (!string.IsNullOrWhiteSpace(descriptionCriteria))
            {
                query += " AND Description LIKE @description";
            }

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                if (!string.IsNullOrWhiteSpace(skuCriteria))
                {
                    cmd.Parameters.AddWithValue("@sku", int.Parse(skuCriteria));
                }

                if (!string.IsNullOrWhiteSpace(nameCriteria))
                {
                    cmd.Parameters.AddWithValue("@name", "%" + nameCriteria + "%");
                }

                if (!string.IsNullOrWhiteSpace(descriptionCriteria))
                {
                    cmd.Parameters.AddWithValue("@description", "%" + descriptionCriteria + "%");
                }

                dataAdapter = new SqlDataAdapter(cmd);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Products");
                dataGridView1.DataSource = dataSet.Tables["Products"];
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            update update = new update();

            // Show the insertForm as a dialog
            DialogResult result = update.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadProductData();
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void insert_Click(object sender, EventArgs e)
        {
            // Create an instance of your insertForm
            insertForm insertForm = new insertForm();

            // Show the insertForm as a dialog
            DialogResult result = insertForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadProductData();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            deleteForm deleteForm = new deleteForm();
            DialogResult result = deleteForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadProductData();
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {

            string deleteQuery = "DELETE FROM Product"; 
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
    }
}
