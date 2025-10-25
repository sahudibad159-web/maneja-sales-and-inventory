using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
namespace Sales_Inventory
{

    public partial class UC_Products : UserControl
    {

        public UC_Products()
        {
            InitializeComponent();
            UC_Product_Load(null, null);
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void UC_Product_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadProducts();
            StyleDataGridView(dgvProducts);  // ilagay ang pangalan ng DGV mo
                                             // Para walang naka-select agad
            dgvProducts.ClearSelection();
            dgvProducts.CurrentCell = null;


            // CompanyName → bawal number, letters + space + . , - allowed
            //   txt.KeyPress += BlockInvalidCharacters_KeyPress;
            // txtCompanyName.KeyPress += BlockNumbers_KeyPress;

            // SupplierName → same rules as CompanyName



            // ContactNumber → digits only
            txtBarcode.KeyPress += DigitsOnly_KeyPress;
            txtReorderLevel.KeyPress += DigitsOnly_KeyPress;
            txtReorderQuantity.KeyPress += DigitsOnly_KeyPress;
            txtRetailPrice.KeyPress += DigitsOnly_KeyPress;
            txtWholeSalePrice.KeyPress += DigitsOnly_KeyPress;

            // Address → letters + numbers + space + . , - allowed (pero bawal paste pa rin)
          //  txtAddress.KeyPress += BlockAddressCharacters_KeyPress;

            // Bawal mag-right click copy/paste
            txtBarcode.ContextMenu = new ContextMenu();
            txtDescription.ContextMenu = new ContextMenu();
            txtProductName.ContextMenu = new ContextMenu();
            txtReorderLevel.ContextMenu = new ContextMenu();
            txtReorderQuantity.ContextMenu = new ContextMenu();
            txtRetailPrice.ContextMenu = new ContextMenu();
            txtWholeSalePrice.ContextMenu = new ContextMenu();

            // Bawal Ctrl+V / Ctrl+C / Ctrl+X / Shift+Insert / Ctrl+Insert
            txtBarcode.KeyDown += BlockCopyPaste_KeyDown;
            txtDescription.KeyDown += BlockCopyPaste_KeyDown;
            txtProductName.KeyDown += BlockCopyPaste_KeyDown;
            txtReorderLevel.KeyDown += BlockCopyPaste_KeyDown;
            txtReorderQuantity.KeyDown += BlockCopyPaste_KeyDown;
            txtRetailPrice.KeyDown += BlockCopyPaste_KeyDown;
            txtWholeSalePrice.KeyDown += BlockCopyPaste_KeyDown;


            txtProductName.KeyPress += BlockMultipleSpaces_KeyPress;
            txtDescription.KeyPress += BlockMultipleSpaces_KeyPress;
            txtProductName.KeyPress += BlockMultipleSpaces_KeyPress;
            txtReorderLevel.KeyPress += BlockMultipleSpaces_KeyPress;
            txtReorderQuantity.KeyPress += BlockMultipleSpaces_KeyPress;
            txtRetailPrice.KeyPress += BlockMultipleSpaces_KeyPress;
            txtWholeSalePrice.KeyPress += BlockMultipleSpaces_KeyPress;

            //// Auto-normalize spaces kapag iniwan ang textbox
            txtBarcode.TextChanged += NormalizeSpaces_OnChange;
            txtDescription.TextChanged += NormalizeSpaces_OnChange;
            txtProductName.TextChanged += NormalizeSpaces_OnChange;
            txtReorderLevel.TextChanged += NormalizeSpaces_OnChange;
            txtReorderQuantity.TextChanged += NormalizeSpaces_OnChange;
            txtRetailPrice.TextChanged += NormalizeSpaces_OnChange;
            txtWholeSalePrice.TextChanged += NormalizeSpaces_OnChange;

            // Disable shortcuts
            txtBarcode.ShortcutsEnabled = false;
            txtDescription.ShortcutsEnabled = false;
            txtProductName.ShortcutsEnabled = false;
            txtReorderLevel.ShortcutsEnabled = false;
            txtReorderQuantity.ShortcutsEnabled = false;
            txtRetailPrice.ShortcutsEnabled = false;
            txtWholeSalePrice.ShortcutsEnabled = false;

            txtBarcode.MaxLength = 13; // prevents typing more than 13 chars
            txtBarcode.KeyPress += txtBarcode_KeyPress;
            txtBarcode.TextChanged += txtBarcode_TextChanged;
         

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

        // 🔹 Para siguradong isang space lang kahit paste
        private void NormalizeSpaces_OnChange(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                int cursor = tb.SelectionStart;

                // Palitan lahat ng multiple spaces → single space
                string newText = Regex.Replace(tb.Text, @"\s{2,}", " ");

                if (tb.Text != newText)
                {
                    tb.Text = newText;
                    tb.SelectionStart = Math.Min(cursor, tb.Text.Length); // ibalik cursor
                }
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


        private void LoadCategories()
        {
            try
            {
                ConnectionModule.openCon();
                string query = "SELECT CategoryID, CategoryName FROM Category";
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
                cmbCategory.DataSource = dt;
                cmbCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void LoadProducts()
        {
            try
            {
                ConnectionModule.openCon();
                string query = @"
        SELECT  
            p.ProductID,          
            p.Barcode, 
            p.ProductName, 
            p.CategoryID,
            c.CategoryName, 
            p.Description, 
            p.RetailPrice, 
            p.WholeSalePrice, 
            p.ReorderLevel
        FROM Product p
        INNER JOIN Category c ON p.CategoryID = c.CategoryID
        WHERE p.isActive = 1";  // ✅ only show active products

                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvProducts.DataSource = dt;

                dgvProducts.Columns["CategoryName"].HeaderText = "Category";
                dgvProducts.Columns["WholeSalePrice"].HeaderText = "Wholesale Price";
                dgvProducts.Columns["RetailPrice"].HeaderText = "Retail Price";
                dgvProducts.Columns["ReorderLevel"].HeaderText = "Reorder Level";

                if (dgvProducts.Columns.Contains("ProductID"))
                    dgvProducts.Columns["ProductID"].Visible = false;
                if (dgvProducts.Columns.Contains("CategoryID"))
                    dgvProducts.Columns["CategoryID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }


      

        // Allow digits and control keys only
        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // reject non-digit input
            }
        }

        // Handle paste and enforce digits-only + max length
        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            // keep only digits
            string digitsOnly = Regex.Replace(txtBarcode.Text, @"\D", "");

            // trim to max 13 if pasted longer string
            if (digitsOnly.Length > 13)
                digitsOnly = digitsOnly.Substring(0, 13);

            if (txtBarcode.Text != digitsOnly)
            {
                int sel = txtBarcode.SelectionStart;
                txtBarcode.Text = digitsOnly;
                txtBarcode.SelectionStart = Math.Min(sel, txtBarcode.Text.Length);
            }
        }

        // Validate on leaving the field (prevents moving focus if invalid)

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Normalize inputs
            string productName = Regex.Replace(txtProductName.Text.Trim(), @"\s+", " ");
            string barcode = Regex.Replace(txtBarcode.Text.Trim(), @"\s+", " ");
            string category = Regex.Replace(cmbCategory.Text.Trim(), @"\s+", " ");
            string description = Regex.Replace(txtDescription.Text.Trim(), @"\s+", " ");
            string retailPrice = Regex.Replace(txtRetailPrice.Text.Trim(), @"\s+", " ");
            string wholesalePrice = Regex.Replace(txtWholeSalePrice.Text.Trim(), @"\s+", " ");
            string reorderLevel = Regex.Replace(txtReorderLevel.Text.Trim(), @"\s+", " ");

            // Required fields check
            if (string.IsNullOrWhiteSpace(barcode) ||
                string.IsNullOrWhiteSpace(productName) ||
                string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(retailPrice) ||
                string.IsNullOrWhiteSpace(wholesalePrice) ||
                string.IsNullOrWhiteSpace(reorderLevel))
            {
                MessageBox.Show("Please fill out all required fields before saving.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Barcode must be 13 digits
            if (!Regex.IsMatch(barcode, @"^\d{13}$"))
            {
                MessageBox.Show("Barcode must be exactly 13 digits (numbers only).",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBarcode.Focus();
                return;
            }

            // Numeric validation
            if (!decimal.TryParse(retailPrice, out decimal retail) || retail <= 0)
            {
                MessageBox.Show("Retail Price must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRetailPrice.Focus();
                return;
            }

            if (!decimal.TryParse(wholesalePrice, out decimal wholesale) || wholesale <= 0)
            {
                MessageBox.Show("Wholesale Price must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWholeSalePrice.Focus();
                return;
            }

            if (!int.TryParse(reorderLevel, out int rLevel) || rLevel < 0)
            {
                MessageBox.Show("Reorder Level cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReorderLevel.Focus();
                return;
            }

            // Extra check: Retail Price must be >= Wholesale Price
            if (retail <= wholesale)
            {
                MessageBox.Show($"Retail Price (₱{retail:0.00}) must be greater than Wholesale Price (₱{wholesale:0.00}).",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRetailPrice.Focus();
                return;
            }


            try
            {
                ConnectionModule.openCon();

                // ✅ Check if category exists
                string checkCat = "SELECT COUNT(*) FROM Category WHERE CategoryName=@cat";
                using (MySqlCommand catCmd = new MySqlCommand(checkCat, ConnectionModule.con))
                {
                    catCmd.Parameters.AddWithValue("@cat", category);
                    if (Convert.ToInt32(catCmd.ExecuteScalar()) == 0)
                    {
                        MessageBox.Show("Invalid category selected.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // ✅ Check duplicate ProductName + Description (case-insensitive)
                string checkDuplicateNameDesc = @"
        SELECT COUNT(*) 
        FROM Product 
        WHERE LOWER(ProductName) = LOWER(@ProductName) 
          AND LOWER(Description) = LOWER(@Description) 
          AND IsActive = 1
          AND Barcode != @Barcode"; // Exclude current barcode for editing

                using (MySqlCommand dupCmd = new MySqlCommand(checkDuplicateNameDesc, ConnectionModule.con))
                {
                    dupCmd.Parameters.AddWithValue("@ProductName", productName.Trim());
                    dupCmd.Parameters.AddWithValue("@Description", description.Trim());
                    dupCmd.Parameters.AddWithValue("@Barcode", barcode.Trim());

                    int duplicateCount = Convert.ToInt32(dupCmd.ExecuteScalar());
                    if (duplicateCount > 0)
                    {
                        MessageBox.Show(
                            "A product with the same name and description already exists. Cannot add duplicate even with a different barcode.",
                            "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // ✅ Check if barcode exists and get IsActive
                string checkDup = "SELECT IsActive FROM Product WHERE Barcode=@Barcode";
                using (MySqlCommand checkCmd = new MySqlCommand(checkDup, ConnectionModule.con))
                {
                    checkCmd.Parameters.AddWithValue("@Barcode", barcode);
                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        bool isActive = Convert.ToBoolean(result);

                        if (isActive)
                        {
                            MessageBox.Show("Duplicate Barcode found! Barcode already exists and is active.",
                                            "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            // Reactivate inactive product
                            string reactivateQuery = @"UPDATE Product
                SET ProductName=@ProductName,
                    CategoryID=(SELECT CategoryID FROM Category WHERE CategoryName=@CategoryName),
                    Description=@Description,
                    RetailPrice=@RetailPrice,
                    WholeSalePrice=@WholeSalePrice,
                    ReorderLevel=@ReorderLevel,
                    IsActive=1
                WHERE Barcode=@Barcode";

                            using (MySqlCommand reactivateCmd = new MySqlCommand(reactivateQuery, ConnectionModule.con))
                            {
                                reactivateCmd.Parameters.AddWithValue("@Barcode", barcode);
                                reactivateCmd.Parameters.AddWithValue("@ProductName", productName);
                                reactivateCmd.Parameters.AddWithValue("@CategoryName", category);
                                reactivateCmd.Parameters.AddWithValue("@Description", description);
                                reactivateCmd.Parameters.AddWithValue("@RetailPrice", retail);
                                reactivateCmd.Parameters.AddWithValue("@WholeSalePrice", wholesale);
                                reactivateCmd.Parameters.AddWithValue("@ReorderLevel", rLevel);
                                reactivateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Inactive product reactivated successfully!",
                                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        // Insert new product
                        string insertQuery = @"INSERT INTO Product 
            (Barcode, ProductName, CategoryID, Description, RetailPrice, WholeSalePrice, ReorderLevel, IsActive)
            VALUES (@Barcode, @ProductName, 
                    (SELECT CategoryID FROM Category WHERE CategoryName=@CategoryName), 
                    @Description, @RetailPrice, @WholeSalePrice, @ReorderLevel, 1)";

                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, ConnectionModule.con))
                        {
                            insertCmd.Parameters.AddWithValue("@Barcode", barcode);
                            insertCmd.Parameters.AddWithValue("@ProductName", productName);
                            insertCmd.Parameters.AddWithValue("@CategoryName", category);
                            insertCmd.Parameters.AddWithValue("@Description", description);
                            insertCmd.Parameters.AddWithValue("@RetailPrice", retail);
                            insertCmd.Parameters.AddWithValue("@WholeSalePrice", wholesale);
                            insertCmd.Parameters.AddWithValue("@ReorderLevel", rLevel);
                            insertCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Product saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 📝 Audit Trail (Insert)
                        string details = $"[Barcode: {barcode}] | Product: {productName} | Category: {category} | Retail: ₱{retail:0.00} | Wholesale: ₱{wholesale:0.00} | Reorder Level: {rLevel}";
                        ConnectionModule.InsertAuditTrail("Insert", "Product", details);
                    }
                }

                LoadProducts();
                ClearFields();
                dgvProducts.ClearSelection();
                dgvProducts.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Please select a valid product to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Normalize inputs
            string newBarcode = Regex.Replace(txtBarcode.Text.Trim(), @"\s+", " ");
            string newName = Regex.Replace(txtProductName.Text.Trim(), @"\s+", " ");
            string newCategory = Regex.Replace(cmbCategory.Text.Trim(), @"\s+", " ");
            string newDescription = Regex.Replace(txtDescription.Text.Trim(), @"\s+", " ");
            string newRetail = Regex.Replace(txtRetailPrice.Text.Trim(), @"\s+", " ");
            string newWholesale = Regex.Replace(txtWholeSalePrice.Text.Trim(), @"\s+", " ");
            string newReorderLevel = Regex.Replace(txtReorderLevel.Text.Trim(), @"\s+", " ");

            // Required fields validation
            if (string.IsNullOrWhiteSpace(newBarcode) ||
                string.IsNullOrWhiteSpace(newName) ||
                string.IsNullOrWhiteSpace(newCategory) ||
                string.IsNullOrWhiteSpace(newDescription) ||
                string.IsNullOrWhiteSpace(newRetail) ||
                string.IsNullOrWhiteSpace(newWholesale) ||
                string.IsNullOrWhiteSpace(newReorderLevel))
            {
                MessageBox.Show("Please fill out all required fields before updating.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Barcode must be exactly 13 digits
            if (!Regex.IsMatch(newBarcode, @"^\d{13}$"))
            {
                MessageBox.Show("Barcode must be exactly 13 digits (numbers only).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBarcode.Focus();
                return;
            }

            // Numeric validation
            if (!decimal.TryParse(newRetail, out decimal retail) || retail <= 0)
            {
                MessageBox.Show("Retail Price must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRetailPrice.Focus();
                return;
            }

            if (!decimal.TryParse(newWholesale, out decimal wholesale) || wholesale <= 0)
            {
                MessageBox.Show("Wholesale Price must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWholeSalePrice.Focus();
                return;
            }

            if (!int.TryParse(newReorderLevel, out int rLevel) || rLevel < 0)
            {
                MessageBox.Show("Reorder Level cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReorderLevel.Focus();
                return;
            }

            // Retail Price must be >= Wholesale Price
            if (retail <= wholesale)
            {
                MessageBox.Show($"Retail Price (₱{retail:0.00}) must be greater than Wholesale Price (₱{wholesale:0.00}).",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRetailPrice.Focus();
                return;
            }


            string oldBarcode = dgvProducts.CurrentRow.Cells["Barcode"].Value.ToString();

            try
            {
                ConnectionModule.openCon();

                // Check if category exists
                string checkCat = "SELECT COUNT(*) FROM Category WHERE CategoryName=@cat";
                using (MySqlCommand catCmd = new MySqlCommand(checkCat, ConnectionModule.con))
                {
                    catCmd.Parameters.AddWithValue("@cat", newCategory);
                    if (Convert.ToInt32(catCmd.ExecuteScalar()) == 0)
                    {
                        MessageBox.Show("Invalid category selected.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Check duplicate ProductName + Description (case-insensitive, excluding current barcode)
                string checkDuplicateNameDesc = @"
            SELECT COUNT(*) 
            FROM Product 
            WHERE LOWER(ProductName) = LOWER(@ProductName) 
              AND LOWER(Description) = LOWER(@Description)
              AND IsActive = 1
              AND Barcode != @OldBarcode";

                using (MySqlCommand dupCmd = new MySqlCommand(checkDuplicateNameDesc, ConnectionModule.con))
                {
                    dupCmd.Parameters.AddWithValue("@ProductName", newName.Trim());
                    dupCmd.Parameters.AddWithValue("@Description", newDescription.Trim());
                    dupCmd.Parameters.AddWithValue("@OldBarcode", oldBarcode.Trim());

                    int duplicateCount = Convert.ToInt32(dupCmd.ExecuteScalar());
                    if (duplicateCount > 0)
                    {
                        MessageBox.Show(
                            "A product with the same name and description already exists. Cannot update to duplicate values.",
                            "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Check if barcode exists and is active (excluding current product)
                string checkDupBarcode = "SELECT IsActive FROM Product WHERE Barcode=@Barcode AND Barcode != @OldBarcode";
                using (MySqlCommand checkCmd = new MySqlCommand(checkDupBarcode, ConnectionModule.con))
                {
                    checkCmd.Parameters.AddWithValue("@Barcode", newBarcode);
                    checkCmd.Parameters.AddWithValue("@OldBarcode", oldBarcode);
                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        bool isActive = Convert.ToBoolean(result);

                        if (isActive)
                        {
                            MessageBox.Show("Duplicate Barcode found! Barcode already exists and is active.",
                                            "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            // Reactivate inactive product
                            string reactivateQuery = @"UPDATE Product
                        SET ProductName=@ProductName,
                            CategoryID=(SELECT CategoryID FROM Category WHERE CategoryName=@CategoryName),
                            Description=@Description,
                            RetailPrice=@RetailPrice,
                            WholeSalePrice=@WholeSalePrice,
                            ReorderLevel=@ReorderLevel,
                            IsActive=1
                        WHERE Barcode=@Barcode";

                            using (MySqlCommand reactivateCmd = new MySqlCommand(reactivateQuery, ConnectionModule.con))
                            {
                                reactivateCmd.Parameters.AddWithValue("@Barcode", newBarcode);
                                reactivateCmd.Parameters.AddWithValue("@ProductName", newName);
                                reactivateCmd.Parameters.AddWithValue("@CategoryName", newCategory);
                                reactivateCmd.Parameters.AddWithValue("@Description", newDescription);
                                reactivateCmd.Parameters.AddWithValue("@RetailPrice", retail);
                                reactivateCmd.Parameters.AddWithValue("@WholeSalePrice", wholesale);
                                reactivateCmd.Parameters.AddWithValue("@ReorderLevel", rLevel);
                                reactivateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Inactive product reactivated successfully!",
                                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadProducts();
                            ClearFields();
                            return;
                        }
                    }
                }

                // Update product
                string query = @"UPDATE Product 
                         SET Barcode=@Barcode, ProductName=@ProductName,
                             CategoryID=(SELECT CategoryID FROM Category WHERE CategoryName=@CategoryName),
                             Description=@Description, RetailPrice=@RetailPrice, WholeSalePrice=@WholeSalePrice,
                             ReorderLevel=@ReorderLevel, IsActive=1
                         WHERE Barcode=@OldBarcode";

                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@Barcode", newBarcode);
                    cmd.Parameters.AddWithValue("@ProductName", newName);
                    cmd.Parameters.AddWithValue("@CategoryName", newCategory);
                    cmd.Parameters.AddWithValue("@Description", newDescription);
                    cmd.Parameters.AddWithValue("@RetailPrice", retail);
                    cmd.Parameters.AddWithValue("@WholeSalePrice", wholesale);
                    cmd.Parameters.AddWithValue("@ReorderLevel", rLevel);
                    cmd.Parameters.AddWithValue("@OldBarcode", oldBarcode);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 📝 Audit Trail (Update)
                string details = $"[Old Barcode: {oldBarcode}] → [New Barcode: {newBarcode}] | Product: {newName} | Category: {newCategory} | Retail: ₱{retail:0.00} | Wholesale: ₱{wholesale:0.00} | Reorder Level: {newReorderLevel}";
                ConnectionModule.InsertAuditTrail("Update", "Product", details);

                LoadProducts();
                ClearFields();
                dgvProducts.ClearSelection();
                dgvProducts.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || dgvProducts.CurrentRow.Cells["ProductID"].Value == DBNull.Value)
            {
                MessageBox.Show("Please select a valid product to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
            string productName = dgvProducts.CurrentRow.Cells["ProductName"].Value.ToString();
            string barcode = dgvProducts.CurrentRow.Cells["Barcode"].Value.ToString();

            try
            {
                ConnectionModule.openCon();

                // ✅ Step 1: Check kung ginagamit sa SALESDETAILS
                string checkSales = "SELECT COUNT(*) FROM salesdetails WHERE ProductID = @ProductID";
                using (MySqlCommand cmdSales = new MySqlCommand(checkSales, ConnectionModule.con))
                {
                    cmdSales.Parameters.AddWithValue("@ProductID", id);
                    int countSales = Convert.ToInt32(cmdSales.ExecuteScalar());
                    if (countSales > 0)
                    {
                        MessageBox.Show($"Product '{productName}' is linked to sales records.\n" +
                                        "Instead of deleting, it will be marked as inactive.",
                                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // ✅ Step 2: Confirm "soft delete"
                DialogResult dr = MessageBox.Show(
                    $"Are you sure you want to deactivate product '{productName}'?",
                    "Confirm Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No) return;

                // ✅ Step 3: Update isActive to 0 instead of deleting
                string deactivateQuery = "UPDATE product SET isActive = 0 WHERE ProductID = @ProductID";
                using (MySqlCommand cmdDeactivate = new MySqlCommand(deactivateQuery, ConnectionModule.con))
                {
                    cmdDeactivate.Parameters.AddWithValue("@ProductID", id);
                    cmdDeactivate.ExecuteNonQuery();
                }

                MessageBox.Show("Product has been deactivated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ✅ Optional: Audit trail
                ConnectionModule.InsertAuditTrail("Deactivate", "Product", $"Deactivated Product: '{productName}' (Barcode: {barcode})");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deactivating product: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
                LoadProducts(); // Make sure this only loads products where isActive = 1
                ClearFields();
                dgvProducts.ClearSelection();
                dgvProducts.CurrentCell = null;
            }
        }



        private void ClearFields()
        {
            txtBarcode.Clear();
            txtProductName.Clear();
            cmbCategory.SelectedIndex = -1;
            cmbCategory.SelectedItem = null;
            cmbCategory.Text = string.Empty;

            txtDescription.Clear();
            txtRetailPrice.Clear();
            txtWholeSalePrice.Clear();
            txtReorderLevel.Clear();
        }



        int lastSelectedProductRowIndex = -1; // 👈 Declare this at class level (top of form)

        private void dgvProducts_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
          
        }


        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchProduct.Text.Trim();

            try
            {
                ConnectionModule.openCon();
                string query = @"
            SELECT  
                p.ProductID,          
                p.Barcode, 
                p.ProductName, 
                p.CategoryID,
                c.CategoryName, 
                p.Description, 
                p.RetailPrice, 
                p.WholeSalePrice, 
                p.ReorderLevel
            FROM Product p
            INNER JOIN Category c ON p.CategoryID = c.CategoryID
            WHERE p.isActive = 1
              AND (p.ProductName LIKE @search 
                   OR p.Barcode LIKE @search
                   OR c.CategoryName LIKE @search
                   OR p.Description LIKE @search)";

                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Minimum rows logic for consistent height
                int minRows = 5;
                while (dt.Rows.Count < minRows)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                dgvProducts.DataSource = dt;

                // Set headers
                dgvProducts.Columns["CategoryName"].HeaderText = "Category";
                dgvProducts.Columns["WholeSalePrice"].HeaderText = "Wholesale Price";
                dgvProducts.Columns["RetailPrice"].HeaderText = "Retail Price";
                dgvProducts.Columns["ReorderLevel"].HeaderText = "Reorder Level";

                // Hide IDs
                if (dgvProducts.Columns.Contains("ProductID"))
                    dgvProducts.Columns["ProductID"].Visible = false;
                if (dgvProducts.Columns.Contains("CategoryID"))
                    dgvProducts.Columns["CategoryID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching products: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // 👇 If same row clicked again → UNSELECT
                    if (e.RowIndex == lastSelectedProductRowIndex)
                    {
                        dgvProducts.ClearSelection();
                        ClearFields();
                        lastSelectedProductRowIndex = -1;
                        return;
                    }

                    // ✅ Otherwise, select and show product data
                    DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                    txtBarcode.Text = row.Cells["Barcode"].Value?.ToString();
                    txtProductName.Text = row.Cells["ProductName"].Value?.ToString();
                    txtDescription.Text = row.Cells["Description"].Value?.ToString();
                    txtRetailPrice.Text = row.Cells["RetailPrice"].Value?.ToString();
                    txtWholeSalePrice.Text = row.Cells["WholeSalePrice"].Value?.ToString();
                    txtReorderLevel.Text = row.Cells["ReorderLevel"].Value?.ToString();

                    // ✅ Set category by ID (ensure correct match)
                    if (row.Cells["CategoryID"].Value != DBNull.Value)
                    {
                        cmbCategory.SelectedValue = row.Cells["CategoryID"].Value;
                    }
                    else
                    {
                        cmbCategory.SelectedIndex = -1;
                    }

                    lastSelectedProductRowIndex = e.RowIndex;
                }
                else
                {
                    // 👇 If clicked outside rows (blank area)
                    dgvProducts.ClearSelection();
                    ClearFields();
                    lastSelectedProductRowIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateBarcode_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            string productCode = "";

            for (int i = 0; i < 13; i++)
            {
                productCode += rnd.Next(0, 10).ToString(); // bawat digit ay 0-9
            }

            txtBarcode.Text = productCode;

            // pictureBoxBarcode.Image = null; // optional, no preview for font-based barcode
        }


        // 🔹 PrintPage event
        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            string code = txtBarcode.Text.Trim();

            if (!string.IsNullOrEmpty(code))
            {
                int paperWidth = 220; // 58mm
                float y = 10f;

                // ✅ Normal barcode image
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Width = 200,
                        Height = 80,     // standard barcode size
                        Margin = 0,
                        PureBarcode = true
                    }
                };

                var pixelData = writer.Write(code);

                using (Bitmap bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppArgb))
                {
                    BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                            ImageLockMode.WriteOnly, bitmap.PixelFormat);

                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    bitmap.UnlockBits(bitmapData);

                    float imgX = (paperWidth - bitmap.Width) / 2f;
                    e.Graphics.DrawImage(bitmap, imgX, y);

                    y += bitmap.Height + 5;

                    // ✅ Draw barcode number
                    using (Font textFont = new Font("Arial", 10, FontStyle.Regular))
                    {
                        SizeF textSize = e.Graphics.MeasureString(code, textFont);
                        float textX = (paperWidth - textSize.Width) / 2f;
                        e.Graphics.DrawString(code, textFont, Brushes.Black, textX, y);

                        y += textSize.Height + 5;
                    }

                    // ✅ Trick to force longer paper (adjust y + XXXX)
                    using (Pen invisiblePen = new Pen(Color.White))
                    {
                        e.Graphics.DrawRectangle(invisiblePen, 0, y + 400, 1, 1); // 🟢 Increase this value if needed
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string code = txtBarcode.Text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Please generate a barcode first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += Pd_PrintPage;

            // ✅ Increase height to allow longer receipt
            pd.DefaultPageSettings.PaperSize = new PaperSize("58mm", 220, 1500); // ← Try 1500 or even 2000
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing barcode: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtRetailPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtWholeSalePrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtReorderLevel_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
