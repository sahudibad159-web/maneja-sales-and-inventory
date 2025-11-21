using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sales_Inventory
{
    public partial class TransactionHistory : Form
    {
        private string currentUser;
        public TransactionHistory(string fullName)
        {
            InitializeComponent();
            currentUser = fullName; // store locally
            StyleDataGridView(dgvTransactionItems);
            StyleDataGridView(dgvTransactions);

            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect = false;
            dgvTransactionItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactionItems.MultiSelect = false;

            // attach events BEFORE loading data
            dgvTransactions.DataBindingComplete += dgvTransactions_DataBindingComplete;
            dgvTransactions.SelectionChanged += dgvTransactions_SelectionChanged;
            dgvTransactionItems.CellContentClick += dgvTransactionItems_CellContentClick;
            dgvTransactionItems.DataBindingComplete += dgvTransactionItems_DataBindingComplete_1;

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
       

        private void TransactionHistory_Load(object sender, EventArgs e)
        {
            // 🟨 Prevent automatic selection of first row
          
            LoadTransactions();
            dgvTransactionItems.ClearSelection();
            dgvTransactionItems.CurrentCell = null;
            dgvTransactions.ClearSelection();
            dgvTransactions.CurrentCell = null;
        }
        bool isLoadingTransactions = false;
        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            LoadTransactions(dtDate.Value);
             
        }
        private void LoadTransactions(DateTime? selectedDate = null)
        {
            isLoadingTransactions = true; // 🚫 Block SelectionChanged events

            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();

                string query = "SELECT SaleID, TransactionDate, NetAmount, PaymentMethod, CashierName, IsVoided FROM sales";
                if (selectedDate.HasValue)
                    query += " WHERE DATE(TransactionDate) = @date";
                query += " ORDER BY TransactionDate DESC";

                using (var cmd = new MySqlCommand(query, con))
                {
                    if (selectedDate.HasValue)
                        cmd.Parameters.AddWithValue("@date", selectedDate.Value.Date);

                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvTransactions.DataSource = dt;

                        // 🟨 Hide SaleID column if it exists
                        if (dgvTransactions.Columns.Contains("SaleID"))
                            dgvTransactions.Columns["SaleID"].Visible = false;

                       
                    }
                }
            }

            // 🧹 Done loading
            isLoadingTransactions = false; // ✅ Allow SelectionChanged again
        }






        private void dgvTransactions_SelectionChanged(object sender, EventArgs e)
        {
            if (isLoadingTransactions) return; // 🚫 skip during load
            if (dgvTransactions.SelectedRows.Count == 0) return;
            if (dgvTransactions.CurrentRow == null) return;
           // if (!dgvTransactions.Focused) return; // ✅ Only trigger on manual click

            DataGridViewRow selectedRow = dgvTransactions.SelectedRows[0];
            if (selectedRow.Cells["SaleID"].Value == null) return;

            long saleId = Convert.ToInt64(selectedRow.Cells["SaleID"].Value);

            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();

                // 🟩 Load all sale details for the selected transaction
                string query = @"
            SELECT sd.SaleDetailID, sd.ProductID, p.ProductName, sd.Quantity, 
                   sd.UnitPrice, sd.SubTotal, sd.IsVoided, sd.VoidReason, sd.SaleID
            FROM salesdetails sd
            INNER JOIN product p ON sd.ProductID = p.ProductID
            WHERE sd.SaleID = @saleId";

                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@saleId", saleId);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvTransactionItems.DataSource = dt;

                        // 🟩 Hide unnecessary columns
                        if (dgvTransactionItems.Columns.Contains("SaleDetailID"))
                            dgvTransactionItems.Columns["SaleDetailID"].Visible = false;

                        if (dgvTransactionItems.Columns.Contains("SaleID"))
                            dgvTransactionItems.Columns["SaleID"].Visible = false;

                        if (dgvTransactionItems.Columns.Contains("ProductID"))
                            dgvTransactionItems.Columns["ProductID"].Visible = false;

                        // 🟨 Prevent automatic row selection
                        dgvTransactionItems.ClearSelection();
                        dgvTransactionItems.CurrentCell = null;
                    }
                }

                // 🔹 Check transaction void state
                bool isTransactionVoided = false;
                string checkSale = "SELECT IsVoided FROM sales WHERE SaleID=@saleId";
                using (var cmd = new MySqlCommand(checkSale, con))
                {
                    cmd.Parameters.AddWithValue("@saleId", saleId);
                    object result = cmd.ExecuteScalar();
                    isTransactionVoided = result != null && Convert.ToInt32(result) == 1;
                }

                // 🔹 Count total items and how many are voided
                int totalItems = 0;
                int totalVoided = 0;

                string countQuery = "SELECT COUNT(*) FROM salesdetails WHERE SaleID=@saleId";
                using (var cmd = new MySqlCommand(countQuery, con))
                {
                    cmd.Parameters.AddWithValue("@saleId", saleId);
                    totalItems = Convert.ToInt32(cmd.ExecuteScalar());
                }

                string voidedCountQuery = "SELECT COUNT(*) FROM salesdetails WHERE SaleID=@saleId AND IsVoided=1";
                using (var cmd = new MySqlCommand(voidedCountQuery, con))
                {
                    cmd.Parameters.AddWithValue("@saleId", saleId);
                    totalVoided = Convert.ToInt32(cmd.ExecuteScalar());
                }

                bool allItemsVoided = (totalItems > 0 && totalItems == totalVoided);

                // 🔒 Disable buttons only if transaction OR all items are voided
                if (isTransactionVoided || allItemsVoided)
                {
                    btnVoidTransaction.Enabled = false;
                    btnVoidItem.Enabled = false;
                    btnVoidTransaction.BackColor = Color.LightGray;
                    btnVoidItem.BackColor = Color.LightGray;

                    // 🔴 Red highlight row in dgvTransactions
                    selectedRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200); // light red
                    selectedRow.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    btnVoidTransaction.Enabled = true;
                    btnVoidItem.Enabled = true;
                    btnVoidTransaction.BackColor = SystemColors.Control;
                    btnVoidItem.BackColor = SystemColors.Control;

                    // 🔄 Reset row color (no red highlight)
                    selectedRow.DefaultCellStyle.BackColor = Color.White;
                    selectedRow.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }



        private void dgvTransactionItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgvTransactionItems.Rows[e.RowIndex].Selected = true;
        }


        // ✅ This method uses your VoidReason form
        private string ShowReasonDialog(string[] reasons)
        {
            using (var frm = new VoidReason(reasons))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    return frm.SelectedReason;
                else
                    return null;
            }
        }

        private void btnVoidTransaction_Click(object sender, EventArgs e)
        {

            if (dgvTransactions.CurrentRow == null) return;

            long saleId = Convert.ToInt64(dgvTransactions.CurrentRow.Cells["SaleID"].Value);

            // 🔒 STEP 1: Ask for admin password confirmation (masked)
            string enteredPassword = "";
            using (var pwdForm = new PasswordPromptForm())
            {
                if (pwdForm.ShowDialog() == DialogResult.OK)
                    enteredPassword = pwdForm.EnteredPassword;
            }

            if (string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show("Admin password is required to proceed.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔑 STEP 2: Hash the entered password
            string hashedPassword = HashPassword(enteredPassword);

            // 🔍 STEP 3: Verify admin password and get FullName
            string adminFullName = "";
            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();
                string checkPwd = "SELECT FullName FROM users WHERE Username='admin' AND PasswordHash=@p LIMIT 1";
                using (var cmd = new MySqlCommand(checkPwd, con))
                {
                    cmd.Parameters.AddWithValue("@p", hashedPassword);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        adminFullName = result.ToString();
                    else
                    {
                        MessageBox.Show("Incorrect admin password. Transaction voiding aborted.",
                                        "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }


            // 🟨 VALIDATION: Check if transaction is older than 1 day
            // 🟨 VALIDATION: Check if transaction is older than 15 hours
            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();
                string query = "SELECT TransactionDate FROM sales WHERE SaleID=@id";
                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", saleId);
                    object result = cmd.ExecuteScalar();

                    if (result != null && DateTime.TryParse(result.ToString(), out DateTime transDate))
                    {
                        // ✅ Check if transaction is older than 15 hours
                        if ((DateTime.Now - transDate).TotalHours > 15)
                        {
                            MessageBox.Show(
                                "This transaction is more than 15 hours old and can no longer be voided.",
                                "Void Not Allowed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                            return;
                        }
                    }
                }
            }


            // 🧩 Continue your existing "already voided" check
            bool isVoided = false;
            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("SELECT IsVoided FROM sales WHERE SaleID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", saleId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && Convert.ToInt32(result) == 1)
                        isVoided = true;
                }
            }

            if (isVoided)
            {
                MessageBox.Show("This transaction has already been voided and cannot be voided again.",
                                "Already Voided", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                btnVoidTransaction.Enabled = false;
                btnVoidItem.Enabled = false;
                return;
            }

            // Continue your normal void logic here...
        // Ask for void reason
        string[] reasons = { "OverPunch", "Product Issue", "System Error" };
            string reason = ShowReasonDialog(reasons);
            if (string.IsNullOrEmpty(reason)) return;

            var confirm = MessageBox.Show(
                $"Are you sure you want to void this entire transaction?\nReason: {reason}",
                "Confirm Void",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    try
                    {

                        // 1️⃣ Get all items in the transaction
                        string getItems = "SELECT SaleDetailID, ProductID, Quantity, SubTotal FROM salesdetails WHERE SaleID=@saleId";
                        var items = new List<(long SaleDetailID, int ProductID, int Quantity, decimal SubTotal)>();

                        using (var cmd = new MySqlCommand(getItems, con, trans))
                        {
                            cmd.Parameters.AddWithValue("@saleId", saleId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    items.Add((
                                        reader.GetInt64("SaleDetailID"),
                                        reader.GetInt32("ProductID"),
                                        reader.GetInt32("Quantity"),
                                        reader.GetDecimal("SubTotal")
                                    ));
                                }
                            }
                        }

                        decimal totalVoidedAmount = items.Sum(i => i.SubTotal);

                        // 2️⃣ Mark sales details as voided
                        string updateItems = "UPDATE salesdetails SET IsVoided=1, VoidReason=@r, VoidedBy=@user WHERE SaleID=@saleId";
                        using (var cmd = new MySqlCommand(updateItems, con, trans))
                        {
                            cmd.Parameters.AddWithValue("@saleId", saleId);
                            cmd.Parameters.AddWithValue("@r", reason);
                            cmd.Parameters.AddWithValue("@user", adminFullName);
                            cmd.Parameters.AddWithValue("@time", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }

                        // 3️⃣ Mark sale as voided
                        string updateSale = @"
                            UPDATE sales 
                            SET IsVoided=1, 
                                VoidReason=@r, 
                                VoidedBy=@user, 
                                VoidedAt=NOW()
                            WHERE SaleID=@saleId";

                        using (var cmd = new MySqlCommand(updateSale, con, trans))
                        {
                            cmd.Parameters.AddWithValue("@saleId", saleId);
                            cmd.Parameters.AddWithValue("@r", reason);
                            cmd.Parameters.AddWithValue("@user", adminFullName);
                            cmd.Parameters.AddWithValue("@NOW", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }

                        // 4️⃣ Restore inventory + reverse OUT movements
                        // 4️⃣ Handle inventory depending on reason
                        foreach (var item in items)
                        {
                            int quantity = item.Quantity;
                            int productId = item.ProductID;

                            if (reason.Equals("OverPunch", StringComparison.OrdinalIgnoreCase) ||
                                reason.Equals("System Error", StringComparison.OrdinalIgnoreCase))
                            {
                                // 🔁 Return to inventory
                                string updateStock = "UPDATE Inventory SET QuantityInStock = QuantityInStock + @qty WHERE ProductID=@pid";
                                using (var cmd = new MySqlCommand(updateStock, con, trans))
                                {
                                    cmd.Parameters.AddWithValue("@qty", quantity);
                                    cmd.Parameters.AddWithValue("@pid", productId);
                                    cmd.ExecuteNonQuery();
                                }

                                // 🗑 Remove the OUT movement
                                string reverseMovement = @"
            DELETE FROM inventory_movements
            WHERE ProductID=@pid AND MovementType='OUT' AND ReferenceID=@saleId;";
                                using (var cmd = new MySqlCommand(reverseMovement, con, trans))
                                {
                                    cmd.Parameters.AddWithValue("@pid", productId);
                                    cmd.Parameters.AddWithValue("@saleId", saleId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else if (reason.Equals("Product Issue", StringComparison.OrdinalIgnoreCase) ||
                                     reason.Equals("Damaged Item", StringComparison.OrdinalIgnoreCase))
                            {
                                // ⚠️ Do NOT return to inventory
                                // Still mark as voided (sales -), but stock stays the same.
                                // You may log it separately if needed.
                            }
                        }


                        trans.Commit();

                        // 5️⃣ Print void receipt
                        PrintVoidTransactionReceipt(saleId, reason, totalVoidedAmount);

                        MessageBox.Show("Transaction successfully voided.", "Void", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // ✅ Remember current SaleID before reload
                        long selectedSaleID = saleId;

                        // ✅ Reload
                        LoadTransactions();

                        // ✅ Reselect same row after reload
                        foreach (DataGridViewRow row in dgvTransactions.Rows)
                        {
                            if (Convert.ToInt64(row.Cells["SaleID"].Value) == selectedSaleID)
                            {
                                row.Selected = true;
                                dgvTransactions.CurrentCell = row.Cells["TransactionDate"]; // focus any visible cell
                                dgvTransactions.FirstDisplayedScrollingRowIndex = row.Index; // keep scroll position
                                break;
                            }
                        }

                        // 🔄 Trigger the SelectionChanged manually so items refresh automatically
                        dgvTransactions_SelectionChanged(null, null);


                        // 🔒 Disable void buttons after success
                        btnVoidTransaction.Enabled = false;
                        btnVoidItem.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        try { trans.Rollback(); } catch { }
                        MessageBox.Show("Error voiding transaction: " + ex.Message);
                    }
                }
            }
        }

        private void PrintVoidTransactionReceipt(long saleId, string reason, decimal totalAmount)
        {
            PrintDocument pd = new PrintDocument();

            // Optional: Remove the default print dialog pop-up
            pd.PrintController = new StandardPrintController();

            // Set custom paper size (e.g., 80mm width, and enough height)
            PaperSize paperSize = new PaperSize("Custom", 280, 1000); // width in hundredths of an inch (e.g. 280 = 2.8 inches)
            pd.DefaultPageSettings.PaperSize = paperSize;

            // Optional: Remove margins
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            pd.PrintPage += (s, ev) =>
            {
                float y = 20;
                Font font = new Font("Consolas", 9);

                ev.Graphics.DrawString("** VOID RECEIPT **", new Font("Consolas", 10, FontStyle.Bold), Brushes.Black, 10, y);
                y += 20;
                ev.Graphics.DrawString($"Sale ID: {saleId}", font, Brushes.Black, 10, y);
                y += 20;
                ev.Graphics.DrawString($"Reason: {reason}", font, Brushes.Black, 10, y);
                y += 20;
                ev.Graphics.DrawString($"Total Amount Voided: ₱{totalAmount:N2}", font, Brushes.Black, 10, y);
                y += 20;
                ev.Graphics.DrawString($"Date: {DateTime.Now}", font, Brushes.Black, 10, y);
                y += 20;
                ev.Graphics.DrawString("Voided by: " + ConnectionModule.Session.FullName, font, Brushes.Black, 10, y);

            };

            pd.Print();
        }

        // 🔐 Hash password using SHA256 (same as in registration)
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

        private void btnVoidItem_Click(object sender, EventArgs e)
        {
            if (dgvTransactionItems.CurrentRow == null) return;

            bool isVoided = Convert.ToBoolean(dgvTransactionItems.CurrentRow.Cells["IsVoided"].Value);
            if (isVoided)
            {
                MessageBox.Show("This item is already voided.", "Void", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long saleDetailId = Convert.ToInt64(dgvTransactionItems.CurrentRow.Cells["SaleDetailID"].Value);
            long saleId = Convert.ToInt64(dgvTransactionItems.CurrentRow.Cells["SaleID"].Value);

            // 🟨 VALIDATION: Check if transaction is older than 15 hours
            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();
                string query = "SELECT TransactionDate FROM sales WHERE SaleID=@id";
                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", saleId);
                    object result = cmd.ExecuteScalar();

                    if (result != null && DateTime.TryParse(result.ToString(), out DateTime transDate))
                    {
                        // ✅ Check if transaction is older than 15 hours
                        if ((DateTime.Now - transDate).TotalHours > 15)
                        {
                            MessageBox.Show(
                                "This transaction is more than 15 hours old and can no longer be voided.",
                                "Void Not Allowed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                            return;
                        }
                    }
                }
            }
            // 🔒 STEP 1: Ask for admin password confirmation (masked)
            string enteredPassword = "";
            using (var pwdForm = new PasswordPromptForm())
            {
                if (pwdForm.ShowDialog() == DialogResult.OK)
                    enteredPassword = pwdForm.EnteredPassword;
            }

            if (string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show("Admin password is required to proceed.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔑 STEP 2: Hash the entered password
            string hashedPassword = HashPassword(enteredPassword);

            // 🔍 STEP 3: Verify admin password and get FullName
            string adminFullName = "";
            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();
                string checkPwd = "SELECT FullName FROM users WHERE Username='admin' AND PasswordHash=@p LIMIT 1";
                using (var cmd = new MySqlCommand(checkPwd, con))
                {
                    cmd.Parameters.AddWithValue("@p", hashedPassword);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        adminFullName = result.ToString();
                    else
                    {
                        MessageBox.Show("Incorrect admin password. Transaction voiding aborted.",
                                        "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            int quantity = Convert.ToInt32(dgvTransactionItems.CurrentRow.Cells["Quantity"].Value);
            decimal unitPrice = Convert.ToDecimal(dgvTransactionItems.CurrentRow.Cells["UnitPrice"].Value);
            int productId = Convert.ToInt32(dgvTransactionItems.CurrentRow.Cells["ProductID"].Value);
            decimal subTotal = Convert.ToDecimal(dgvTransactionItems.CurrentRow.Cells["SubTotal"].Value);

            // 🟩 Step 1: Ask for reason first
            string[] reasons = { "OverPunch", "Product Issue", "System Error" };
            string reason = ShowReasonDialog(reasons);
            if (string.IsNullOrEmpty(reason)) return;

            // 🟩 Step 2: Ask how many to void
            string input = ShowInputBox($"Enter quantity to void (Max: {quantity}):", "Void Quantity", "1");
            if (string.IsNullOrWhiteSpace(input)) return;

            if (!int.TryParse(input, out int voidQty) || voidQty <= 0)
            {
                MessageBox.Show("Invalid quantity entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (voidQty > quantity)
            {
                MessageBox.Show($"Cannot void more than purchased quantity ({quantity}).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to void {voidQty} pcs of this item?\nReason: {reason}",
                "Confirm Void", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            // 🧩 Your existing DB transaction logic continues here ...

            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ Get current quantity and compute new values
                        // 1️⃣ Compute new values
                        int newQty = quantity - voidQty;
                        decimal newSubtotal = newQty * unitPrice;
                        decimal voidedAmount = voidQty * unitPrice;

                        string updateItem = @"
                        UPDATE salesdetails
                            SET Quantity = @newQty,
                                SubTotal = @subtotal,
                                VoidedAmount = IFNULL(VoidedAmount, 0) + @voidedAmount,
                               IsVoided = 1,
                                VoidReason = @reason,
                                VoidedBy = @user,
                                VoidedAt = NOW()
                        WHERE SaleDetailID = @id;
                    ";

                        using (var cmd = new MySqlCommand(updateItem, con, trans))
                        {
                            cmd.Parameters.AddWithValue("@newQty", newQty);
                            cmd.Parameters.AddWithValue("@subtotal", newSubtotal);
                            cmd.Parameters.AddWithValue("@voidedAmount", voidedAmount);
                            cmd.Parameters.AddWithValue("@reason", reason);
                            cmd.Parameters.AddWithValue("@id", saleDetailId);
                            cmd.Parameters.AddWithValue("@user", adminFullName);
                            cmd.ExecuteNonQuery();
                        }



                        // 2️⃣ Update inventory + delivery_details if OverPunch
                        if (reason == "OverPunch")
                        {
                            // a. Return to inventory
                            string updateStock = "UPDATE Inventory SET QuantityInStock = QuantityInStock + @qty WHERE ProductID=@pid";
                            using (var cmd = new MySqlCommand(updateStock, con, trans))
                            {
                                cmd.Parameters.AddWithValue("@qty", voidQty);
                                cmd.Parameters.AddWithValue("@pid", productId);
                                cmd.ExecuteNonQuery();
                            }

                            // b. Restore to delivery_details FIFO (oldest first)
                            string getBatches = @"
                        SELECT dd.idDetail, dd.QtyDelivered
                        FROM delivery_details dd
                        INNER JOIN delivery d ON dd.idDelivery = d.idDelivery
                        WHERE dd.ProductID=@productId
                        ORDER BY d.DeliveryDate ASC";

                            using (var cmd = new MySqlCommand(getBatches, con, trans))
                            {
                                cmd.Parameters.AddWithValue("@productId", productId);
                                using (var reader = cmd.ExecuteReader())
                                {
                                    int remainingQty = voidQty;
                                    var batches = new List<(long idDetail, int qtyDelivered)>();

                                    while (reader.Read())
                                    {
                                        long idDetail = reader.GetInt64("idDetail");
                                        int qtyDelivered = reader.GetInt32("QtyDelivered");
                                        batches.Add((idDetail, qtyDelivered));
                                    }

                                    reader.Close();

                                    foreach (var batch in batches)
                                    {
                                        if (remainingQty <= 0) break;

                                        int restoreQty = remainingQty;
                                        string updateBatch = "UPDATE delivery_details SET QtyDelivered = QtyDelivered + @qty WHERE idDetail=@id";
                                        using (var cmd2 = new MySqlCommand(updateBatch, con, trans))
                                        {
                                            cmd2.Parameters.AddWithValue("@qty", restoreQty);
                                            cmd2.Parameters.AddWithValue("@id", batch.idDetail);
                                            cmd2.ExecuteNonQuery();
                                        }

                                        remainingQty -= restoreQty;
                                    }
                                }
                            }
                        }
                       

                        trans.Commit();

                        string productName = "";
                        try
                        {
                            ConnectionModule.openCon();
                            using (var cmd = new MySqlCommand(@"
        SELECT p.ProductName
        FROM salesdetails s
        INNER JOIN product p ON s.ProductID = p.ProductID
        WHERE s.SaleDetailID = @SaleDetailID
        LIMIT 1", ConnectionModule.con)) // ✅ correct: s.ID not s.ProductID
                            {
                                cmd.Parameters.AddWithValue("@SaleDetailID", saleDetailId); // correct param name
                                var result = cmd.ExecuteScalar();
                                if (result != null)
                                    productName = result.ToString();
                                else
                                    MessageBox.Show("⚠️ Product not found for this SaleDetail ID.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("❌ Error getting product name: " + ex.Message);
                        }
                        finally
                        {
                            ConnectionModule.closeCon();
                        }

                        // Now we can safely print
                        if (!string.IsNullOrWhiteSpace(productName))
                        {
                            PrintVoidReceipt(productName, reason, subTotal);
                        }
                        else
                        {
                            MessageBox.Show("❌ Cannot print void receipt: product name is blank.");
                        }




                        MessageBox.Show($"{voidQty} item(s) successfully voided.", "Void", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 4️⃣ Refresh UI (reload transaction list and items)
                        LoadTransactions();

                        // 🔁 Re-select same SaleID para ma-trigger ulit ang dgvTransactionItems load
                        foreach (DataGridViewRow row in dgvTransactions.Rows)
                        {
                            if (Convert.ToInt64(row.Cells["SaleID"].Value) == saleId)
                            {
                                row.Selected = true;
                                dgvTransactions.CurrentCell = row.Cells["TransactionDate"]; // focus column
                                break;
                            }
                        }

                        // 🔄 Manually trigger selection event to reload transaction items
                        dgvTransactions_SelectionChanged(null, null);




                    }
                    catch (Exception ex)
                    {
                        try { trans.Rollback(); } catch { }
                        MessageBox.Show("Error voiding item: " + ex.Message);
                    }
                }
            }
        }
      


        private string ShowInputBox(string text, string caption, string defaultValue)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, Width = 280 };
            TextBox inputBox = new TextBox() { Left = 20, Top = 50, Width = 280, Text = defaultValue };
            Button okButton = new Button() { Text = "OK", Left = 230, Width = 70, Top = 80, DialogResult = DialogResult.OK };
            okButton.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(okButton);
            prompt.AcceptButton = okButton;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : "";
        }


        private void PrintVoidReceipt(string productName, string reason, decimal subTotal)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (s, ev) =>
            {
                float y = 10;
                Font font = new Font("Consolas", 11); // bigger font size
                float marginLeft = 10;
                float pageWidth = ev.PageBounds.Width;
                float printableWidth = pageWidth - 2 * marginLeft;

                ev.Graphics.DrawString("** VOID RECEIPT **", new Font("Consolas", 12, FontStyle.Bold), Brushes.Black, marginLeft, y);
                y += 25;

                // Wrap text for product name
                RectangleF productRect = new RectangleF(marginLeft, y, printableWidth, 100);
                StringFormat format = new StringFormat();
                format.FormatFlags = 0; // wrap enabled

                ev.Graphics.DrawString($"Voided Product: {productName}", font, Brushes.Black, productRect, format);
                y += ev.Graphics.MeasureString(productName, font, (int)printableWidth).Height + 15;

                ev.Graphics.DrawString($"Reason: {reason}", font, Brushes.Black, marginLeft, y);
                y += 25;
                ev.Graphics.DrawString($"Amount Voided: ₱{subTotal:N2}", font, Brushes.Black, marginLeft, y);
                y += 25;
                ev.Graphics.DrawString($"Date: {DateTime.Now}", font, Brushes.Black, marginLeft, y);
                y += 35;
                ev.Graphics.DrawString("Voided by: " + ConnectionModule.Session.FullName, font, Brushes.Black, marginLeft, y);
            };

            pd.Print();
        }





        private void dgvTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvTransactionItems_DataBindingComplete_1(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvTransactionItems.ClearSelection();
            dgvTransactionItems.CurrentCell = null;
            this.ActiveControl = null;

            foreach (DataGridViewRow row in dgvTransactionItems.Rows)
            {
                if (row.Cells["IsVoided"].Value != DBNull.Value)
                {
                    bool isVoided = Convert.ToInt32(row.Cells["IsVoided"].Value) == 1;
                    if (isVoided)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Strikeout);
                        row.DefaultCellStyle.SelectionBackColor = Color.MistyRose;
                        row.DefaultCellStyle.SelectionForeColor = Color.Red;
                    }
                }
            }
        }


        private void dgvTransactions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvTransactions.ClearSelection();
            dgvTransactions.CurrentCell = null;
            this.ActiveControl = null;

            // Build list of SaleIDs present in the grid
            var saleIds = new List<long>();
            foreach (DataGridViewRow row in dgvTransactions.Rows)
            {
                if (row.Cells["SaleID"].Value == null) continue;
                if (long.TryParse(row.Cells["SaleID"].Value.ToString(), out long sid))
                    saleIds.Add(sid);
            }

            // If no sale ids, nothing to do
            if (saleIds.Count == 0) return;

            // Prepare a parameterized query to get total items and voided items per SaleID
            var counts = new Dictionary<long, (int totalItems, int voidedItems)>();
            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();

                // create parameter names @p0,@p1,...
                var paramNames = saleIds.Select((id, idx) => "@p" + idx).ToArray();
                string sql = $@"
            SELECT SaleID, COUNT(*) AS TotalItems,
                   SUM(CASE WHEN IsVoided=1 THEN 1 ELSE 0 END) AS VoidedItems
            FROM salesdetails
            WHERE SaleID IN ({string.Join(",", paramNames)})
            GROUP BY SaleID;";

                using (var cmd = new MySqlCommand(sql, con))
                {
                    for (int i = 0; i < saleIds.Count; i++)
                        cmd.Parameters.AddWithValue(paramNames[i], saleIds[i]);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long sid = reader.GetInt64("SaleID");
                            int total = reader.IsDBNull(reader.GetOrdinal("TotalItems")) ? 0 : reader.GetInt32("TotalItems");
                            int voided = reader.IsDBNull(reader.GetOrdinal("VoidedItems")) ? 0 : Convert.ToInt32(reader["VoidedItems"]);
                            counts[sid] = (total, voided);
                        }
                    }
                }
            }

            // Now apply styling: mark row red ONLY if sale.IsVoided == 1 OR (totalItems > 0 && voidedItems == totalItems)
            foreach (DataGridViewRow row in dgvTransactions.Rows)
            {
                if (row.Cells["SaleID"].Value == null) continue;
                long sid = Convert.ToInt64(row.Cells["SaleID"].Value);

                bool isVoided = false;
                if (row.Cells["IsVoided"].Value != DBNull.Value)
                    isVoided = Convert.ToInt32(row.Cells["IsVoided"].Value) == 1;

                bool allItemsVoided = false;
                if (counts.TryGetValue(sid, out var t))
                {
                    allItemsVoided = (t.totalItems > 0 && t.totalItems == t.voidedItems);
                }

                if (isVoided || allItemsVoided)
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Strikeout);
                    row.DefaultCellStyle.SelectionBackColor = Color.MistyRose;
                    row.DefaultCellStyle.SelectionForeColor = Color.Red;
                }
                else
                {
                    // reset to normal in case styles persisted
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    row.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                }
            }
        }



    }
}
