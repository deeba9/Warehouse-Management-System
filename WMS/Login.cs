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
    public partial class Login : Form
    {
        private SqlConnection connection;
        private int failedLoginAttempts = 0;
        private const int maxFailedAttempts = 5;
        private bool isApplicationLocked = false;

        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\PMLS\\Desktop\\WMS\\WMS\\Database.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
        }
        public Login()
        {
            InitializeComponent();
            InitializeDatabaseConnection();

        }

        private void closebt_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginbt_Click(object sender, EventArgs e)
        {
            try
            {
                if (isApplicationLocked)
                {
                    MessageBox.Show("The application is currently locked. Please try again later.");
                    return;
                }
                else
                {
                    connection.Open();

                    string query = "SELECT type FROM Users WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Email", userTB.Text);
                        cmd.Parameters.AddWithValue("@Password", pswd.Text);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string userType = reader["Type"].ToString();
                                    if (userType == "Admin")
                                    {
                                        // Valid login for an Admin user. Show the landing form.
                                        this.Hide();
                                        landing land = new landing();
                                        DialogResult result = land.ShowDialog();
                                    }
                                    if (userType == "Customer")
                                    {
                                        // Valid login for an Customer user. Show the landing form.
                                        this.Hide();
                                        landing2 land2 = new landing2();
                                        DialogResult result = land2.ShowDialog();
                                    }
                                }
                                failedLoginAttempts = 0;
                            }
                            else
                            {
                                // Failed login
                                failedLoginAttempts++;
                                if (failedLoginAttempts >= maxFailedAttempts)
                                {
                                    LockApplication();
                                }
                                else
                                {
                                    MessageBox.Show($"Invalid username or password. Attempts remaining: {maxFailedAttempts - failedLoginAttempts}");
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void LockApplication()
        {
            isApplicationLocked = true;
            // Disable login controls, display a message, or take other actions for locking
            MessageBox.Show("Application disabled for 2 minutes!");
            Timer unlockTimer = new Timer();
            unlockTimer.Interval = 2 * 60 * 1000; // 2 minutes in milliseconds

            unlockTimer.Tick += (s, args) =>
            {
                // This code will be executed after the specified interval (15 minutes)
                failedLoginAttempts = 0;
                isApplicationLocked = false;
                unlockTimer.Stop(); // Stop the timer after the specified interval
                unlockTimer.Dispose(); // Dispose of the timer to free up resources
            };

            unlockTimer.Start();
        }



        private void userTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
