using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;


namespace Sales_Inventory
{
    public partial class ForgotForm : Form
    {
        // IP of phone running SMS Gateway
        private string phoneIP = "192.168.43.1"; // change to your actual phone IP
        private int port = 8080;

        public ForgotForm()
        {
            InitializeComponent();
            // Bawal mag-right click copy/paste
            txtMobile.ContextMenu = new ContextMenu();
            txtMobile.KeyPress += DigitsOnly_KeyPress;
            txtMobile.KeyDown += BlockCopyPaste_KeyDown;
            txtMobile.KeyPress += BlockMultipleSpaces_KeyPress;
            txtMobile.ShortcutsEnabled = false;
        }

        // 🔹 Digits only (ContactNumber)
        private void DigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // 🔹 Letters + numbers + space + ., - allowed (Address)
        private void BlockMultipleSpaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb != null && e.KeyChar == ' ')
            {
                int pos = tb.SelectionStart;

                // 1. Bawal kung unang character
                if (pos == 0)
                {
                    e.Handled = true;
                    return;
                }

                // 2. Bawal kung previous character ay space
                if (pos > 0 && tb.Text[pos - 1] == ' ')
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void BlockCopyPaste_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.V || e.KeyCode == Keys.X)) ||
                (e.Shift && e.KeyCode == Keys.Insert) ||
                (e.Control && e.KeyCode == Keys.Insert))
            {
                e.SuppressKeyPress = true;
                MessageBox.Show("Copy/Paste is disabled in this field.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            new Loginform().Show();
            this.Close();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            string mobile = txtMobile.Text.Trim();

            if (string.IsNullOrEmpty(mobile))
            {
                MessageBox.Show("Please enter your registered mobile number.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    string query = @"SELECT Username FROM Users 
                                     WHERE ContactNumber=@mobile 
                                     AND Status='Active' 
                                     AND Role IN ('Admin','Staff','Cashier')";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@mobile", mobile);

                    string username = null;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            username = reader["Username"].ToString();
                    }

                    if (username != null)
                    {
                        // ✅ Generate OTP
                        string otp = new Random().Next(100000, 999999).ToString();

                        // ✅ Send via SMS Gateway
                        SMSGatewayAndroid sms = new SMSGatewayAndroid(phoneIP, port);
                        string response = sms.SendSMS(mobile,
                            $"Your OTP code is {otp}. Use it to reset your {username} account password.");

                        // ✅ Show success
                        MessageBox.Show($"✅ OTP sent successfully to {mobile}.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // ✅ Proceed to OTP verification form
                        SecurityCode securityForm = new SecurityCode(mobile, otp);
                        securityForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Mobile number not registered or inactive account.",
                            "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error: " + ex.Message, "SMS Gateway / Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ForgotForm_Load(object sender, EventArgs e)
        {

        }
    }
}
