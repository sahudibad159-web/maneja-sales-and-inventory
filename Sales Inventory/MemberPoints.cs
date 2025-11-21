using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sales_Inventory
{
    public partial class MemberPoints : Form
    {
        private string memberId;
        private decimal currentPoints;

        public bool PointsCoverTotal { get; private set; } = false;
        public decimal TotalAmount { get; set; } // set from POS before opening MemberPoints


        public decimal RedeemedPoints { get; private set; } = 0;
        public MemberPoints(string memberId)
        {
            InitializeComponent();
            this.memberId = memberId;
        }

        private void MemberPoints_Load(object sender, EventArgs e)
        {
            txtPointsToRedeem.KeyPress += DigitsOnly_KeyPress;
            txtPointsToRedeem.ContextMenu = new ContextMenu();
            txtPointsToRedeem.KeyDown += BlockCopyPaste_KeyDown;
            txtPointsToRedeem.ShortcutsEnabled = false;
            // Load member info from DB
            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();
                string query = "SELECT FirstName,LastName, Points FROM members WHERE MemberCode = @code";
                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@code", memberId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label4.Text = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();

                            currentPoints = Convert.ToDecimal(reader["Points"]);
                            lblCurrentPoints.Text = currentPoints.ToString("N2");
                        }
                        else
                        {
                            MessageBox.Show("Member not found!");
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                        }
                    }
                }
            }
        }
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

        // 🔹 Digits only (ContactNumber)
        private void DigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
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
        public bool CartIsEmpty { get; set; }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            // Reset muna
            RedeemedPoints = 0;
            PointsCoverTotal = false;

            // 🛑 Prevent redeeming if cart is empty
            if (CartIsEmpty)
            {
                MessageBox.Show("You cannot redeem points because the cart is empty.",
                                "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1️⃣ Check kung naka-check ang checkbox
            if (!chkRedeem.Checked)
            {
                MessageBox.Show("Please check Redeem first if you want to use points.",
                                "Redeem Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2️⃣ Check kung may input sa textbox
            if (string.IsNullOrWhiteSpace(txtPointsToRedeem.Text))
            {
                MessageBox.Show("Please enter how many points you want to redeem.",
                                "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3️⃣ Clean input
            string cleanText = txtPointsToRedeem.Text.Replace("₱", "").Replace(",", "").Trim();

            if (!decimal.TryParse(cleanText, NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal toRedeem))
            {
                MessageBox.Show("Invalid points value. Please enter a valid number (e.g. 100 or 100.50).",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4️⃣ Check kung sapat ang points ng member
            if (toRedeem > currentPoints)
            {
                MessageBox.Show("Insufficient points!",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 5️⃣ Check kung valid total
            if (TotalAmount <= 0)
            {
                MessageBox.Show("Invalid total amount. Please check the POS total.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 6️⃣ Prevent redeeming more than total amount
            if (toRedeem > TotalAmount)
            {
                MessageBox.Show($"You cannot redeem more points than the total amount.\n\n" +
                                $"Total: ₱{TotalAmount:N2}\nPoints entered: {toRedeem:N2}",
                                "Invalid Redemption", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 7️⃣ Valid → assign values
            RedeemedPoints = toRedeem;

            // If fully covered by points
            if (toRedeem == TotalAmount)
                PointsCoverTotal = true;

            // ✅ Close cleanly
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
