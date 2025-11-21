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
using System.Text.RegularExpressions;

namespace Sales_Inventory
{
    public partial class UC_Category : UserControl
    {
        public UC_Category()
        {
            InitializeComponent();
            UC_Category_Load(null, null); // tawagin agad after load
            StyleDataGridView(dgv);
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

            // Columns fill evenly across available width (ok lang ito kahit vertical scroll lang)
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Scrollbars → Vertical only
            dgv.ScrollBars = ScrollBars.Vertical;

            // Pantay ang columns at minimum width
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 100;
                col.FillWeight = 1;
            }
        }
    // Single row selection




        private void UC_LoadCategory()
        {
            try
            {
                ConnectionModule.openCon();
                string query = "SELECT CategoryID, CategoryName, Description FROM Category"; // <- kasama na ID
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;

                // Itago ang ID column para di makita ng user
                if (dgv.Columns.Contains("CategoryID"))
                    dgv.Columns["CategoryID"].Visible = false;

                // Optional: minimum rows
                int minRows = 5;
                while (dgv.Rows.Count < minRows)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                ConnectionModule.closeCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }


        private void UC_Category_Load(object sender, EventArgs e)
        {
            UC_LoadCategory();
            // Para walang naka-select agad
            dgv.ClearSelection();
            dgv.CurrentCell = null;


           
            // Bawal mag-type ng number
            txtCategoryName.KeyPress += BlockNumbers_KeyPress;
            txtDescription.KeyPress += BlockNumbers_KeyPress;

           
          

            // Bawal mag-right click copy/paste
            txtCategoryName.ContextMenu = new ContextMenu();
            txtDescription.ContextMenu = new ContextMenu();

            // Bawal Ctrl+V / Ctrl+C / Ctrl+X / Shift+Insert / Ctrl+Insert
            txtCategoryName.KeyDown += BlockCopyPaste_KeyDown;
            txtDescription.KeyDown += BlockCopyPaste_KeyDown;

            txtDescription.KeyPress += BlockMultipleSpaces_KeyPress;

            // Auto-normalize spaces kapag iniwan ang textbox
            txtCategoryName.Leave += NormalizeSpaces;
            txtDescription.Leave += NormalizeSpaces;


            txtCategoryName.ShortcutsEnabled = false;
            txtDescription.ShortcutsEnabled = false;

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
       


        private void ClearFields()
        {
            txtCategoryName.Clear();
            txtDescription.Clear();
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

     


        private void dgv_Click(object sender, EventArgs e)
        {

        }

        private int lastSelectedRowIndex = -1; // 👈 declare this at the top of your form (class level)

        private void dgv_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 👇 Check if user clicked on a valid row
                if (e.RowIndex >= 0)
                {
                    // If same row clicked again → UNSELECT
                    if (e.RowIndex == lastSelectedRowIndex)
                    {
                        dgv.ClearSelection();
                        txtCategoryName.Clear();
                        txtDescription.Clear();
                        lastSelectedRowIndex = -1;
                        return;
                    }

                    // ✅ Otherwise, select and show data
                    DataGridViewRow row = dgv.Rows[e.RowIndex];
                    txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();
                    txtDescription.Text = row.Cells["Description"].Value.ToString();

                    lastSelectedRowIndex = e.RowIndex;
                }
                else
                {
                    // 👇 If clicked outside the rows (like blank area)
                    dgv.ClearSelection();
                    txtCategoryName.Clear();
                    txtDescription.Clear();
                    lastSelectedRowIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting category: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {   // Normalize input: trim + collapse multiple spaces
            string categoryName = System.Text.RegularExpressions.Regex.Replace(txtCategoryName.Text.Trim(), @"\s+", " ");
            string description = System.Text.RegularExpressions.Regex.Replace(txtDescription.Text.Trim(), @"\s+", " ");

            // Validation
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Please enter a Category Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please enter a Description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConnectionModule.openCon();

                // Check duplicate
                string checkQuery = "SELECT COUNT(*) FROM Category WHERE CategoryName=@CategoryName";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, ConnectionModule.con);
                checkCmd.Parameters.AddWithValue("@CategoryName", categoryName);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Duplicate category found! Category Name already exists.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Insert
                string insertQuery = "INSERT INTO Category (CategoryName, Description) VALUES (@CategoryName, @Description)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, ConnectionModule.con);
                insertCmd.Parameters.AddWithValue("@CategoryName", categoryName);
                insertCmd.Parameters.AddWithValue("@Description", description);
                insertCmd.ExecuteNonQuery();

                MessageBox.Show("Category saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 📝 Audit trail with clearer details
                ConnectionModule.InsertAuditTrail(
                    "Insert",
                    "Category",
                    $"Added new category → Name: {categoryName}, Description: {description}"
                );

                UC_LoadCategory();
                ClearFields();
                dgv.ClearSelection();
                dgv.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving category: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }

        }

        private void btnUpdate_Click_2(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Cells["CategoryID"].Value == DBNull.Value)
            {
                MessageBox.Show("Please select a valid category to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Old values galing sa DGV
            int categoryId = Convert.ToInt32(dgv.CurrentRow.Cells["CategoryID"].Value);
            string oldCategoryName = dgv.CurrentRow.Cells["CategoryName"].Value.ToString();
            string oldDescription = dgv.CurrentRow.Cells["Description"].Value.ToString();

            // New values galing sa textboxes
            string newCategoryName = System.Text.RegularExpressions.Regex.Replace(txtCategoryName.Text.Trim(), @"\s+", " ");
            string newDescription = System.Text.RegularExpressions.Regex.Replace(txtDescription.Text.Trim(), @"\s+", " ");

            // ✅ Validation
            if (string.IsNullOrWhiteSpace(newCategoryName))
            {
                MessageBox.Show("Category Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                MessageBox.Show("Description cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Check kung may binago
            if (oldCategoryName == newCategoryName && oldDescription == newDescription)
            {
                MessageBox.Show("No changes detected. Update cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ConnectionModule.openCon();

                // 🔍 Check duplicate
                string checkQuery = "SELECT COUNT(*) FROM Category WHERE CategoryName=@CategoryName AND CategoryID<>@CategoryID";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, ConnectionModule.con);
                checkCmd.Parameters.AddWithValue("@CategoryName", newCategoryName);
                checkCmd.Parameters.AddWithValue("@CategoryID", categoryId);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Duplicate category found! Category Name already exists.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Update query
                string query = "UPDATE Category SET CategoryName=@CategoryName, Description=@Description WHERE CategoryID=@CategoryID";
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                cmd.Parameters.AddWithValue("@CategoryName", newCategoryName);
                cmd.Parameters.AddWithValue("@Description", newDescription);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                cmd.ExecuteNonQuery();



                // 📝 Audit Trail (per field)
                string categoryRef = $"[Category: {newCategoryName}]";

                if (oldCategoryName != newCategoryName)
                    ConnectionModule.InsertAuditTrail("Update", "Category", $"{categoryRef} → Updated CategoryName: {oldCategoryName} → {newCategoryName}");

                if (oldDescription != newDescription)
                    ConnectionModule.InsertAuditTrail("Update", "Category", $"{categoryRef} → Updated Description: {oldDescription} → {newDescription}");


                MessageBox.Show("Category updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating category: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
                UC_LoadCategory();
                ClearFields();
                dgv.ClearSelection();
                dgv.CurrentCell = null;
            }
        }

        private void btnDelete_Click_2(object sender, EventArgs e)
        {

            // 1️⃣ Validation: dapat may laman lahat ng textbox (ibig sabihin may napiling category)
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please select a category from the list to delete.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2️⃣ Validation: may napiling row sa DGV?
            if (dgv.CurrentRow == null || dgv.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Please select a category first.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgv.CurrentRow;

            // 3️⃣ Validation: valid ba ang CategoryID?
            if (row.Cells["CategoryID"].Value == null ||
                row.Cells["CategoryID"].Value == DBNull.Value ||
                string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["CategoryID"].Value)))
            {
                MessageBox.Show("Please select a valid category from the list.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int categoryID = Convert.ToInt32(row.Cells["CategoryID"].Value);
            string categoryName = row.Cells["CategoryName"].Value.ToString();

            // 4️⃣ Confirm deletion
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete the category '{categoryName}'?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            try
            {
                ConnectionModule.openCon();

                // 5️⃣ Check muna kung ginagamit sa Product table
                string checkQuery = "SELECT COUNT(*) FROM Product WHERE CategoryID = @CategoryID";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, ConnectionModule.con))
                {
                    checkCmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    int usedCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (usedCount > 0)
                    {
                        MessageBox.Show($"Category '{categoryName}' is in use and cannot be deleted.",
                            "Delete Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // 6️⃣ Proceed delete
                string deleteQuery = "DELETE FROM Category WHERE CategoryID = @CategoryID";
                using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, ConnectionModule.con))
                {
                    deleteCmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    deleteCmd.ExecuteNonQuery();
                }

                // 7️⃣ Audit + Success message
                ConnectionModule.InsertAuditTrail("Delete", "Category", $"Deleted Category: {categoryName} (ID {categoryID})");
                MessageBox.Show("Category deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 8️⃣ Refresh and clear fields
                UC_LoadCategory();
                ClearFields();
                dgv.ClearSelection();
                dgv.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting category: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void txtSearchCategory_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchCategory.Text.Trim();

            try
            {
                ConnectionModule.openCon();
                string query = @"SELECT CategoryID, CategoryName, Description 
                         FROM Category 
                         WHERE CategoryName LIKE @search OR Description LIKE @search";

                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Minimum rows logic
                int minRows = 5;
                while (dt.Rows.Count < minRows)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                dgv.DataSource = dt;

                // Hide ID column
                if (dgv.Columns.Contains("CategoryID"))
                    dgv.Columns["CategoryID"].Visible = false;

                ConnectionModule.closeCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching categories: " + ex.Message);
            }
        }
    }
}
