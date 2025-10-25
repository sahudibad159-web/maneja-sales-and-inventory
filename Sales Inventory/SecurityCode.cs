using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sales_Inventory
{
    public partial class SecurityCode : Form
    {
        private string expectedOTP;
        private string mobileNumber;
        private string username;
        public SecurityCode(string mobile, string otp)
        {
            InitializeComponent();

            mobileNumber = mobile;
            expectedOTP = otp;

        }

        private void SecurityCode_Load(object sender, EventArgs e)
        {
            lblMobile.Text = $"Mobile: {mobileNumber}";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        { // Return to login

            Loginform login = new Loginform();
            login.Show();
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                string enteredOTP = txtOTP.Text.Trim();

                if (string.IsNullOrEmpty(enteredOTP))
                {
                    MessageBox.Show("Please enter the OTP sent to your mobile number.",
                        "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (enteredOTP == expectedOTP)
                {
                    using (MySqlConnection con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                    {
                        con.Open();
                        string query = "SELECT Username FROM Users WHERE ContactNumber=@mobile LIMIT 1";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@mobile", mobileNumber);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            username = result.ToString();
                    }

                    NewPassword newPass = new NewPassword(mobileNumber, username);
                    newPass.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid OTP. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Unexpected Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // ✅ Make sure `mobileNumber` and `expectedOTP` are class-level variables
                if (string.IsNullOrEmpty(mobileNumber))
                {
                    MessageBox.Show("Missing mobile number. Please restart the verification process.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Generate new OTP
                Random rand = new Random();
                expectedOTP = rand.Next(100000, 999999).ToString(); // 6-digit OTP

                // ✅ Compose message
                string message = $"Your new verification code is {expectedOTP}.";

                // ✅ Use the same IP and port that worked before
                string phoneIP = "172.19.101.12"; // same as in ForgotForm
                int port = 8080;

                // ✅ Send SMS using your working gateway
                SMSGatewayAndroid sms = new SMSGatewayAndroid(phoneIP, port);
                string response = sms.SendSMS(mobileNumber, message);

                // ✅ Confirmation message
                MessageBox.Show("✅ A new OTP has been sent to your registered mobile number.",
                    "OTP Resent", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optionally log the new OTP to console or file for debugging
                Console.WriteLine($"[DEBUG] New OTP sent: {expectedOTP}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error resending OTP: " + ex.Message,
                    "SMS Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
