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
    public partial class OrderReceiptForm : Form
    {
        private SqlConnection connection;
       private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public OrderReceiptForm()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
           
        }
        // Method to set order details and update labels
        public void SetOrderDetails(string productName, string quantitys, string totalPrice, string method, DateTime date, DateTime times)
        {
            nameee.Text = quantitys;
            names.Text = productName;
            methods.Text = method; 
            pricee.Text = totalPrice;
            dates.Text = date.ToString();
            timesss.Text = times.ToString();

            nameee.ReadOnly = true;
            names.ReadOnly = true;
            methods.ReadOnly = true;
            pricee.ReadOnly = true;
            dates.ReadOnly = true;
            timesss.ReadOnly = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.ReadOnly = true;
        }

        private void date_TextChanged(object sender, EventArgs e)
        {

        }

        private void timesss_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
        }
    }
}
