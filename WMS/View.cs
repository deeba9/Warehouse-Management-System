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
    public partial class View : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;

        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            connection.Open();

        }
        public View()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            LoadProductData();
        }


        private void LoadProductData()
        {
            string query = "SELECT ID, ProductName, TotalPrice FROM Orders";
            dataAdapter = new SqlDataAdapter(query, connection);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Orders");
            dataGridView1.DataSource = dataSet.Tables["Orders"];
            DataGridViewButtonColumn btnReceiptColumn = new DataGridViewButtonColumn();
            btnReceiptColumn.Name = "btnReceipt";
            btnReceiptColumn.HeaderText = "Generate Receipt";
            btnReceiptColumn.Text = "Generate";
            btnReceiptColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnReceiptColumn);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["btnReceipt"].Index && e.RowIndex >= 0)
            {
                int selectedOrderID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
                string productName = dataGridView1.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                //int quantity = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value);
                string totalPrice = dataGridView1.Rows[e.RowIndex].Cells["TotalPrice"].Value.ToString();
                
                string query = "SELECT ProductUnit, PaymentMethod, TimeStamp,  DeliveryDate FROM Orders WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", selectedOrderID);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    // Check if there is at least one row in the result
                    if (table.Rows.Count > 0)
                    {
                        // Access the values from the first row (assuming there's only one result)
                        string paymentMethod = table.Rows[0]["PaymentMethod"].ToString();
                        DateTime timeStamp = Convert.ToDateTime(table.Rows[0]["TimeStamp"]);
                        string quantity = table.Rows[0]["ProductUnit"].ToString();
                        DateTime deliveryDate = Convert.ToDateTime(table.Rows[0]["DeliveryDate"]);

                        // Open the OrderReceiptForm and pass the order details
                        OrderReceiptForm receiptForm = new OrderReceiptForm();
                        receiptForm.SetOrderDetails(productName, quantity, totalPrice, paymentMethod, deliveryDate, timeStamp);
                        receiptForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No data found for the specified ID.");
                    }
                }


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
        }
    }
}
