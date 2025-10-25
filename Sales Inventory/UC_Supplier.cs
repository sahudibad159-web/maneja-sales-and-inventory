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
    public partial class UC_Supplier : UserControl
    {
        public UC_Supplier()
        {
            InitializeComponent();
            UC_Supplier_Load(null, null);
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void UC_Supplier_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            StyleDataGridView(dgvSuppliers);
            // Para walang naka-select agad
            dgvSuppliers.ClearSelection();
            dgvSuppliers.CurrentCell = null;


            // CompanyName → bawal number, letters + space + . , - allowed
            txtCompanyName.KeyPress += BlockInvalidCharacters_KeyPress;
            txtCompanyName.KeyPress += BlockNumbers_KeyPress;

            // SupplierName → same rules as CompanyName
           
            

            // ContactNumber → digits only
            txtContactNumber.KeyPress += DigitsOnly_KeyPress;

            // Address → letters + numbers + space + . , - allowed (pero bawal paste pa rin)
            txtAddress.KeyPress += BlockAddressCharacters_KeyPress;

            // Bawal mag-right click copy/paste
            txtSupplierName.ContextMenu = new ContextMenu();
            txtCompanyName.ContextMenu = new ContextMenu();
            txtContactNumber.ContextMenu = new ContextMenu();
            txtAddress.ContextMenu = new ContextMenu();

            // Bawal Ctrl+V / Ctrl+C / Ctrl+X / Shift+Insert / Ctrl+Insert
            txtAddress.KeyDown += BlockCopyPaste_KeyDown;
            txtContactNumber.KeyDown += BlockCopyPaste_KeyDown;
            txtSupplierName.KeyDown += BlockCopyPaste_KeyDown;
            txtCompanyName.KeyDown += BlockCopyPaste_KeyDown;

            // Auto-normalize spaces kapag iniwan ang textbox
            txtSupplierName.Leave += NormalizeSpaces;
            txtCompanyName.Leave += NormalizeSpaces;
            txtContactNumber.Leave += NormalizeSpaces;
            txtAddress.Leave += NormalizeSpaces;

            // Disable shortcuts
            txtSupplierName.ShortcutsEnabled = false;
            txtCompanyName.ShortcutsEnabled = false;
            txtContactNumber.ShortcutsEnabled = false;
            txtAddress.ShortcutsEnabled = false;


        }
        private void NormalizeSpaces(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = Regex.Replace(tb.Text.Trim(), @"\s+", " ");
            }
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

        // 🔹 Bawal numbers (para sa CompanyName, SupplierName)
        private void BlockNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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

        // 🔹 Letters + numbers + space + ., - allowed (Address)
        private void BlockAddressCharacters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) &&
                !char.IsControl(e.KeyChar) &&
                !char.IsWhiteSpace(e.KeyChar) &&
                e.KeyChar != '.' &&
                e.KeyChar != ',' &&
                e.KeyChar != '-')
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
            // Center align content horizontally and vertically in all cells
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            // Selection style
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Disable adding rows by user
            dgv.AllowUserToAddRows = false;

            // Disable row headers
            dgv.RowHeadersVisible = false;

            // Columns fill evenly across available width
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.ScrollBars = ScrollBars.Vertical;
            // Pantay ang columns at minimum width
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 100; // adjust kung gusto mo mas malaki
                col.FillWeight = 1;     // pantay ang distribution
            }

            // Single row selection
            dgv.MultiSelect = false;

            // Optional: alternating row colors for better readability
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
        }

        private void LoadSuppliers()
        {
            try
            {
                ConnectionModule.openCon();
                string query = "SELECT SupplierID, SupplierName, ContactPerson, ContactNumber, Address FROM Supplier"; // ← kasama na SupplierID
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSuppliers.DataSource = dt;

                // Itago ang SupplierID column para di makita ng user
                if (dgvSuppliers.Columns.Contains("SupplierID"))
                    dgvSuppliers.Columns["SupplierID"].Visible = false;

             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading suppliers: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }





      

        private void btnDelete_Click(object sender, EventArgs e)
        {
          
        }


        int lastSelectedSupplierRowIndex = -1; // 👈 Declare this at class level (top of form)

        private void dgvSuppliers_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 👇 Check if user clicked on a valid row
                if (e.RowIndex >= 0)
                {
                    // 👇 If same row clicked again → UNSELECT
                    if (e.RowIndex == lastSelectedSupplierRowIndex)
                    {
                        dgvSuppliers.ClearSelection();
                        ClearFields();
                        lastSelectedSupplierRowIndex = -1;
                        return;
                    }

                    // ✅ Otherwise, select and show data
                    DataGridViewRow row = dgvSuppliers.Rows[e.RowIndex];

                    if (row.Cells["SupplierName"].Value == DBNull.Value ||
                        string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["SupplierName"].Value)))
                    {
                        ClearFields();
                        return;
                    }

                    txtSupplierName.Text = row.Cells["SupplierName"].Value.ToString();
                    txtCompanyName.Text = row.Cells["ContactPerson"].Value.ToString();
                    txtContactNumber.Text = row.Cells["ContactNumber"].Value.ToString();
                    txtAddress.Text = row.Cells["Address"].Value.ToString();

                    lastSelectedSupplierRowIndex = e.RowIndex;
                }
                else
                {
                    // 👇 If clicked outside rows (blank area)
                    dgvSuppliers.ClearSelection();
                    ClearFields();
                    lastSelectedSupplierRowIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearFields()
        {
            txtSupplierName.Clear();
            txtCompanyName.Clear();
            txtContactNumber.Clear();
            txtAddress.Clear();
        }

        private void txtSearchSupplier_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchSupplier.Text.Trim();

            try
            {
                ConnectionModule.openCon();
                string query = @"SELECT SupplierID, SupplierName, ContactPerson, ContactNumber, Address 
                         FROM Supplier 
                         WHERE SupplierName LIKE @search 
                            OR ContactPerson LIKE @search 
                            OR ContactNumber LIKE @search 
                            OR Address LIKE @search";

                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Minimum rows logic (consistent DataGridView height)
                int minRows = 5;
                while (dt.Rows.Count < minRows)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                dgvSuppliers.DataSource = dt;

                // Hide SupplierID column
                if (dgvSuppliers.Columns.Contains("SupplierID"))
                    dgvSuppliers.Columns["SupplierID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching suppliers: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Normalize input (trim + remove double spaces)
            string supplierName = System.Text.RegularExpressions.Regex.Replace(txtSupplierName.Text.Trim(), @"\s+", " ");
            string contactPerson = System.Text.RegularExpressions.Regex.Replace(txtCompanyName.Text.Trim(), @"\s+", " ");
            string contactNumber = System.Text.RegularExpressions.Regex.Replace(txtContactNumber.Text.Trim(), @"\s+", " ");
            string address = System.Text.RegularExpressions.Regex.Replace(txtAddress.Text.Trim(), @"\s+", " ");

            // ✅ Required field check
            if (string.IsNullOrWhiteSpace(supplierName) ||
                string.IsNullOrWhiteSpace(contactPerson) ||
                string.IsNullOrWhiteSpace(contactNumber) ||
                string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("All fields are required. Please fill in all supplier details.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Contact number format validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(contactNumber, @"^09\d{9}$"))
            {
                MessageBox.Show("Invalid contact number. It must start with '09' and contain exactly 11 digits.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConnectionModule.openCon();

                // 🔍 Check duplicate SupplierName
                string checkNameQuery = "SELECT COUNT(*) FROM Supplier WHERE SupplierName=@SupplierName";
                MySqlCommand checkNameCmd = new MySqlCommand(checkNameQuery, ConnectionModule.con);
                checkNameCmd.Parameters.AddWithValue("@SupplierName", supplierName);
                int nameCount = Convert.ToInt32(checkNameCmd.ExecuteScalar());

                if (nameCount > 0)
                {
                    MessageBox.Show("A supplier with the same name already exists.",
                                    "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 🔍 Check duplicate ContactNumber
                string checkNumberQuery = "SELECT COUNT(*) FROM Supplier WHERE ContactNumber=@ContactNumber";
                MySqlCommand checkNumberCmd = new MySqlCommand(checkNumberQuery, ConnectionModule.con);
                checkNumberCmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                int numberCount = Convert.ToInt32(checkNumberCmd.ExecuteScalar());

                if (numberCount > 0)
                {
                    MessageBox.Show("A supplier with the same contact number already exists.",
                                    "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Insert new Supplier
                string insertQuery = @"INSERT INTO Supplier (SupplierName, ContactPerson, ContactNumber, Address)
                               VALUES (@SupplierName, @ContactPerson, @ContactNumber, @Address)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, ConnectionModule.con);
                insertCmd.Parameters.AddWithValue("@SupplierName", supplierName);
                insertCmd.Parameters.AddWithValue("@ContactPerson", contactPerson);
                insertCmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                insertCmd.Parameters.AddWithValue("@Address", address);
                insertCmd.ExecuteNonQuery();
                MessageBox.Show("Category saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🧾 Audit Trail
                string details = $"Added Supplier → Name: {supplierName}, Contact Person: {contactPerson}, " +
                                 $"Contact Number: {contactNumber}, Address: {address}";
                ConnectionModule.InsertAuditTrail("Insert", "Supplier", details);

                LoadSuppliers();
                ClearFields();
                dgvSuppliers.ClearSelection();
                dgvSuppliers.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving supplier: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.CurrentRow == null || dgvSuppliers.CurrentRow.Cells["SupplierID"].Value == DBNull.Value)
            {
                MessageBox.Show("Please select a supplier to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idSupplier = Convert.ToInt32(dgvSuppliers.CurrentRow.Cells["SupplierID"].Value);
            string oldSupplierName = dgvSuppliers.CurrentRow.Cells["SupplierName"].Value.ToString();
            string oldContactPerson = dgvSuppliers.CurrentRow.Cells["ContactPerson"].Value.ToString();
            string oldContactNumber = dgvSuppliers.CurrentRow.Cells["ContactNumber"].Value.ToString();
            string oldAddress = dgvSuppliers.CurrentRow.Cells["Address"].Value.ToString();

            string newSupplierName = System.Text.RegularExpressions.Regex.Replace(txtSupplierName.Text.Trim(), @"\s+", " ");
            string newContactPerson = System.Text.RegularExpressions.Regex.Replace(txtCompanyName.Text.Trim(), @"\s+", " ");
            string newContactNumber = System.Text.RegularExpressions.Regex.Replace(txtContactNumber.Text.Trim(), @"\s+", " ");
            string newAddress = System.Text.RegularExpressions.Regex.Replace(txtAddress.Text.Trim(), @"\s+", " ");

            // ✅ Validation (required fields)
            if (string.IsNullOrWhiteSpace(newSupplierName) ||
                string.IsNullOrWhiteSpace(newContactPerson) ||
                string.IsNullOrWhiteSpace(newContactNumber) ||
                string.IsNullOrWhiteSpace(newAddress))
            {
                MessageBox.Show("All fields are required for update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Contact number validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(newContactNumber, @"^\d{7,15}$"))
            {
                MessageBox.Show("Invalid contact number format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Detect no changes
            if (oldSupplierName == newSupplierName &&
                oldContactPerson == newContactPerson &&
                oldContactNumber == newContactNumber &&
                oldAddress == newAddress)
            {
                MessageBox.Show("No changes detected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ConnectionModule.openCon();

                // 🔍 Check for duplicates (exclude current record)
                string checkQuery = "SELECT COUNT(*) FROM Supplier WHERE SupplierName=@SupplierName AND SupplierID<>@SupplierID";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, ConnectionModule.con);
                checkCmd.Parameters.AddWithValue("@SupplierName", newSupplierName);
                checkCmd.Parameters.AddWithValue("@SupplierID", idSupplier);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Another supplier with the same name already exists.",
                                    "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Proceed update
                string updateQuery = @"UPDATE Supplier 
                               SET SupplierName=@SupplierName, ContactPerson=@ContactPerson,
                                   ContactNumber=@ContactNumber, Address=@Address
                               WHERE SupplierID=@SupplierID";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, ConnectionModule.con);
                updateCmd.Parameters.AddWithValue("@SupplierName", newSupplierName);
                updateCmd.Parameters.AddWithValue("@ContactPerson", newContactPerson);
                updateCmd.Parameters.AddWithValue("@ContactNumber", newContactNumber);
                updateCmd.Parameters.AddWithValue("@Address", newAddress);
                updateCmd.Parameters.AddWithValue("@SupplierID", idSupplier);
                updateCmd.ExecuteNonQuery();

                // 🧾 Audit Trail (field-level)
                string changes = $"[SupplierID: {idSupplier}] ";
                if (oldSupplierName != newSupplierName)
                    changes += $"Name: {oldSupplierName} → {newSupplierName}; ";
                if (oldContactPerson != newContactPerson)
                    changes += $"Contact Person: {oldContactPerson} → {newContactPerson}; ";
                if (oldContactNumber != newContactNumber)
                    changes += $"Contact Number: {oldContactNumber} → {newContactNumber}; ";
                if (oldAddress != newAddress)
                    changes += $"Address: {oldAddress} → {newAddress}; ";

                ConnectionModule.InsertAuditTrail("Update", "Supplier", $"Updated Supplier {changes}");

                MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSuppliers();
                ClearFields();
                dgvSuppliers.ClearSelection();
                dgvSuppliers.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating supplier: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            // 1️⃣ Validation: may laman ba ang mga textbox (ibig sabihin may napiling supplier?)
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text) ||
                string.IsNullOrWhiteSpace(txtCompanyName.Text) ||
                string.IsNullOrWhiteSpace(txtContactNumber.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please select a supplier from the list to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2️⃣ Kunin ang selected row
            if (dgvSuppliers.CurrentRow == null)
            {
                MessageBox.Show("Please select a supplier first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvSuppliers.CurrentRow;

            // 3️⃣ Kunin SupplierID galing sa DGV
            if (row.Cells["SupplierID"].Value == null ||
       row.Cells["SupplierID"].Value == DBNull.Value ||
       string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["SupplierID"].Value)))
            {
                MessageBox.Show("Please select a valid supplier from the list.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            int supplierID = Convert.ToInt32(row.Cells["SupplierID"].Value);
            string supplierName = row.Cells["SupplierName"].Value.ToString();

            // 4️⃣ Confirm deletion
            DialogResult result = MessageBox.Show($"Are you sure you want to delete supplier '{supplierName}'?",
                                                  "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            try
            {
                ConnectionModule.openCon();

                // 5️⃣ Optional: check kung may related deliveries
                string checkQuery = "SELECT COUNT(*) FROM delivery WHERE SupplierID=@SupplierID";

                MySqlCommand checkCmd = new MySqlCommand(checkQuery, ConnectionModule.con);
                checkCmd.Parameters.AddWithValue("@SupplierID", supplierID);
                int usedCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (usedCount > 0)
                {
                    MessageBox.Show("Cannot delete this supplier because it is linked to existing deliveries.",
                                    "Delete Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 6️⃣ Proceed delete
                string deleteQuery = "DELETE FROM Supplier WHERE SupplierID=@SupplierID";
                MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, ConnectionModule.con);
                deleteCmd.Parameters.AddWithValue("@SupplierID", supplierID);
                deleteCmd.ExecuteNonQuery();

                ConnectionModule.InsertAuditTrail("Delete", "Supplier", $"Deleted Supplier: {supplierName} (ID {supplierID})");
                MessageBox.Show("Supplier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadSuppliers();
                ClearFields();
                dgvSuppliers.ClearSelection();
                dgvSuppliers.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting supplier: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }
    }

}
