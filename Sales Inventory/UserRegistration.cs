using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sales_Inventory
{
    public partial class UserRegistration : Form
    {
        private int lastSelectedUserRow = -1;
        private int selectedUserId = -1;
        public UserRegistration()
        {
            InitializeComponent();
            LoadUsers();
            StyleDataGridView(dgvUsers);
           
        }

        // ✅ Clear fields function
        private void ClearUserFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
            txtAge.Clear();
           txtContact.Clear();
            cmbRole.SelectedIndex = -1;
            cmbRole.SelectedItem = null;
            dgvUsers.ClearSelection();

        }
      

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void StyleDataGridView(DataGridView dgv)
        {
            // General appearance
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.GridColor = Color.LightGray;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Header style
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 35;

            // Row style
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.RowsDefaultCellStyle.Padding = new Padding(5);
            dgv.RowTemplate.Height = 30;

            // Selection style
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.ScrollBars = ScrollBars.Vertical;

            // Disable adding rows by user
            dgv.AllowUserToAddRows = false;

            // Disable row headers
            dgv.RowHeadersVisible = false;

            // Columns auto-size based on content
            // Let columns fill evenly, pero may padding at hindi dikit-dikit
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Optional: make text wrap neatly
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Keep readable proportions per column (optional fine-tuning)
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 80;
                col.FillWeight = 1; // para pantay lahat
            }


            // Single row selection
            dgv.MultiSelect = false;
        }

       


        private void LoadUsers()
        {
            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query = @"SELECT UserID, Username, PasswordHash, FullName, Age, ContactNumber, Role, Status, DateCreated 
                 FROM users 
                 ORDER BY DateCreated DESC";


                    using (var cmd = new MySqlCommand(query, con))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvUsers.DataSource = dt;

                        // 🔹 Hide sensitive columns
                        if (dgvUsers.Columns.Contains("UserID"))
                            dgvUsers.Columns["UserID"].Visible = false;

                        if (dgvUsers.Columns.Contains("PasswordHash"))
                            dgvUsers.Columns["PasswordHash"].Visible = false;

                        // 🔹 Adjust display headers
                        dgvUsers.Columns["Username"].HeaderText = "Username";
                        dgvUsers.Columns["FullName"].HeaderText = "Full Name";
                        dgvUsers.Columns["Age"].HeaderText = "Age";
                        dgvUsers.Columns["ContactNumber"].HeaderText = "Contact No.";                   
                        dgvUsers.Columns["Role"].HeaderText = "Role";
                        dgvUsers.Columns["Status"].HeaderText = "Status";
                        dgvUsers.Columns["DateCreated"].HeaderText = "Date Created";

                        // 🔹 Optional: formatting & grid style
                        dgvUsers.ReadOnly = true;
                        dgvUsers.AllowUserToAddRows = false;
                        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgvUsers.RowHeadersVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserRegistration_Load(object sender, EventArgs e)
        {
            dgvUsers.ClearSelection();
            dgvUsers.CurrentCell = null;

            txtPassword.UseSystemPasswordChar = true;
            // Restrict inputs (letters, spaces, ., , , - only)
            txtFirstName.KeyPress += BlockInvalidCharacters_KeyPress;
            txtLastName.KeyPress += BlockInvalidCharacters_KeyPress;
           

            // Bawal mag-type ng number
            txtFirstName.KeyPress += BlockNumbers_KeyPress;
            txtLastName.KeyPress += BlockNumbers_KeyPress;
            cmbRole.KeyPress += BlockNumbers_KeyPress;

            // 🔒 Lock cmbRole to predefined roles only
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Staff");
            cmbRole.Items.Add("Cashier");

            // 🧹 Prevent manual text entry or pasting
            cmbRole.ContextMenu = new ContextMenu();
            cmbRole.KeyPress += (s, ev) => { ev.Handled = true; }; // disable typing

            // Restrict inputs


            txtFirstName.KeyPress += BlockInvalidCharacters_KeyPress;
            txtLastName.KeyPress += BlockInvalidCharacters_KeyPress;
           
            cmbRole.KeyPress += BlockInvalidCharacters_KeyPress;
          

            // Bawal mag-right click copy/paste
            txtAge.ContextMenu = new ContextMenu();
            txtContact.ContextMenu = new ContextMenu();
            txtFirstName.ContextMenu = new ContextMenu();
            txtLastName.ContextMenu = new ContextMenu();
            txtPassword.ContextMenu = new ContextMenu();
            cmbRole.ContextMenu = new ContextMenu();
            txtUserName.ContextMenu = new ContextMenu();

            // Bawal Ctrl+V / Ctrl+C / Ctrl+X / Shift+Insert / Ctrl+Insert
            txtAge.KeyDown += BlockCopyPaste_KeyDown;
            txtContact.KeyDown += BlockCopyPaste_KeyDown;
            txtFirstName.KeyDown += BlockCopyPaste_KeyDown;
            txtLastName.KeyDown += BlockCopyPaste_KeyDown;
            txtPassword.KeyDown += BlockCopyPaste_KeyDown;
            cmbRole.KeyDown += BlockCopyPaste_KeyDown;
            txtUserName.KeyDown += BlockCopyPaste_KeyDown;

            // Auto-normalize spaces kapag iniwan ang textbox
            txtUserName.Leave += NormalizeSpaces;
            cmbRole.Leave += NormalizeSpaces;
            txtPassword.Leave += NormalizeSpaces;
            txtLastName.Leave += NormalizeSpaces;
            txtFirstName.Leave += NormalizeSpaces;
            txtContact.Leave += NormalizeSpaces;
            txtAge.Leave += NormalizeSpaces;

            // --- Numbers only fields ---
            txtAge.KeyPress += NumbersOnly_KeyPress;
            txtContact.KeyPress += NumbersOnly_KeyPress;

            txtAge.ShortcutsEnabled = false;
            txtContact.ShortcutsEnabled = false;
            txtFirstName.ShortcutsEnabled = false;
            txtLastName.ShortcutsEnabled = false;
            txtPassword.ShortcutsEnabled = false;
         
            txtUserName.ShortcutsEnabled = false;
        }

        private void NumbersOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits and backspace only
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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


       

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // If same row clicked again → deselect
                    if (e.RowIndex == lastSelectedUserRow)
                    {
                        dgvUsers.ClearSelection();
                        ClearUserFields();
                        lastSelectedUserRow = -1;
                        selectedUserId = -1;
                        return;
                    }

                    DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

                    if (row.Cells["UserID"].Value == DBNull.Value)
                    {
                        ClearUserFields();
                        return;
                    }

                    // ✅ Fill the textboxes
                    selectedUserId = Convert.ToInt32(row.Cells["UserID"].Value);
                    txtUserName.Text = row.Cells["Username"].Value?.ToString();
                  //  txtPassword.Text = row.Cells["PasswordHash"].Value?.ToString();
                    txtAge.Text = row.Cells["Age"].Value?.ToString();
                    txtContact.Text = row.Cells["ContactNumber"].Value?.ToString();    
                    cmbRole.Text = row.Cells["Role"].Value?.ToString();

                    // Split fullname into first and last name (optional)
                    string fullName = row.Cells["FullName"].Value?.ToString() ?? "";
                    string[] parts = fullName.Split(' ');
                    txtFirstName.Text = parts.Length > 0 ? parts[0] : "";
                    txtLastName.Text = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : "";

                    lastSelectedUserRow = e.RowIndex;
                }
                else
                {
                    dgvUsers.ClearSelection();
                    ClearUserFields();
                    lastSelectedUserRow = -1;
                    selectedUserId = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string sanitize(string input)
        {
            return Regex.Replace(input.Trim(), @"[;'""]", ""); // remove quotes & semicolons
        }
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


        private void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ 1. Validate required fields
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

                // ✅ 2. Validate age
                if (!int.TryParse(txtAge.Text.Trim(), out int age))
                {
                    MessageBox.Show("Please enter a valid age.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (age < 18)
                {
                    MessageBox.Show("Age must be at least 18 years old.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (age > 50)
                {
                    MessageBox.Show("Age cannot be greater than 50 years old.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbRole.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a valid role (Staff or Cashier).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // ✅ 3. Validate password strength
                if (txtPassword.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ 4. Validate username format (no symbols)
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtUserName.Text, @"^[a-zA-Z0-9_]+$"))
                {
                    MessageBox.Show("Username can only contain letters, numbers, and underscores.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ 5. Validate Contact Number (must start with 09 and 11 digits)
                string contact = txtContact.Text.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(contact, @"^09\d{9}$"))
                {
                    MessageBox.Show("Contact number must be 11 digits and start with '09' (e.g. 09123456789).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {

                    con.Open();

                    // ✅ 6. Check if username exists
                    string checkUserQuery = "SELECT COUNT(*) FROM users WHERE Username = @Username";
                    using (var checkCmd = new MySqlCommand(checkUserQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // ✅ 7. Check if contact number already exists
                    string checkContactQuery = "SELECT COUNT(*) FROM users WHERE ContactNumber = @ContactNumber";
                    using (var checkCmd = new MySqlCommand(checkContactQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@ContactNumber", contact);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("This contact number is already registered to another user.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string fullName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}";

                    // ✅ 8. Insert query
                    string insertQuery = @"INSERT INTO users (Username, PasswordHash, FullName, Age, ContactNumber, Role, Status, DateCreated)
                       VALUES (@Username, @PasswordHash, @FullName, @Age, @ContactNumber, @Role, @Status, NOW())";


                    using (var cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", sanitize(txtUserName.Text));
                        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(txtPassword.Text.Trim()));

                        cmd.Parameters.AddWithValue("@FullName", sanitize(fullName));
                        cmd.Parameters.AddWithValue("@Age", txtAge.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", contact);
                        cmd.Parameters.AddWithValue("@Role", cmbRole.Text.Trim());
                        cmd.Parameters.AddWithValue("@Status", "Active");
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ClearUserFields();
                dgvUsers.ClearSelection();
                dgvUsers.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ 1. Make sure a user is selected
                if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.Cells["UserID"].Value == DBNull.Value)
                {
                    MessageBox.Show("Please select a user to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);

                // ✅ 2. Validate required fields
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtUserName.Text) ||
                  //  string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtAge.Text) ||
                    string.IsNullOrWhiteSpace(txtContact.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ 2. Validate age
                if (!int.TryParse(txtAge.Text.Trim(), out int age))
                {
                    MessageBox.Show("Please enter a valid age.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (age < 18)
                {
                    MessageBox.Show("Age must be at least 18 years old.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (age > 50)
                {
                    MessageBox.Show("Age cannot be greater than 50 years old.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbRole.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a valid role (Staff or Cashier).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ 4. Validate password strength
                if (txtPassword.Text.Length > 0 && txtPassword.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // ✅ 5. Validate username format
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtUserName.Text, @"^[a-zA-Z0-9_]+$"))
                {
                    MessageBox.Show("Username can only contain letters, numbers, and underscores.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ 6. Validate contact number (must be 11 digits and start with 09)
                string contact = txtContact.Text.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(contact, @"^09\d{9}$"))
                {
                    MessageBox.Show("Contact number must be 11 digits and start with '09'.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string newUsername = txtUserName.Text.Trim();
                string newPassword = txtPassword.Text.Trim();
                string newFullName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}";
                string newAge = txtAge.Text.Trim();
                string newRole = cmbRole.Text.Trim();
                string newStatus = "Active"; // default status

                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    // ✅ 7. Fetch existing user data
                    string selectQuery = "SELECT Username, PasswordHash, FullName, Age, ContactNumber, Role, Status FROM users WHERE UserID = @UserID";
                    string oldUsername = "", oldPassword = "", oldFullName = "", oldAge = "", oldContact = "", oldRole = "", oldStatus = "";

                    using (var selectCmd = new MySqlCommand(selectQuery, con))
                    {
                        selectCmd.Parameters.AddWithValue("@UserID", userId);
                        using (var reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldUsername = reader["Username"].ToString();
                                oldPassword = reader["PasswordHash"].ToString();
                                oldFullName = reader["FullName"].ToString();
                                oldAge = reader["Age"].ToString();
                                oldContact = reader["ContactNumber"].ToString();
                                oldRole = reader["Role"].ToString();
                                oldStatus = reader["Status"].ToString();
                            }
                        }
                    }

                    // ✅ 8. Check if any change is made
                    if (newUsername == oldUsername &&
                        newPassword == oldPassword &&
                        newFullName == oldFullName &&
                        newAge == oldAge &&
                        contact == oldContact &&
                        newRole == oldRole &&
                        newStatus == oldStatus)
                    {
                        MessageBox.Show("No changes detected. Please modify at least one field before updating.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // ✅ 9. Check if username already exists (exclude current user)
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE Username = @Username AND UserID <> @UserID";
                    using (var checkCmd = new MySqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", newUsername);
                        checkCmd.Parameters.AddWithValue("@UserID", userId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    // ✅ 10. Check if contact number already exists (exclude current user)
                    string checkContactQuery = "SELECT COUNT(*) FROM users WHERE ContactNumber = @ContactNumber AND UserID <> @UserID";
                    using (var checkCmd = new MySqlCommand(checkContactQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@ContactNumber", contact);
                        checkCmd.Parameters.AddWithValue("@UserID", userId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("This contact number is already registered to another user.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // ✅ 10. Update user data
                    string updateQuery = @"UPDATE users 
                                   SET Username = @Username,
                                       PasswordHash = @PasswordHash,
                                       FullName = @FullName,
                                       Age = @Age,
                                       ContactNumber = @ContactNumber,
                                       Role = @Role,
                                       Status = @Status
                                   WHERE UserID = @UserID";

                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", newUsername);
                        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(newPassword));

                        cmd.Parameters.AddWithValue("@FullName", newFullName);
                        cmd.Parameters.AddWithValue("@Age", newAge);
                        cmd.Parameters.AddWithValue("@ContactNumber", contact);
                        cmd.Parameters.AddWithValue("@Role", newRole);
                        cmd.Parameters.AddWithValue("@Status", newStatus);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }

                    // ✅ 11. Insert Audit Trail
                    ConnectionModule.InsertAuditTrail("Update", "Users", $"Updated User: {newUsername} (ID {userId})");
                }

                // ✅ 12. Success message and refresh
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ClearUserFields();
                dgvUsers.ClearSelection();
                dgvUsers.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(cmbRole.Text))
            {
                MessageBox.Show("Please select a user from the list to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvUsers.CurrentRow;

            if (row.Cells["UserID"].Value == null || row.Cells["UserID"].Value == DBNull.Value)
            {
                MessageBox.Show("Please select a valid user from the list.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(row.Cells["UserID"].Value);
            string username = row.Cells["Username"].Value.ToString();

            DialogResult result = MessageBox.Show($"Are you sure you want to deactivate user '{username}'?",
                                                  "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            try
            {
                ConnectionModule.openCon();

                // ✅ Instead of DELETE, mark as inactive
                string updateQuery = "UPDATE users SET Status = 'Inactive' WHERE UserID = @UserID";
                using (MySqlCommand cmd = new MySqlCommand(updateQuery, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.ExecuteNonQuery();
                }

                // 🧾 Insert audit trail
                ConnectionModule.InsertAuditTrail("Deactivate", "Users", $"Deactivated User: {username} (ID {userId})");

                MessageBox.Show("User deactivated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadUsers();
                ClearUserFields();
                dgvUsers.ClearSelection();
               
                dgvUsers.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deactivating user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void txtSearchUsers_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchUser.Text.Trim();

            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query = @"
                SELECT UserID, Username, PasswordHash, FullName, Age, ContactNumber, Role, Status, DateCreated
                FROM users
                WHERE Username LIKE @search
                   OR FullName LIKE @search
                   OR Role LIKE @search
                   OR Status LIKE @search
                ORDER BY DateCreated DESC";

                    using (var cmd = new MySqlCommand(query, con))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvUsers.DataSource = dt;

                        // 🔹 Hide sensitive columns
                        if (dgvUsers.Columns.Contains("UserID"))
                            dgvUsers.Columns["UserID"].Visible = false;

                        if (dgvUsers.Columns.Contains("PasswordHash"))
                            dgvUsers.Columns["PasswordHash"].Visible = false;

                        // 🔹 Adjust display headers
                        dgvUsers.Columns["Username"].HeaderText = "Username";
                        dgvUsers.Columns["FullName"].HeaderText = "Full Name";
                        dgvUsers.Columns["Age"].HeaderText = "Age";
                        dgvUsers.Columns["ContactNumber"].HeaderText = "Contact No.";
                        dgvUsers.Columns["Role"].HeaderText = "Role";
                        dgvUsers.Columns["Status"].HeaderText = "Status";
                        dgvUsers.Columns["DateCreated"].HeaderText = "Date Created";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
