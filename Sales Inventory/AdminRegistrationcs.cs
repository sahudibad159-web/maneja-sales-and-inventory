using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sales_Inventory
{
    public partial class AdminRegistrationcs : Form
    {
        private string userRole = "Admin"; // default role = Admin

        public AdminRegistrationcs(string role)
        {
            InitializeComponent();
            HookTextBoxEvents();
            userRole = role;

        }
        private void HookTextBoxEvents()
        {
            // 🔹 Name fields (FirstName, LastName) - letters, spaces, ., - only, no numbers
            txtFirstName.KeyPress += BlockInvalidCharacters_KeyPress;
            txtLastName.KeyPress += BlockInvalidCharacters_KeyPress;
            txtFirstName.KeyPress += BlockMultipleSpaces_KeyPress;
            txtLastName.KeyPress += BlockMultipleSpaces_KeyPress;
            txtFirstName.TextChanged += NormalizeSpaces_OnChange;
            txtLastName.TextChanged += NormalizeSpaces_OnChange;

            // 🔹 Username - letters, numbers, underscores only
            txtUserName.KeyPress += Username_KeyPress;

            // 🔹 Contact Number - digits only
            txtContact.KeyPress += DigitsOnly_KeyPress;

            // 🔹 Prevent copy/paste in all fields
            txtFirstName.KeyDown += BlockCopyPaste_KeyDown;
            txtLastName.KeyDown += BlockCopyPaste_KeyDown;
            txtUserName.KeyDown += BlockCopyPaste_KeyDown;
            txtPassword.KeyDown += BlockCopyPaste_KeyDown;
            txtContact.KeyDown += BlockCopyPaste_KeyDown;
        }

        // 🔹 Letters + space + ., - only
        private void BlockInvalidCharacters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) &&
                !char.IsControl(e.KeyChar) &&
                !char.IsWhiteSpace(e.KeyChar) &&
                e.KeyChar != '.' &&
                e.KeyChar != ',' &&
                e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        // 🔹 Prevent multiple spaces at start or consecutive
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

        // 🔹 Normalize spaces after change
        private void NormalizeSpaces_OnChange(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                int cursor = tb.SelectionStart;
                string newText = Regex.Replace(tb.Text, @"\s{2,}", " ");
                if (tb.Text != newText)
                {
                    tb.Text = newText;
                    tb.SelectionStart = Math.Min(cursor, tb.Text.Length);
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

        // 🔹 Username letters, numbers, underscores only
        private void Username_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) &&
                !char.IsControl(e.KeyChar) &&
                e.KeyChar != '_')
            {
                e.Handled = true;
            }
        }

        // 🔹 Block Copy/Paste
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
        private void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validate required fields
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtUserName.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtAge.Text) ||
                    string.IsNullOrWhiteSpace(txtContact.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Validate age
                if (!int.TryParse(txtAge.Text.Trim(), out int age) || age < 18 || age > 50)
                {
                    MessageBox.Show("Age must be between 18 and 50 years old.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. Validate password length
                if (txtPassword.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4. Validate username format
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtUserName.Text, @"^[a-zA-Z0-9_]+$"))
                {
                    MessageBox.Show("Username can only contain letters, numbers, and underscores.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 5. Validate contact number
                string contact = txtContact.Text.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(contact, @"^09\d{9}$"))
                {
                    MessageBox.Show("Contact number must be 11 digits and start with '09'.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string fullName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}";

                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    // 6. Check if username exists
                    string checkUserQuery = "SELECT COUNT(*) FROM users WHERE Username=@Username";
                    using (var checkCmd = new MySqlCommand(checkUserQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // 7. Check if contact number exists
                    string checkContactQuery = "SELECT COUNT(*) FROM users WHERE ContactNumber=@ContactNumber";
                    using (var checkCmd = new MySqlCommand(checkContactQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@ContactNumber", contact);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("This contact number is already registered.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // 8. Insert into database
                    string insertQuery = @"
                INSERT INTO users (Username, PasswordHash, FullName, Age, ContactNumber, Role, Status, DateCreated)
                VALUES (@Username, @PasswordHash, @FullName, @Age, @ContactNumber, @Role, @Status, NOW())";

                    using (var cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(txtPassword.Text.Trim()));
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@ContactNumber", contact);
                        cmd.Parameters.AddWithValue("@Role", userRole); // fixed
                        cmd.Parameters.AddWithValue("@Status", "Active");

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Admin account successfully created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // back to login
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        // SHA256 password hashing
        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private void AdminRegistrationcs_Load(object sender, EventArgs e)
        {

        }
    }
}
