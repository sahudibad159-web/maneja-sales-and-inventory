using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zen.Barcode;
namespace Sales_Inventory
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        public int NewMemberId { get; private set; }
        public string NewMemberCode { get; private set; }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            // Basic field validation
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtContactNumber.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string contact = txtContactNumber.Text.Trim();

            // Contact validation ulit (in case hindi nag-trigger yung Leave)
            if (!Regex.IsMatch(contact, @"^\d+$"))
            {
                MessageBox.Show("Contact number must contain numbers only.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactNumber.Focus();
                return;
            }

            if (!contact.StartsWith("09"))
            {
                MessageBox.Show("Contact number must start with '09'.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactNumber.Focus();
                return;
            }

            if (contact.Length != 11)
            {
                MessageBox.Show("Contact number must be 11 digits and start with '09'.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactNumber.Focus();
                return;
            }

            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    using (var txn = con.BeginTransaction())
                    {
                        try
                        {
                            string insertSql = @"INSERT INTO members (FirstName, LastName, ContactNumber, Points, DateJoined, MemberCode)
                                         VALUES (@first, @last, @contact, 0, @date, @code)";
                            using (var cmd = new MySqlCommand(insertSql, con, txn))
                            {
                                cmd.Parameters.AddWithValue("@first", txtFirstName.Text.Trim());
                                cmd.Parameters.AddWithValue("@last", txtLastName.Text.Trim());
                                cmd.Parameters.AddWithValue("@contact", contact);
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@code", txtCode.Text.Trim());
                                cmd.ExecuteNonQuery();
                            }

                            long newId;
                            using (var cmd = new MySqlCommand("SELECT LAST_INSERT_ID();", con, txn))
                            {
                                newId = Convert.ToInt64(cmd.ExecuteScalar());
                            }

                            string newCode = $"M{newId:D4}";

                            string updateSql = "UPDATE members SET MemberCode = @code WHERE MemberID = @id";
                            using (var cmd = new MySqlCommand(updateSql, con, txn))
                            {
                                cmd.Parameters.AddWithValue("@code", newCode);
                                cmd.Parameters.AddWithValue("@id", newId);
                                cmd.ExecuteNonQuery();
                            }

                            txn.Commit();

                            this.NewMemberId = (int)newId;
                            this.NewMemberCode = newCode;

                            MessageBox.Show("Member registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // ✅ Just call without parameters
                            PrintMembershipCard();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            try { txn.Rollback(); } catch { }
                            MessageBox.Show("Error saving member: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PrintMembershipCard()
        {
            string memberName = txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim();
            string memberID = this.NewMemberCode;

            PrintDocument pd = new PrintDocument();
            // Taas ng papel pinalaki para hindi putol ang barcode at ID
            pd.DefaultPageSettings.PaperSize = new PaperSize("Custom", 576, 450); // 58mm width, taller height

            pd.PrintPage += (sender, e) =>
            {
                int startY = 5;
                int paperWidth = 180; // mm ~ 58mm
                Brush brush = Brushes.Black;

                // Logo - mas malaki
                Image logo = Properties.Resources.logo;
                if (logo != null)
                {
                    int logoWidth = 120; // pinalaki
                    int logoHeight = 60; // pinalaki
                    int logoX = (paperWidth - logoWidth) / 2;
                    e.Graphics.DrawImage(logo, logoX, startY, logoWidth, logoHeight);
                    startY += logoHeight + 15; // dagdag space sa baba
                }

                // Store Name
                string storeName = "MANEJA GROCERY STORE";
                using (Font fontStore = new Font("Arial", 12, FontStyle.Bold))
                {
                    SizeF storeSize = e.Graphics.MeasureString(storeName, fontStore);
                    e.Graphics.DrawString(storeName, fontStore, brush, (paperWidth - storeSize.Width) / 2, startY);
                    startY += (int)storeSize.Height + 15;
                }

                // Member Name
                using (Font fontName = new Font("Arial", 14, FontStyle.Bold))
                {
                    SizeF nameSize = e.Graphics.MeasureString(memberName, fontName);
                    e.Graphics.DrawString(memberName, fontName, brush, (paperWidth - nameSize.Width) / 2, startY);
                    startY += (int)nameSize.Height + 20; // dagdag space bago barcode
                }

                // Barcode (Zen.Barcode)
                Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;
                Image barcodeImage = barcode.Draw(memberID, 80); // mas mataas para malinaw
                int barcodeX = (paperWidth - barcodeImage.Width) / 2;
                e.Graphics.DrawImage(barcodeImage, barcodeX, startY);
                startY += barcodeImage.Height + 15; // dagdag space sa baba

                // Member ID
                using (Font fontID = new Font("Arial", 12, FontStyle.Regular))
                {
                    SizeF idSize = e.Graphics.MeasureString(memberID, fontID);
                    e.Graphics.DrawString(memberID, fontID, brush, (paperWidth - idSize.Width) / 2, startY);
                }
            };

            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Printing error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Register_Load(object sender, EventArgs e)
        {
          

            // Restrict inputs (letters, spaces, ., , , - only)
            txtFirstName.KeyPress += BlockInvalidCharacters_KeyPress;
            txtLastName.KeyPress += BlockInvalidCharacters_KeyPress;
            

            // Bawal mag-type ng number
            txtFirstName.KeyPress += BlockNumbers_KeyPress;
            txtFirstName.KeyPress += BlockNumbers_KeyPress;

            // Restrict inputs
           
            txtFirstName.KeyPress += BlockInvalidCharacters_KeyPress;
            txtLastName.KeyPress += BlockInvalidCharacters_KeyPress;

            // Bawal mag-right click copy/paste
            txtCode.ContextMenu = new ContextMenu();
            txtContactNumber.ContextMenu = new ContextMenu();
            txtFirstName.ContextMenu = new ContextMenu();
            txtLastName.ContextMenu = new ContextMenu();

            // Bawal Ctrl+V / Ctrl+C / Ctrl+X / Shift+Insert / Ctrl+Insert
            txtLastName.KeyDown += BlockCopyPaste_KeyDown;
            txtFirstName.KeyDown += BlockCopyPaste_KeyDown;
            txtContactNumber.KeyDown += BlockCopyPaste_KeyDown;
            txtCode.KeyDown += BlockCopyPaste_KeyDown;

            // Auto-normalize spaces kapag iniwan ang textbox
            txtCode.Leave += NormalizeSpaces;
            txtContactNumber.Leave += NormalizeSpaces;
            txtFirstName.Leave += NormalizeSpaces;
            txtLastName.Leave += NormalizeSpaces;

            txtContactNumber.KeyPress += BlockNonNumeric_KeyPress;

            txtLastName.ShortcutsEnabled = false;
            txtFirstName.ShortcutsEnabled = false;
            txtContactNumber.ShortcutsEnabled = false;
            txtCode.ShortcutsEnabled = false;
        }
        private void BlockNonNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control keys (like Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // block all non-numeric
            }
        }

        private void NormalizeSpaces(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                // Trim leading/trailing spaces + multiple spaces → 1 space
                tb.Text = Regex.Replace(tb.Text.Trim(), @"\s+", " ");
            }
        }

        private void BlockNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kung digit, wag i-allow (silent block)
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BlockCopyPaste_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.V || e.KeyCode == Keys.X)) ||
                (e.Shift && e.KeyCode == Keys.Insert) || // Shift+Insert = Paste
                (e.Control && e.KeyCode == Keys.Insert)) // Ctrl+Insert = Copy
            {
                e.SuppressKeyPress = true;
                MessageBox.Show("Copy/Paste is disabled in this field.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void BlockInvalidCharacters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // block lahat ng hindi letter at control
            }
        }
        private void btnGenerateCode_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    // kunin yung huling ginamit na ID
                    string sql = "SELECT AUTO_INCREMENT FROM information_schema.TABLES " +
                                 "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'members'";
                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        object result = cmd.ExecuteScalar();
                        long nextId = Convert.ToInt64(result);

                        string newCode = $"M{nextId:D4}"; // ex. M0005
                        txtCode.Text = newCode;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating code: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtContactNumber_Leave(object sender, EventArgs e)
        {
            //string contact = txtContactNumber.Text.Trim();

            //if (string.IsNullOrEmpty(contact))
            //    return;

            //// Check kung puro number lang (safety check)
            //if (!Regex.IsMatch(contact, @"^\d+$"))
            //{
            //    MessageBox.Show("Contact number must contain numbers only.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtContactNumber.Focus();
            //    return;
            //}

            //// Check kung nagsisimula sa 09
            //if (!contact.StartsWith("09"))
            //{
            //    MessageBox.Show("Contact number must start with '09'.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtContactNumber.Focus();
            //    return;
            //}

            //// Check length (minimum 11 digits typical sa PH mobile number)
            //if (contact.Length != 11)
            //{
            //    MessageBox.Show("Contact number must be 11 digits and start with '09'.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtContactNumber.Focus();
            //    return;
            }
        

    }
}
