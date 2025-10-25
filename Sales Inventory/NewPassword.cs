using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sales_Inventory
{
    public partial class NewPassword : Form
    {
        private string mobileNumber;
        private string username;

        public NewPassword(string mobile, string username)
        {
            InitializeComponent();
            mobileNumber = mobile;
            this.username = username; // ✅ store the username passed from SecurityCode
            txtNewPassword.UseSystemPasswordChar = true;
        }

        private void Skip_Click(object sender, EventArgs e)
        {
            Loginform login = new Loginform();
            login.Show();
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please enter your new password.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔒 Hash the password before saving
            string hashedPassword = HashPassword(newPassword);

            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    string query = "UPDATE Users SET PasswordHash=@pass WHERE Username=@username AND ContactNumber=@mobile";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@pass", hashedPassword);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@mobile", mobileNumber);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        // ✅ Log to Audit Trail
                        ConnectionModule.InsertAuditTrail(
                            "Password Change",
                            "User Management",
                            $"User '{username}' successfully changed their password."
                        );

                        MessageBox.Show("✅ Password updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Loginform login = new Loginform();
                        login.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("❌ Failed to update password. Please try again.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        // 🔐 SHA256 hashing function (same method you use during login)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
