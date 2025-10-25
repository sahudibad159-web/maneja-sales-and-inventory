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

namespace Sales_Inventory
{
    public partial class DiscountMaintenance : Form
    {
        public DiscountMaintenance()
        {
            InitializeComponent();
            LoadDiscount();
        }
        private void Discount_Load(object sender, EventArgs e)
        {
           
        }

        private void LoadDiscount()
        {
            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query = "SELECT DiscountRate FROM discount LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        decimal rate = Convert.ToDecimal(result);
                        txtDiscount.Text = rate.ToString();
                        lblDiscount.Text = "Current Discount: " + rate + "%";
                    }
                    else
                    {
                        txtDiscount.Text = "";
                        lblDiscount.Text = "No discount set.";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading discount: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                MessageBox.Show("Please enter a discount rate.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal rate = Convert.ToDecimal(txtDiscount.Text);

                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    // Always update id=1 (global rate for Senior & PWD)
                    string query = @"INSERT INTO discount (idDiscount, DiscountRate) 
                             VALUES (1, @rate) 
                             ON DUPLICATE KEY UPDATE DiscountRate = @rate";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@rate", rate);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Discount rate updated to {rate}%. (Applies to Senior & PWD)",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Update label
                lblDiscount.Text = $"Current Senior/PWD Discount: {rate}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DiscountMaintenance_Load(object sender, EventArgs e)
        {
            txtDiscount.KeyPress += DigitsOnly_KeyPress;
            txtDiscount.ContextMenu = new ContextMenu();
            txtDiscount.KeyDown += BlockCopyPaste_KeyDown;
            txtDiscount.ShortcutsEnabled = false;
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
    }
}
