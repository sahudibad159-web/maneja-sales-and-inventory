using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using ContextMenu = System.Windows.Forms.ContextMenu;
using TextBox = System.Windows.Forms.TextBox;

namespace Sales_Inventory
{
    public partial class Discount : Form
    {
        private List<DataGridViewRow> selectedItems;

        public List<DiscountResult> DiscountedItems { get; private set; } = new List<DiscountResult>();
        public class DiscountResult
        {
            public DataGridViewRow Row { get; set; }
            public string DiscountType { get; set; }
            public string DiscountFullName { get; set; }
            public string DiscountIDNumber { get; set; }
            public decimal DiscountAmount { get; set; }

        }

        public Discount(List<DataGridViewRow> items)
        {
            InitializeComponent();
            selectedItems = items;

            //// populate listbox dito (once lang)
            //foreach (var row in selectedItems)
            //{
            //    listBoxItems.Items.Add(row.Cells["ProductNameColumn"].Value.ToString());
            //}
        }

        public bool IsVatExempt { get; private set; }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbDiscountType.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtIDNumber.Text))
            {
                MessageBox.Show("Please fill in all discount details.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string discountID = txtIDNumber.Text.Trim();
            string discountType = cmbDiscountType.Text.Trim();

            // ✅ STEP 0: Check if ID number is at least 4 digits
            if (!discountID.All(char.IsDigit))
            {
                MessageBox.Show("ID Number must contain digits only.", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (discountID.Length < 4)
            {
                MessageBox.Show("ID Number must be at least 4 digits long.", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    // ✅ STEP 1: Check 24-hour usage
                    string checkQuery = @"
                SELECT COUNT(*) 
                FROM discount_history
                WHERE DiscountIDNumber = @ID
                  AND DiscountType = @Type
                  AND DiscountDate >= DATE_SUB(NOW(), INTERVAL 1 DAY)";

                    using (var checkCmd = new MySqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@ID", discountID);
                        checkCmd.Parameters.AddWithValue("@Type", discountType);

                        int usedCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (usedCount > 0)
                        {
                            MessageBox.Show("This ID has already received a discount within the last 24 hours.",
                                            "Discount Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // ⚡ STEP 2: Prevent using same 4-digit ending as previous IDs
                    string last4 = discountID.Substring(discountID.Length - 4);

                    string last4Query = @"
                SELECT COUNT(*) 
                FROM discount_history
                WHERE RIGHT(DiscountIDNumber, 4) = @Last4
                  AND DiscountType = @Type";

                    using (var last4Cmd = new MySqlCommand(last4Query, con))
                    {
                        last4Cmd.Parameters.AddWithValue("@Last4", last4);
                        last4Cmd.Parameters.AddWithValue("@Type", discountType);

                        int similarCount = Convert.ToInt32(last4Cmd.ExecuteScalar());
                        if (similarCount > 0)
                        {
                            MessageBox.Show("An ID with the same last 4 digits has already been used. Please check your input.",
                                            "Duplicate Ending Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // ✅ STEP 3: Fetch discount rate
                    string rateQuery = "SELECT DiscountRate FROM discount WHERE idDiscount = 1 LIMIT 1";
                    decimal discountRate = 0.0m;

                    using (var rateCmd = new MySqlCommand(rateQuery, con))
                    {
                        object result = rateCmd.ExecuteScalar();
                        if (result != null)
                        {
                            discountRate = Convert.ToDecimal(result) / 100;
                        }
                        else
                        {
                            MessageBox.Show("No discount rate found in table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // ✅ Senior/PWD VAT exempt
                    IsVatExempt = true;

                    DiscountedItems.Clear();
                    foreach (var row in selectedItems)
                    {
                        decimal price = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
                        int qty = Convert.ToInt32(row.Cells["QuantityColumn"].Value);
                        decimal discountAmount = price * qty * discountRate;

                        DiscountedItems.Add(new DiscountResult
                        {
                            Row = row,
                            DiscountType = discountType,
                            DiscountFullName = txtFullName.Text.Trim(),
                            DiscountIDNumber = discountID,
                            DiscountAmount = discountAmount
                        });
                    }

                    // ✅ STEP 4: Save to discount_history
                    foreach (var item in DiscountedItems)
                    {
                        string insertQuery = @"
                    INSERT INTO discount_history 
                    (DiscountIDNumber, DiscountType, DiscountFullName, DiscountAmount, DiscountDate)
                    VALUES (@ID, @Type, @FullName, @Amount, NOW())";

                        using (var insertCmd = new MySqlCommand(insertQuery, con))
                        {
                            insertCmd.Parameters.AddWithValue("@ID", item.DiscountIDNumber);
                            insertCmd.Parameters.AddWithValue("@Type", item.DiscountType);
                            insertCmd.Parameters.AddWithValue("@FullName", item.DiscountFullName);
                            insertCmd.Parameters.AddWithValue("@Amount", item.DiscountAmount);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying discount: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Discount_Load(object sender, EventArgs e)
        {
            // Existing
            txtIDNumber.KeyPress += DigitsOnly_KeyPress;  // Updated
            txtFullName.KeyPress += LettersAndSpaceOnly_KeyPress;   // New
            txtIDNumber.KeyDown += BlockCopyPaste_KeyDown;
            txtFullName.KeyDown += BlockCopyPaste_KeyDown;
            cmbDiscountType.KeyDown += BlockCopyPaste_KeyDown;

            txtFullName.ShortcutsEnabled = false;
            txtIDNumber.ShortcutsEnabled = false;
            txtFullName.ContextMenu = new ContextMenu();
            txtIDNumber.ContextMenu = new ContextMenu();
          

            txtFullName.ContextMenu = new ContextMenu();
            txtIDNumber.ContextMenu = new ContextMenu();
            cmbDiscountType.ContextMenu = new ContextMenu();
           // listBoxItems.ContextMenu = new ContextMenu();

            txtIDNumber.KeyDown += BlockCopyPaste_KeyDown;
            txtFullName.KeyDown += BlockCopyPaste_KeyDown;
            cmbDiscountType.KeyDown += BlockCopyPaste_KeyDown;
            //  listBoxItems.KeyDown += BlockCopyPaste_KeyDown;

            
            txtFullName.ShortcutsEnabled = false;
            txtIDNumber.ShortcutsEnabled = false;
           

        }

        private void LettersAndSpaceOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }

        // 🔹 Allow only digits
        private void DigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
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
    }
    }

