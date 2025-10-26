using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Cmp;
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
    public partial class UC_Delivery : UserControl
    {
        public string DeliveryReceiptValue { get; set; }
        public string CompanyNameValue { get; set; }
        public DateTime DeliveryDateValue { get; set; }
        public string ReceivedBy { get; set; }

        MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory");
        public UC_Delivery(string deliveryReceipt, string companyName, DateTime deliveryDate, string ReceivedBy)
        {
            InitializeComponent();
            // Assign values sa controls
            txtDeliveryReceipt.Text = deliveryReceipt;
            txtCompanyName.Text = companyName;
            dtpDeliveryDate.Value = deliveryDate;
            txtReceivedBy.Text = ReceivedBy;
        }

        private void AutoFillDescription()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
                return;

            try
            {
                con.Open();
                string query = "SELECT Description FROM product WHERE ProductName = @ProductName LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    txtDescription.Text = result.ToString();
                }
                else
                {
                    txtDescription.Text = ""; // walang match
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching description: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public UC_Delivery()
        {
            InitializeComponent();
        }

        private void UC_Delivery_Load(object sender, EventArgs e)
        {
            dtpDeliveryDate.Enabled = false;
         
            dtpExpirationDate.Enabled = false; // ❌ Disable by default
            dtpExpirationDate.MinDate = DateTimePicker.MinimumDateTime;
            dtpExpirationDate.MaxDate = DateTimePicker.MaximumDateTime;

            LoadDeliveries();
            StyleDataGridView(dgvDeliveries);

            dgvDeliveries.ClearSelection();
            dgvDeliveries.CurrentCell = null;


            // CompanyName → bawal number, letters + space + . , - allowed
            //   txt.KeyPress += BlockInvalidCharacters_KeyPress;
            // txtCompanyName.KeyPress += BlockNumbers_KeyPress;

            // SupplierName → same rules as CompanyName



            // ContactNumber → digits only
            txtQtyOrderd.KeyPress += DigitsOnly_KeyPress;
            txtTotalCost.KeyPress += DigitsOnly_KeyPress;
            txtQtyDelivered.KeyPress += DigitsOnly_KeyPress;
            txtCostPerItem.KeyPress += DigitsOnly_KeyPress;
            txtDeliveryReceipt.KeyPress += DigitsOnly_KeyPress;


            // Address → letters + numbers + space + . , - allowed (pero bawal paste pa rin)
            //  txtAddress.KeyPress += BlockAddressCharacters_KeyPress;

            // Bawal mag-right click copy/paste
            txtCompanyName.ContextMenu = new ContextMenu();
            txtDescription.ContextMenu = new ContextMenu();
            txtProductName.ContextMenu = new ContextMenu();
            txtDeliveryReceipt.ContextMenu = new ContextMenu();
            txtCostPerItem.ContextMenu = new ContextMenu();
           
            txtQtyDelivered.ContextMenu = new ContextMenu();
            txtQtyOrderd.ContextMenu = new ContextMenu();
            txtTotalCost.ContextMenu = new ContextMenu();
            txtReceivedBy.ContextMenu = new ContextMenu();
            txtRemarks1.ContextMenu = new ContextMenu();

            // Bawal Ctrl+V / Ctrl+C / Ctrl+X / Shift+Insert / Ctrl+Insert
            txtCompanyName.KeyDown += BlockCopyPaste_KeyDown;
            txtDescription.KeyDown += BlockCopyPaste_KeyDown;
            txtProductName.KeyDown += BlockCopyPaste_KeyDown;
            txtDeliveryReceipt.KeyDown += BlockCopyPaste_KeyDown;
            txtCostPerItem.KeyDown += BlockCopyPaste_KeyDown;
            txtQtyDelivered.KeyDown += BlockCopyPaste_KeyDown;
            txtQtyOrderd.KeyDown += BlockCopyPaste_KeyDown;
            txtTotalCost.KeyDown += BlockCopyPaste_KeyDown;
            txtReceivedBy.KeyDown += BlockCopyPaste_KeyDown;
            txtRemarks1.KeyDown += BlockCopyPaste_KeyDown;
         


            txtProductName.KeyPress += BlockMultipleSpaces_KeyPress;
            txtDescription.KeyPress += BlockMultipleSpaces_KeyPress;
            txtCompanyName.KeyPress += BlockMultipleSpaces_KeyPress;
            txtDeliveryReceipt.KeyPress += BlockMultipleSpaces_KeyPress;
            txtCostPerItem.KeyPress += BlockMultipleSpaces_KeyPress;
            txtQtyDelivered.KeyPress += BlockMultipleSpaces_KeyPress;
            txtQtyOrderd.KeyPress += BlockMultipleSpaces_KeyPress;
            txtTotalCost.KeyPress += BlockMultipleSpaces_KeyPress;
            txtReceivedBy.KeyPress += BlockMultipleSpaces_KeyPress;
            txtRemarks1.KeyPress += BlockMultipleSpaces_KeyPress;

            //// Auto-normalize spaces kapag iniwan ang textbox
            txtProductName.TextChanged += NormalizeSpaces_OnChange;
            txtDescription.TextChanged += NormalizeSpaces_OnChange;
            txtCompanyName.TextChanged += NormalizeSpaces_OnChange;
            txtDeliveryReceipt.TextChanged += NormalizeSpaces_OnChange;
            txtCostPerItem.TextChanged += NormalizeSpaces_OnChange;
            txtQtyOrderd.TextChanged += NormalizeSpaces_OnChange;
            txtQtyDelivered.TextChanged += NormalizeSpaces_OnChange;
            txtTotalCost.TextChanged += NormalizeSpaces_OnChange;
            txtReceivedBy.TextChanged += NormalizeSpaces_OnChange;
            txtRemarks1.TextChanged += NormalizeSpaces_OnChange;

            // Disable shortcuts
            txtProductName.ShortcutsEnabled = false;
            txtDescription.ShortcutsEnabled = false;
            txtCompanyName.ShortcutsEnabled = false;
            txtDeliveryReceipt.ShortcutsEnabled = false;
            txtCostPerItem.ShortcutsEnabled = false;
            txtQtyDelivered.ShortcutsEnabled = false;
            txtTotalCost.ShortcutsEnabled = false;
            txtReceivedBy.ShortcutsEnabled = false;
            txtRemarks1.ShortcutsEnabled = false;
            txtQtyOrderd.ShortcutsEnabled = false;

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

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false; // optional: para isang row lang ang ma-select

            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = dgv.RowsDefaultCellStyle.BackColor;
            dgv.RowHeadersDefaultCellStyle.SelectionForeColor = dgv.RowsDefaultCellStyle.ForeColor;

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

        private void LoadDeliveries(string search = "")
        {

        }
       

        private bool IsProductExisting(string productName)
        {
            bool exists = false;
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;d"))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM product WHERE ProductName = @ProductName";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        exists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            return exists;
        }

        // Clear all input fields after adding
        private void ClearAllFields()
        {
            txtProductName.Text = "";
            txtDescription.Text = "";
            txtQtyDelivered.Text = "";
            txtQtyOrderd.Text = "";
            txtCostPerItem.Text = "";
            txtRemarks1.Text = "";
            cmbDeliveryStatus.Text = "";
            rdoWithExpiration.Checked = false;
            rdoWithoutExpiration.Checked = false;

            // ✅ Use a safe default value within valid range
            if (dtpExpirationDate.MinDate <= DateTime.Now && DateTime.Now <= dtpExpirationDate.MaxDate)
                dtpExpirationDate.Value = DateTime.Now;
            else
                dtpExpirationDate.Value = dtpExpirationDate.MinDate;
        }

        private int GetSupplierIDByCompanyName(string companyName)
        {
            try
            {
                string query = "SELECT SupplierID FROM supplier WHERE SupplierName = @SupplierName LIMIT 1";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SupplierName", companyName);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    con.Close();

                    if (result != null)
                        return Convert.ToInt32(result);
                    else
                        return -1; // hindi nahanap
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching supplier ID: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
                return -1;
            }
        }


        private int GetProductIDByProductName(string productName)
        {
            int productID = -1;
            string query = "SELECT ProductID FROM product WHERE ProductName = @ProductName LIMIT 1";

            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ProductName", productName);
                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                if (result != null)
                {
                    productID = Convert.ToInt32(result);
                }
            }
            return productID;
        }






        private void CalculateTotalCost()
        {
            if (decimal.TryParse(txtCostPerItem.Text, out decimal cost) &&
                int.TryParse(txtQtyDelivered.Text, out int qty))
            {
                decimal total = cost * qty;
                txtTotalCost.Text = total.ToString("N2"); // format 2 decimal places
            }
            else
            {
                txtTotalCost.Text = "0.00"; // kung invalid input
            }
        }
        private void txtCostPerItem_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtQtyDelivered_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCost();
        }
        


        private void cmbDeliveryStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ordered = 0;
            int delivered = 0;

            int.TryParse(txtQtyOrderd.Text, out ordered);
            int.TryParse(txtQtyDelivered.Text, out delivered);

            if (cmbDeliveryStatus.SelectedItem.ToString() == "Received")
            {
                if (delivered != ordered)
                {
                    MessageBox.Show("For 'Received' status, Quantity Delivered must equal Quantity Ordered.",
                                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQtyDelivered.Text = ordered.ToString();
                }
            }
            else if (cmbDeliveryStatus.SelectedItem.ToString() == "Cancel")
            {
                if (delivered != 0)
                {
                    MessageBox.Show("For 'Cancel' status, Quantity Delivered must be 0.",
                                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQtyDelivered.Text = "0";
                }
            }
        }

        // Flag para malaman kung may ongoing delivery transaction
        private bool isDeliveryInProgress = false;

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            // ✅ Check muna kung may ongoing transaction
            if (isDeliveryInProgress)
            {
                MessageBox.Show(
                    "You already have an ongoing delivery transaction. Please complete or save it first before starting a new one.",
                    "Transaction In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // ✅ Set flag to true kasi mag–oopen na tayo ng new Delivery
            isDeliveryInProgress = true;

            Delivery popup = new Delivery();

            popup.DeliverySaved += (s, args) =>
            {
                // Ilagay sa mga controls ng UC_Delivery
                txtDeliveryReceipt.Text = popup.DeliveryReceipt;
                txtCompanyName.Text = popup.CompanyName;
                dtpDeliveryDate.Value = popup.DateDelivered;
                txtReceivedBy.Text = popup.ReceivedBy;
            };

            // ✅ Handle kung na–close o kinansel ang form
            popup.FormClosed += (s, args) =>
            {
                // Kung wala pa ring laman ang txtDeliveryReceipt ibig sabihin hindi nag-save ng maayos
                if (string.IsNullOrWhiteSpace(txtDeliveryReceipt.Text))
                {
                    isDeliveryInProgress = false; // reset
                }
            };

            popup.ShowDialog();
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // ---------------- Validation ----------------
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                MessageBox.Show("Please select or enter a supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtReceivedBy.Text))
            {
                MessageBox.Show("Please enter who received the delivery.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtReceivedBy.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDeliveryReceipt.Text))
            {
                MessageBox.Show("Please enter a delivery receipt number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDeliveryReceipt.Focus();
                return;
            }

            if (dgvDeliveries.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one product before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int supplierID = GetSupplierIDByCompanyName(txtCompanyName.Text.Trim());
            if (supplierID == -1)
            {
                MessageBox.Show("Invalid supplier. Please select a valid one from the list.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // ---------------- Save Delivery ----------------
            try
            {
                con.Open();
                using (MySqlTransaction transaction = con.BeginTransaction())
                {
                    // 1️⃣ Get SupplierID from SupplierName
                    int supplierId = 0;
                    string getSupplierIDQuery = "SELECT SupplierID FROM supplier WHERE SupplierName=@Name LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(getSupplierIDQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtCompanyName.Text.Trim());
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Supplier not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            transaction.Rollback();
                            return;
                        }
                        supplierId = Convert.ToInt32(result);
                    }

                    string deliveryReceipt = txtDeliveryReceipt.Text.Trim();

                    // 2️⃣ Check duplicate delivery receipt for this supplier
                    string checkQuery = @"SELECT COUNT(*) FROM delivery 
                              WHERE TRIM(LOWER(DeliveryReceipt)) = TRIM(LOWER(@Receipt)) 
                                AND SupplierID=@SupplierID";

                    using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, con, transaction))
                    {
                        cmdCheck.Parameters.AddWithValue("@Receipt", deliveryReceipt);
                        cmdCheck.Parameters.AddWithValue("@SupplierID", supplierId);
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar() ?? 0);
                        if (count > 0)
                        {
                            MessageBox.Show("Delivery Receipt number already exists for this Supplier. Please enter a unique one.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transaction.Rollback();
                            txtDeliveryReceipt.Focus();
                            return;
                        }
                    }

                    // 3️⃣ Insert delivery
                    string insertQuery = @"INSERT INTO delivery 
                               (SupplierID, DeliveryDate, DeliveryReceipt, Remarks, DeliveryStatus, ReceivedBy)
                               VALUES (@SupplierID, @DeliveryDate, @DeliveryReceipt, @Remarks, @DeliveryStatus, @ReceivedBy)";

                    int deliveryID;
                    using (MySqlCommand cmdInsert = new MySqlCommand(insertQuery, con, transaction))
                    {
                        cmdInsert.Parameters.AddWithValue("@SupplierID", supplierId);
                        cmdInsert.Parameters.AddWithValue("@DeliveryDate", DateTime.Now);
                        cmdInsert.Parameters.AddWithValue("@DeliveryReceipt", deliveryReceipt);
                        cmdInsert.Parameters.AddWithValue("@Remarks", txtRemarks1.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@DeliveryStatus", "Pending");
                        cmdInsert.Parameters.AddWithValue("@ReceivedBy", txtReceivedBy.Text.Trim());
                        cmdInsert.ExecuteNonQuery();
                        deliveryID = (int)cmdInsert.LastInsertedId;
                    }


                    bool allReceived = true;
                    bool hasPartial = false;

                    foreach (DataGridViewRow row in dgvDeliveries.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int productID = Convert.ToInt32(row.Cells["ProductID"].Value);
                        int qtyOrdered = Convert.ToInt32(row.Cells["QtyOrdered"].Value);
                        int qtyDelivered = Convert.ToInt32(row.Cells["QtyDelivered"].Value);
                        decimal costPerItem = Convert.ToDecimal(row.Cells["CostPerItem"].Value);
                        decimal totalCost = Convert.ToDecimal(row.Cells["TotalCost"].Value);
                        string description = row.Cells["Description"].Value?.ToString() ?? "";
                        string remarks = row.Cells["Remarks"].Value?.ToString() ?? "";
                        string expirationValue = row.Cells["ExpirationDate"].Value?.ToString()?.Trim() ?? "";
                        object expirationForDB;

                        // Expiration parsing
                        if (string.Equals(expirationValue, "No Expiration", StringComparison.OrdinalIgnoreCase))
                        {
                            expirationForDB = "No Expiration";
                        }
                        else if (DateTime.TryParse(expirationValue, out DateTime parsedDate))
                        {
                            if (parsedDate.Date <= DateTime.Now.Date)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"Product \"{row.Cells["ProductName"].Value}\" has an invalid expiration date ({parsedDate:yyyy-MM-dd}). Cannot save expired or today-expiring products.", "Invalid Expiration Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            expirationForDB = parsedDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            expirationForDB = "No Expiration";
                        }

                        // Determine item status
                        string itemStatus = "Received";
                        if (qtyDelivered == 0 || cmbDeliveryStatus.Text.Trim().Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                        {
                            itemStatus = "Cancelled";
                            allReceived = false;
                        }
                        else if (qtyDelivered < qtyOrdered)
                        {
                            itemStatus = "Partial";
                            hasPartial = true;
                            allReceived = false;
                        }

                        // Insert delivery details
                        string queryDetails = @"INSERT INTO delivery_details 
                    (idDelivery, ProductID, QtyOrdered, QtyDelivered, CostPerItem, TotalCost, ExpirationDate, Description, Remarks, Status) 
                    VALUES (@idDelivery, @ProductID, @QtyOrdered, @QtyDelivered, @CostPerItem, @TotalCost, @ExpirationDate, @Description, @Remarks, @Status)";

                        int deliveryDetailID;
                        using (MySqlCommand cmdDetails = new MySqlCommand(queryDetails, con, transaction))
                        {
                            cmdDetails.Parameters.AddWithValue("@idDelivery", deliveryID);
                            cmdDetails.Parameters.AddWithValue("@ProductID", productID);
                            cmdDetails.Parameters.AddWithValue("@QtyOrdered", qtyOrdered);
                            cmdDetails.Parameters.AddWithValue("@QtyDelivered", qtyDelivered);
                            cmdDetails.Parameters.AddWithValue("@CostPerItem", costPerItem);
                            cmdDetails.Parameters.AddWithValue("@TotalCost", totalCost);
                            cmdDetails.Parameters.AddWithValue("@Description", description?.Trim() ?? "");
                            cmdDetails.Parameters.AddWithValue("@Remarks", remarks);
                            cmdDetails.Parameters.AddWithValue("@Status", itemStatus);
                            cmdDetails.Parameters.AddWithValue("@ExpirationDate", expirationForDB);
                            cmdDetails.ExecuteNonQuery();

                            deliveryDetailID = (int)cmdDetails.LastInsertedId;
                        }

                        // Update inventory & nearly expired
                        if (itemStatus != "Cancelled" && qtyDelivered > 0)
                        {
                            UpdateInventory(con, transaction, productID, qtyDelivered, deliveryDetailID, expirationForDB, row.Cells["Description"].Value.ToString());

                        }
                    }

                    // ---------------- Finalize Delivery Status ----------------
                    string overallStatus = "Pending";
                    if (cmbDeliveryStatus.Text.Trim().Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                        overallStatus = "Cancelled";
                    else if (allReceived)
                        overallStatus = "Received";
                    else if (hasPartial)
                        overallStatus = "Partial";

                    string updateDeliveryStatus = "UPDATE delivery SET DeliveryStatus=@status WHERE idDelivery=@idDelivery";
                    using (MySqlCommand cmdUpdateDelivery = new MySqlCommand(updateDeliveryStatus, con, transaction))
                    {
                        cmdUpdateDelivery.Parameters.AddWithValue("@status", overallStatus);
                        cmdUpdateDelivery.Parameters.AddWithValue("@idDelivery", deliveryID);
                        cmdUpdateDelivery.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    // 📝 Audit Trail (Delivery)
                    int productCount = dgvDeliveries.Rows.Count;
                    string details = $"[Delivery Receipt: {txtDeliveryReceipt.Text}] | Supplier: {txtCompanyName.Text} | Status: {overallStatus} | Received By: {txtReceivedBy.Text} | Products: {productCount} item{(productCount > 1 ? "s" : "")}";
                    ConnectionModule.InsertAuditTrail("Insert", "Delivery", details);


                    MessageBox.Show("Delivery saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear form
                    dgvDeliveries.Rows.Clear();
                    txtCompanyName.Clear();
                    txtDeliveryReceipt.Clear();
                    txtRemarks1.Clear();
                    txtReceivedBy.Clear();

                    // ✅ Reset flag para ma–allow ulit ang pag-open ng delivery form
                    isDeliveryInProgress = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving delivery: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void UpdateInventory(MySqlConnection con, MySqlTransaction transaction, int productID, int qtyDelivered, int deliveryDetailID, object expirationForDB, string description)
        {
        
            int currentInventoryID = 0;
            int currentQty = 0;
            int reorderLevel = 0;

            // 🛠 Step 1: Ensure we have correct description (in case textbox was cleared)
            if (string.IsNullOrWhiteSpace(description))
            {
                using (MySqlCommand cmdGetDesc = new MySqlCommand("SELECT Description FROM product WHERE ProductID=@ProductID LIMIT 1", con, transaction))
                {
                    cmdGetDesc.Parameters.AddWithValue("@ProductID", productID);
                    object descObj = cmdGetDesc.ExecuteScalar();
                    description = descObj?.ToString() ?? "";
                }
            }

            // 🧾 Step 2: Get ReorderLevel from Product table (since inventory may not have one yet)
            using (MySqlCommand cmdGetReorder = new MySqlCommand("SELECT ReorderLevel FROM product WHERE ProductID=@ProductID LIMIT 1", con, transaction))
            {
                cmdGetReorder.Parameters.AddWithValue("@ProductID", productID);
                object rlObj = cmdGetReorder.ExecuteScalar();
                reorderLevel = rlObj != null ? Convert.ToInt32(rlObj) : 0;
            }

            // 🧾 Step 3: Check if inventory record already exists (ProductID + Description)
            string checkInventory = @"SELECT idInventory, QuantityInStock 
                              FROM inventory 
                              WHERE ProductID=@ProductID AND Description=@Description";
            using (MySqlCommand cmdCheck = new MySqlCommand(checkInventory, con, transaction))
            {
                cmdCheck.Parameters.AddWithValue("@ProductID", productID);
                cmdCheck.Parameters.AddWithValue("@Description", description?.Trim() ?? "");
                using (var reader = cmdCheck.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentInventoryID = Convert.ToInt32(reader["idInventory"]);
                        currentQty = Convert.ToInt32(reader["QuantityInStock"]);
                    }
                }
            }

            // 🧩 Step 4: Update or Insert Inventory
            if (currentInventoryID > 0)
            {
                // ✅ Same ProductID + Description → Update quantity only
                string updateInventory = @"UPDATE inventory 
                                   SET QuantityInStock = @newQty, 
                                       LastUpdated = NOW()
                                   WHERE idInventory=@idInventory";
                using (MySqlCommand cmdUpdate = new MySqlCommand(updateInventory, con, transaction))
                {
                    cmdUpdate.Parameters.AddWithValue("@newQty", currentQty + qtyDelivered);
                    cmdUpdate.Parameters.AddWithValue("@idInventory", currentInventoryID);
                    cmdUpdate.ExecuteNonQuery();
                }
            }
            else
            {
                // ✅ Different Description or new product → Insert new row (with ReorderLevel)
                string insertInventory = @"INSERT INTO inventory 
                                   (idDetail, ProductID, Description, QuantityInStock, Remarks, LastUpdated, ReorderLevel) 
                                   VALUES (@idDetail, @ProductID, @Description, @qtyDelivered, 'Initial stock from delivery', NOW(), @ReorderLevel)";
                using (MySqlCommand cmdInsert = new MySqlCommand(insertInventory, con, transaction))
                {
                    cmdInsert.Parameters.AddWithValue("@idDetail", deliveryDetailID);
                    cmdInsert.Parameters.AddWithValue("@ProductID", productID);
                    cmdInsert.Parameters.AddWithValue("@Description", description?.Trim() ?? "");
                    cmdInsert.Parameters.AddWithValue("@qtyDelivered", qtyDelivered);
                    cmdInsert.Parameters.AddWithValue("@ReorderLevel", reorderLevel);
                    cmdInsert.ExecuteNonQuery();

                    currentInventoryID = (int)cmdInsert.LastInsertedId;
                }
            }
        

        // 📦 Step 4: Log movement
        string queryMovement = @"INSERT INTO inventory_movements 
    (idDetail, ProductID, MovementType, Quantity, Source, ReferenceID, Remarks, Description) 
    VALUES (@idDetail, @ProductID, 'IN', @qtyDelivered, 'Delivery', @referenceID, @Remarks, @Description)";
            using (MySqlCommand cmdMove = new MySqlCommand(queryMovement, con, transaction))
            {
                cmdMove.Parameters.AddWithValue("@idDetail", deliveryDetailID);
                cmdMove.Parameters.AddWithValue("@ProductID", productID);
                cmdMove.Parameters.AddWithValue("@qtyDelivered", qtyDelivered);
                cmdMove.Parameters.AddWithValue("@referenceID", deliveryDetailID);

                // ✅ Add missing parameters
                cmdMove.Parameters.AddWithValue("@Remarks", "Stock received from supplier");
                cmdMove.Parameters.AddWithValue("@Description", description?.Trim() ?? "");

                cmdMove.ExecuteNonQuery();
            }


            // ⏰ Step 5: Nearly Expired Products
            if (expirationForDB is string strExp && strExp != "No Expiration" && qtyDelivered > 0)
            {
                DateTime expDate = DateTime.Parse(strExp);
                double daysToExpire = (expDate - DateTime.Now.Date).TotalDays;

                if (daysToExpire <= 30)
                {
                    // 🧠 Fallback: if description is empty, get from inventory or product
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        using (MySqlCommand cmdGetDesc = new MySqlCommand(
                            "SELECT Description FROM inventory WHERE idInventory=@invID LIMIT 1", con, transaction))
                        {
                            cmdGetDesc.Parameters.AddWithValue("@invID", currentInventoryID);
                            object descObj = cmdGetDesc.ExecuteScalar();
                            description = descObj?.ToString() ?? "";
                        }
                    }

                    string checkNearly = @"
            SELECT idNearlyExpired 
            FROM nearly_expired_products 
            WHERE ProductName = (SELECT ProductName FROM product WHERE ProductID=@ProductID LIMIT 1)
              AND Description = @Description
              AND DATE(ExpirationDate) = @ExpirationDate";

                    object existing;
                    using (MySqlCommand cmdCheckNearly = new MySqlCommand(checkNearly, con, transaction))
                    {
                        cmdCheckNearly.Parameters.AddWithValue("@ProductID", productID);
                        cmdCheckNearly.Parameters.AddWithValue("@Description", description?.Trim() ?? "");
                        cmdCheckNearly.Parameters.AddWithValue("@ExpirationDate", expDate.Date);
                        existing = cmdCheckNearly.ExecuteScalar();
                    }

                    if (existing != null)
                    {
                        string updateNearly = @"
                UPDATE nearly_expired_products 
                SET Quantity = Quantity + @Quantity, 
                    DaysRemaining = @DaysRemaining 
                WHERE idNearlyExpired = @idNearlyExpired";
                        using (MySqlCommand cmdUpdateNearly = new MySqlCommand(updateNearly, con, transaction))
                        {
                            cmdUpdateNearly.Parameters.AddWithValue("@Quantity", qtyDelivered);
                            cmdUpdateNearly.Parameters.AddWithValue("@DaysRemaining", Math.Round(daysToExpire));
                            cmdUpdateNearly.Parameters.AddWithValue("@idNearlyExpired", existing);
                            cmdUpdateNearly.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertNearlyExpired = @"
                INSERT INTO nearly_expired_products 
                (idInventory, ProductName, Quantity, ExpirationDate, DaysRemaining, Description)
                VALUES (
                    @idInventory, 
                    (SELECT ProductName FROM product WHERE ProductID=@ProductID LIMIT 1), 
                    @Quantity, 
                    @ExpirationDate, 
                    @DaysRemaining, 
                    @Description)";
                        using (MySqlCommand cmdNearlyExpired = new MySqlCommand(insertNearlyExpired, con, transaction))
                        {
                            cmdNearlyExpired.Parameters.AddWithValue("@idInventory", currentInventoryID);
                            cmdNearlyExpired.Parameters.AddWithValue("@ProductID", productID);
                            cmdNearlyExpired.Parameters.AddWithValue("@Quantity", qtyDelivered);
                            cmdNearlyExpired.Parameters.AddWithValue("@ExpirationDate", expDate);
                            cmdNearlyExpired.Parameters.AddWithValue("@DaysRemaining", Math.Round(daysToExpire));
                            cmdNearlyExpired.Parameters.AddWithValue("@Description", description?.Trim() ?? "");
                            cmdNearlyExpired.ExecuteNonQuery();
                        }
                    }
                }
            }

        }


        // --------------------- Add Item to Delivery ---------------------
        // --------------------- Add Item to Delivery ---------------------
        private void AddItem_Click_1(object sender, EventArgs e)
        {
            // ---------------- Basic Validation ----------------
            if (!ValidateDeliveryItem(out int qtyDelivered, out int qtyOrdered, out decimal costPerItem))
                return;

            string deliveryReceipt = txtDeliveryReceipt.Text.Trim();
            string productName = txtProductName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string supplierName = txtCompanyName.Text.Trim();
            string remarks = txtRemarks1.Text.Trim();
            string expirationDate = rdoWithExpiration.Checked ? dtpExpirationDate.Value.ToString("yyyy-MM-dd") : "No Expiration";

            int supplierID = GetSupplierIDByCompanyName(supplierName);
            if (supplierID == -1)
            {
                MessageBox.Show("Invalid supplier. Please select a valid SupplierName.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int productID = GetProductIDByProductName(productName);
            if (productID == -1)
            {
                MessageBox.Show("Product not found in the database! Please select a valid product.", "Invalid Product", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return;
            }

            // ---------------- Price validation ----------------
            (decimal wholesalePrice, decimal retailPrice) = GetProductPrices(productID);
            if (retailPrice <= wholesalePrice || wholesalePrice <= costPerItem)
            {
                MessageBox.Show(
                    $"⚠ Price Error!\n\n" +
                    $"• Retail Price (₱{retailPrice:0.00}) must be greater than Wholesale Price (₱{wholesalePrice:0.00}).\n" +
                    $"• Wholesale Price (₱{wholesalePrice:0.00}) must be greater than Cost per Item (₱{costPerItem:0.00}).",
                    "Price Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ---------------- Quantity validation ----------------
            if (qtyDelivered > qtyOrdered)
            {
                MessageBox.Show("Delivered quantity cannot exceed Ordered quantity.", "Quantity Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ---------------- Cancelled status check ----------------
            if (cmbDeliveryStatus.Text.Trim().Equals("Cancelled", StringComparison.OrdinalIgnoreCase) && qtyDelivered > 0)
            {
                MessageBox.Show("Cannot cancel a delivery that has delivered quantity. Adjust QtyDelivered or change status.", "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ---------------- Auto-set status and remarks based on quantities ----------------
            string status;

            // determine status
            if (qtyDelivered == 0)
            {
                status = "Cancelled";
                if (string.IsNullOrWhiteSpace(remarks))
                    remarks = "Delivery cancelled";
            }
            else if (qtyDelivered == qtyOrdered)
            {
                status = "Received";

                // ✅ if no remarks typed, auto set to "Completed" (optional)
                if (string.IsNullOrWhiteSpace(remarks))
                    remarks = "Completed";

                // 👉 OR if you want it to stay empty, comment the above line:
                // remarks = null;
            }
            else // qtyDelivered < qtyOrdered
            {
                status = "Partial";
                if (string.IsNullOrWhiteSpace(remarks))
                    remarks = $"Partial delivery ({qtyDelivered}/{qtyOrdered})";
            }

            // update status in combo
            cmbDeliveryStatus.Text = status;

            // ---------------- Add / Merge row to DataGridView ----------------
            bool foundSameRow = false;

            foreach (DataGridViewRow existingRow in dgvDeliveries.Rows)
            {
                if (existingRow.IsNewRow) continue;

                // Check if all key fields match
                bool isSame =
                    existingRow.Cells["ProductID"].Value?.ToString() == productID.ToString() &&
                    existingRow.Cells["ProductName"].Value?.ToString() == productName &&
                    existingRow.Cells["Description"].Value?.ToString() == description &&
                    existingRow.Cells["CostPerItem"].Value?.ToString() == costPerItem.ToString("0.00") &&
                    existingRow.Cells["ExpirationDate"].Value?.ToString() == expirationDate &&
                    existingRow.Cells["Remarks"].Value?.ToString() == remarks;

                if (isSame)
                {
                    // ✅ If same item (exact match), sum up quantities and total cost
                    int oldQtyOrdered = Convert.ToInt32(existingRow.Cells["QtyOrdered"].Value);
                    int oldQtyDelivered = Convert.ToInt32(existingRow.Cells["QtyDelivered"].Value);
                    decimal oldTotalCost = Convert.ToDecimal(existingRow.Cells["TotalCost"].Value);

                    int newQtyOrdered = oldQtyOrdered + qtyOrdered;
                    int newQtyDelivered = oldQtyDelivered + qtyDelivered;
                    decimal newTotalCost = oldTotalCost + (qtyDelivered * costPerItem);

                    existingRow.Cells["QtyOrdered"].Value = newQtyOrdered;
                    existingRow.Cells["QtyDelivered"].Value = newQtyDelivered;
                    existingRow.Cells["TotalCost"].Value = newTotalCost.ToString("0.00");

                    foundSameRow = true;
                    break; // stop checking more rows
                }
            }

            // ➕ If no identical row found, add new one
            if (!foundSameRow)
            {
                int rowIndex = dgvDeliveries.Rows.Add();
                dgvDeliveries.Rows[rowIndex].Cells["DeliveryReceipt"].Value = deliveryReceipt;
                dgvDeliveries.Rows[rowIndex].Cells["ProductID"].Value = productID;
                dgvDeliveries.Rows[rowIndex].Cells["ProductName"].Value = productName;
                dgvDeliveries.Rows[rowIndex].Cells["Description"].Value = description;
                dgvDeliveries.Rows[rowIndex].Cells["Remarks"].Value = remarks;
                dgvDeliveries.Rows[rowIndex].Cells["QtyOrdered"].Value = qtyOrdered;
                dgvDeliveries.Rows[rowIndex].Cells["QtyDelivered"].Value = qtyDelivered;
                dgvDeliveries.Rows[rowIndex].Cells["CostPerItem"].Value = costPerItem.ToString("0.00");
                dgvDeliveries.Rows[rowIndex].Cells["TotalCost"].Value = (qtyDelivered * costPerItem).ToString("0.00");
                dgvDeliveries.Rows[rowIndex].Cells["DeliveryDate"].Value = dtpDeliveryDate.Value.ToString("yyyy-MM-dd");
                dgvDeliveries.Rows[rowIndex].Cells["ExpirationDate"].Value = expirationDate;
                dgvDeliveries.Rows[rowIndex].Cells["Status"].Value = status;
            }

            dgvDeliveries.Refresh();
            ClearAllFields();
            dgvDeliveries.ClearSelection();

        }

        // ---------------- Centralized Item Validation ----------------
        private bool ValidateDeliveryItem(out int qtyDelivered, out int qtyOrdered, out decimal costPerItem)
        {
            qtyDelivered = 0; qtyOrdered = 0; costPerItem = 0m;

            string[] requiredFields = {
        txtDeliveryReceipt.Text, txtQtyDelivered.Text, txtCostPerItem.Text,
        txtDescription.Text, txtProductName.Text, txtCompanyName.Text,
        txtReceivedBy.Text, cmbDeliveryStatus.Text, txtQtyOrderd.Text
    };

            if (requiredFields.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Please fill in all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtQtyDelivered.Text.Trim(), out qtyDelivered) || qtyDelivered < 0)
            {
                MessageBox.Show("Invalid delivered quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtQtyOrderd.Text.Trim(), out qtyOrdered) || qtyOrdered < 0)
            {
                MessageBox.Show("Invalid ordered quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!decimal.TryParse(txtCostPerItem.Text.Trim(), out costPerItem) || costPerItem < 0)
            {
                MessageBox.Show("Invalid cost per item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // ---------------- Expiration validation ----------------
            if (rdoWithExpiration.Checked && dtpExpirationDate.Value.Date <= DateTime.Now.Date)
            {
                MessageBox.Show("You cannot add products that are expired or expiring today. Please select a future date.",
                                "Invalid Expiration Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private void btnDetails_Click_1(object sender, EventArgs e)
        {
            // Gumawa ng instance ng UC_DeliveryDetails
            UC_DeliveryDetails detailsUC = new UC_DeliveryDetails();
            detailsUC.Dock = DockStyle.Fill; // sakupin buong panel

            // Kunin yung parent form
            Form parentForm = this.FindForm();

            if (parentForm != null)
            {
                // Hanapin yung panel sa form na gusto mong palitan
                Panel mainPanel = parentForm.Controls["mainPanel"] as Panel;

                if (mainPanel != null)
                {
                    mainPanel.Controls.Clear();
                    mainPanel.Controls.Add(detailsUC);
                }
            }
        }

        private (decimal WholesalePrice, decimal RetailPrice) GetProductPrices(int productID)
        {
            string query = "SELECT WholesalePrice, RetailPrice FROM product WHERE ProductID=@id";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", productID);
            con.Open();
            var reader = cmd.ExecuteReader();
            decimal wholesale = 0, retail = 0;
            if (reader.Read())
            {
                wholesale = Convert.ToDecimal(reader["WholesalePrice"]);
                retail = Convert.ToDecimal(reader["RetailPrice"]);
            }
            con.Close();
            return (wholesale, retail);
        }



        private void Edit_Click(object sender, EventArgs e)
        {
            if (dgvDeliveries.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvDeliveries.SelectedRows[0];

                // ---------------- Fill the TextBoxes and ComboBoxes ----------------
                txtDeliveryReceipt.Text = selectedRow.Cells["DeliveryReceipt"].Value?.ToString() ?? "";
                txtProductName.Text = selectedRow.Cells["ProductName"].Value?.ToString() ?? "";
                txtDescription.Text = selectedRow.Cells["Description"].Value?.ToString() ?? "";
                txtRemarks1.Text = selectedRow.Cells["Remarks"]?.Value?.ToString() ?? "";

                txtQtyOrderd.Text = selectedRow.Cells["QtyOrdered"].Value?.ToString() ?? "0";
                txtQtyDelivered.Text = selectedRow.Cells["QtyDelivered"].Value?.ToString() ?? "0";
                txtCostPerItem.Text = selectedRow.Cells["CostPerItem"].Value?.ToString() ?? "0.00";

                // ---------------- Set DeliveryDate ----------------
                if (DateTime.TryParse(selectedRow.Cells["DeliveryDate"].Value?.ToString(), out DateTime deliveryDate))
                    dtpDeliveryDate.Value = deliveryDate;
                else
                    dtpDeliveryDate.Value = DateTime.Now;

                // ---------------- Set ExpirationDate ----------------
                string expDate = selectedRow.Cells["ExpirationDate"]?.Value?.ToString() ?? "No Expiration";
                if (expDate.Equals("No Expiration", StringComparison.OrdinalIgnoreCase))
                {
                    rdoWithoutExpiration.Checked = true;
                }
                else if (DateTime.TryParse(expDate, out DateTime expirationDate))
                {
                    rdoWithExpiration.Checked = true;
                    dtpExpirationDate.Value = expirationDate;
                }
                else
                {
                    // fallback
                    rdoWithoutExpiration.Checked = true;
                }

                // ---------------- Auto-set status in the edit fields (optional) ----------------
                string status = selectedRow.Cells["Status"]?.Value?.ToString() ?? "";
                cmbDeliveryStatus.Text = status;

                // ---------------- Remove the row from DataGridView so user can re-add after editing ----------------
                dgvDeliveries.Rows.Remove(selectedRow);

                // ---------------- Optional: set focus to first editable field ----------------
                txtProductName.Focus();
            }
            else
            {
                MessageBox.Show("Please select a row to edit!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void Remove_Click(object sender, EventArgs e)
        {
            if (dgvDeliveries.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    dgvDeliveries.Rows.Remove(dgvDeliveries.SelectedRows[0]);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                txtDescription.Text = "";
            }
        }


        private void txtProductName_Leave(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(txtProductName.Text))
            //{
            //    // clear both kapag walang laman si ProductName
            //    txtProductName.Text = "";
            //    txtDescription.Text = "";
            //    return;
            //}

            ////try
            ////{
            ////    using (MySqlConnection con2 = new MySqlConnection(con.ConnectionString))
            ////    {
            ////        con2.Open();
            ////        string query = "SELECT ProductName, Description " +
            ////                       "FROM product " +
            ////                       "WHERE Barcode = @Input OR ProductName = @Input " +
            ////                       "LIMIT 1";

            ////        using (MySqlCommand cmd = new MySqlCommand(query, con2))
            ////        {
            ////            cmd.Parameters.AddWithValue("@Input", txtProductName.Text.Trim());
            ////            using (MySqlDataReader reader = cmd.ExecuteReader())
            ////            {
            ////                if (reader.Read())
            ////                {
            ////                    txtProductName.Text = reader["ProductName"].ToString();
            ////                    txtDescription.Text = reader["Description"].ToString();
            ////                }
            ////                else
            ////                {
            ////                    // kapag walang nahanap, clear pareho
            ////                    txtProductName.Text = "";
            ////                    txtDescription.Text = "";
            ////                }
            ////            }
            ////        }
            ////    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error fetching product info: " + ex.Message,
            //                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void rdoWithExpiration_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rdoWithExpiration.Checked)
            {
                dtpExpirationDate.Enabled = true;  // enable kapag may expiration
            }
        }

        private void rdoWithoutExpiration_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rdoWithoutExpiration.Checked)
            {
                dtpExpirationDate.Enabled = false;  // ❌ Disable if "Without Expiration" selected
                dtpExpirationDate.Value = DateTime.Now; // optional: reset value
            }
        }

        private void txtCostPerItem_TextChanged_1(object sender, EventArgs e)
        {
            CalculateTotalCost();
        }

        private void txtQtyDelivered_TextChanged_1(object sender, EventArgs e)
        {
            CalculateTotalCost();
        }

        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    using (MySqlConnection con2 = new MySqlConnection(con.ConnectionString))
                    {
                        con2.Open();
                        string query = "SELECT ProductName, Description FROM product WHERE ProductName = @ProductName OR Barcode = @Barcode LIMIT 1";
                        using (MySqlCommand cmd = new MySqlCommand(query, con2))
                        {
                            cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Barcode", txtProductName.Text.Trim());

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtProductName.Text = reader["ProductName"].ToString();
                                    txtDescription.Text = reader["Description"].ToString();
                                }
                                else
                                {
                                    txtDescription.Text = "";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching product info: " + ex.Message);
                }
            }
        }

        private void dgvDeliveries_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    }
