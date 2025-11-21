using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Sales_Inventory
{
    public partial class Loginform : Form
    {
        public Loginform()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        // 🔐 Hash password using SHA256 (same as in registration)
        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Username and Password.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConnectionModule.openCon();

                // 🔐 Hash password
                string hashedPassword = HashPassword(password);

                // 🔹 Check credentials (case-sensitive username)
                string query = @"SELECT Role, FullName 
                         FROM Users 
                         WHERE BINARY Username=@username 
                         AND PasswordHash=@password 
                         AND Status='Active' 
                         LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                {
                    cmd.Parameters.Add("@username", MySqlDbType.VarChar, 50).Value = username;
                    cmd.Parameters.Add("@password", MySqlDbType.VarChar, 64).Value = hashedPassword;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["Role"]?.ToString() ?? "Unknown";
                            string fullName = reader["FullName"]?.ToString() ?? username;

                            MessageBox.Show($"Welcome {fullName} ({role})!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Session tracking
                            ConnectionModule.Session.Username = username;
                            ConnectionModule.Session.FullName = fullName;
                            ConnectionModule.Session.Role = role;
                            ConnectionModule.Session.ShiftStart = DateTime.Now;

                            reader.Close();

                            // Insert successful login log
                            string logQuery = @"INSERT INTO login_logs (Username, Role, LoginTime, Status)
                                        VALUES (@user, @role, NOW(), 'Logged In')";
                            using (MySqlCommand logCmd = new MySqlCommand(logQuery, ConnectionModule.con))
                            {
                                logCmd.Parameters.Add("@user", MySqlDbType.VarChar, 50).Value = username;
                                logCmd.Parameters.Add("@role", MySqlDbType.VarChar, 50).Value = role;
                                logCmd.ExecuteNonQuery();
                            }

                            // Proceed to form
                            Form nextForm = role == "Cashier" ? (Form)new POS(role) : new Dashboard(role);
                            nextForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            reader.Close();

                            // Log failed attempt
                            string logFailQuery = @"INSERT INTO login_logs (Username, Role, LoginTime, Status)
                                             VALUES (@user, 'Unknown', NOW(), 'Failed Attempt')";
                            using (MySqlCommand failLogCmd = new MySqlCommand(logFailQuery, ConnectionModule.con))
                            {
                                failLogCmd.Parameters.Add("@user", MySqlDbType.VarChar, 50).Value = username;
                                failLogCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Invalid username or password, or account inactive.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }



        private void Exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            ForgotForm forgotForm = new ForgotForm();
            forgotForm.ShowDialog();
            this.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
