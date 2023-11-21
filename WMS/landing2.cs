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
    public partial class landing2 : Form
    {
        public landing2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            create create  = new create();    
            DialogResult result = create.ShowDialog();
           
        }

        private void prodBT_Click(object sender, EventArgs e)
        {
            View view = new View(); 
            DialogResult dialogResult = view.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();  
            DialogResult dialogResult = login.ShowDialog(); 
        }
    }
}
