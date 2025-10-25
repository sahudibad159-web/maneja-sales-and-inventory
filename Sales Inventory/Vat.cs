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
    public partial class Vat : Form
    {
        public Vat()
        {
            InitializeComponent();
            LoadVat();
        }

        private void LoadVat()
        {
            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query = "SELECT VatRate FROM vattable LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        decimal vatRate = Convert.ToDecimal(result);
                        txtVatRate.Text = vatRate.ToString();
                        lblVat.Text = $"Current VAT: {vatRate}%";
                    }
                    else
                    {
                        txtVatRate.Text = "";
                        lblVat.Text = "Current VAT: None";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading VAT rate: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVatRate.Text))
            {
                MessageBox.Show("Please enter a VAT rate.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!decimal.TryParse(txtVatRate.Text.Trim(), out decimal vatRate))
                {
                    MessageBox.Show("Invalid VAT rate. Please enter a numeric value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    string checkQuery = "SELECT COUNT(*) FROM vattable";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, con);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        string updateQuery = "UPDATE vattable SET VatRate = @VatRate LIMIT 1";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, con);
                        updateCmd.Parameters.AddWithValue("@VatRate", vatRate);
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("VAT rate updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO vattable (VatRate) VALUES (@VatRate)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, con);
                        insertCmd.Parameters.AddWithValue("@VatRate", vatRate);
                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show("VAT rate saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Reload para updated yung lblVat
                LoadVat();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving VAT rate: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Vat_Load(object sender, EventArgs e)
        {
            txtVatRate.KeyPress += DigitsOnly_KeyPress;
            txtVatRate.ContextMenu = new ContextMenu();
            txtVatRate.KeyDown += BlockCopyPaste_KeyDown;
            txtVatRate.ShortcutsEnabled = false;
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
