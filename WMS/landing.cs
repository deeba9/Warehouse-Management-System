using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMS
{
    public partial class landing : Form
    {
        public landing()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product prod = new Product();
            DialogResult result = prod.ShowDialog();
        }

        private void loginbt_Click(object sender, EventArgs e)
        {
            Supplier su = new Supplier();
            DialogResult result = su.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            DialogResult dialogResult = login.ShowDialog();
        }
    }
}
