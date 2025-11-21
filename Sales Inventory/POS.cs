using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization; // make sure this is at the top of your file
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Controls;
using System.Windows.Forms;
using ContextMenu = System.Windows.Forms.ContextMenu;
using TextBox = System.Windows.Forms.TextBox;

namespace Sales_Inventory
{
    public partial class POS : Form
    {
        // Fields to store member and points info
        private decimal RedeemPoints = 0;
        private int? MemberId = null;
        private decimal netAmount = 0; // set this from cart total
        bool isVatExemptApplied = false; // global variable sa POS form
        public string MemberCode { get; set; } // set from POS form
        private string AppliedDiscountType;
        private string AppliedDiscountFullName;
        private string AppliedDiscountIDNumber;

        decimal cashSales = 0;
        decimal gcashSales = 0;
        decimal pointsSales = 0;
        // Discount counts
        int seniorCount, pwdCount;

        // Global variables (top of your POS form)
        decimal grossSales, totalDiscounts, voidedSales, netSales;
        decimal pwdDiscount, seniorDiscount, shiftTotal;
        DateTime shiftStartTime, shiftEndTime;
        // sa taas ng class mo (POS.cs o saan ka naglalagay ng print logic)
        private string printMode = "";

        public POS(string role)
        {
            InitializeComponent();
            SetupDgvProduct();
            dgvProduct.CellClick += dgvProduct_CellClick;
            this.KeyPreview = true; // 👉 Important para mahuli ng form ang key press kahit may focus sa control
            this.KeyDown += new KeyEventHandler(btnSave_KeyDown);

            this.KeyPreview = true; // 🟢 kailangan para gumana ang keyboard shortcuts
            this.KeyDown += new KeyEventHandler(guna2Button5_KeyDown);

            this.KeyPreview = true; // 🟢 kailangan para gumana ang keyboard shortcuts
            this.KeyDown += new KeyEventHandler(btnDiscount_KeyDown);

            this.KeyPreview = true; // 🟢 kailangan para gumana ang keyboard shortcuts
            this.KeyDown += new KeyEventHandler(guna2Button4_KeyDown);

            this.KeyPreview = true; // 🟢 kailangan para gumana ang keyboard shortcuts
            this.KeyDown += new KeyEventHandler(guna2Button2_KeyDown);

            this.KeyPreview = true; // 🟢 kailangan para gumana ang keyboard shortcuts
            this.KeyDown += new KeyEventHandler(txtEndShift_KeyDown);

            this.KeyPreview = true; // 🟢 kailangan para gumana ang keyboard shortcuts
            this.KeyDown += new KeyEventHandler(txtTransactionHistory_KeyDown);


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox9_TextChanged(object sender, EventArgs e)
        {

        }
        private decimal GetVatRateFromDatabase()
        {
            decimal vatRate = 0.00m;

            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query = "SELECT VatRate FROM VatTable LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            vatRate = Convert.ToDecimal(result) / 100;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving VAT rate: " + ex.Message);
            }

            return vatRate;
        }


        private void ComputeTotals()
        {
            decimal subTotal = 0;
            decimal discount = 0;        // for Senior/PWD only
            decimal redeemedPoints = 0;  // for member points
            decimal vatRate = GetVatRateFromDatabase();

            // 1️⃣ Compute subtotal (price × quantity)
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                if (row.Cells["PriceColumn"].Value != null && row.Cells["QuantityColumn"].Value != null)
                {
                    decimal price = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
                    int qty = Convert.ToInt32(row.Cells["QuantityColumn"].Value);
                    subTotal += price * qty;
                }
            }

            // 2️⃣ Get Senior/PWD discount
            if (!string.IsNullOrWhiteSpace(txtDiscount.Text))
                discount = Convert.ToDecimal(txtDiscount.Text);

            // 3️⃣ Get Redeemed Points (deduction)
            if (!string.IsNullOrWhiteSpace(txtRedeemedPoints.Text))
                redeemedPoints = Convert.ToDecimal(txtRedeemedPoints.Text);

            // 4️⃣ Compute total
            decimal totalAfterDiscountAndPoints = subTotal - discount - redeemedPoints;
            if (totalAfterDiscountAndPoints < 0)
                totalAfterDiscountAndPoints = 0;

            // 5️⃣ VAT breakdown
            decimal vatableSales = 0;
            decimal vatAmount = 0;
            decimal vatExempt = 0;

            if (isVatExemptApplied)
            {
                vatExempt = totalAfterDiscountAndPoints;
            }
            else
            {
                vatableSales = totalAfterDiscountAndPoints / (1 + vatRate);
                vatAmount = totalAfterDiscountAndPoints - vatableSales;
            }

            // 6️⃣ Update UI
            txtSubTotal.Text = subTotal.ToString("N2");
            txtVatableSales.Text = vatableSales.ToString("N2");
            txtVatAmount.Text = vatAmount.ToString("N2");
            txtVatExempt.Text = vatExempt.ToString("N2");
            txtTotal.Text = totalAfterDiscountAndPoints.ToString("N2");
        }


        bool quantityWarningShown = false;

        private void txtQuantity_TextChanged_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                quantityWarningShown = false;
                return;
            }

            // Use long instead of int
            if (!long.TryParse(txtQuantity.Text, out long qty) || qty <= 0)
            {
                if (!quantityWarningShown)
                {
                    MessageBox.Show("Please enter a valid quantity greater than 0.",
                                    "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    quantityWarningShown = true;
                }

                txtQuantity.Clear();
                return;
            }

            quantityWarningShown = false;
        }

        private void LoadDataByBarcode(string barcode)
        {
            try
            {
                ConnectionModule.openCon();

                string query = @"
        SELECT p.ProductID, p.ProductName, p.Description,
               i.QuantityInStock, p.RetailPrice, p.WholeSalePrice, p.Barcode
        FROM Product p
        INNER JOIN Inventory i ON p.ProductID = i.ProductID
        WHERE p.Barcode = @barcode";

                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@barcode", barcode);

                    using (MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            int productId = Convert.ToInt32(reader["ProductID"]);
                            string productName = reader["ProductName"].ToString();
                            string description = reader["Description"].ToString();
                            int stock = Convert.ToInt32(reader["QuantityInStock"]);
                            decimal retailPrice = Convert.ToDecimal(reader["RetailPrice"]);
                            decimal wholesalePrice = Convert.ToDecimal(reader["WholeSalePrice"]);

                            if (stock <= 0)
                            {
                                MessageBox.Show($"The product \"{productName}\" is out of stock.",
                                    "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            int inputQty = 1;
                            if (!string.IsNullOrWhiteSpace(txtQuantity.Text) &&
                                int.TryParse(txtQuantity.Text, out int q) && q > 0)
                            {
                                inputQty = q;
                            }

                            if (inputQty > stock)
                            {
                                MessageBox.Show($"Only {stock} pcs available for \"{productName}\".",
                                    "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            bool found = false;

                            foreach (DataGridViewRow row in dgvProduct.Rows)
                            {
                                if (row.Cells["ProductNameColumn"].Value != null &&
                                    row.Cells["ProductNameColumn"].Value.ToString().Equals(productName, StringComparison.OrdinalIgnoreCase))
                                {
                                    // 🚫 Prevent quantity add if discounted
                                    decimal existingDiscount = row.Cells["DiscountColumn"].Value != null
                                        ? Convert.ToDecimal(row.Cells["DiscountColumn"].Value)
                                        : 0;

                                    if (existingDiscount > 0)
                                    {
                                        MessageBox.Show("Cannot add quantity for a discounted item.",
                                            "Action Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    int currentQty = Convert.ToInt32(row.Cells["QuantityColumn"].Value);
                                    int newQty = currentQty + inputQty;

                                    if (newQty > stock)
                                    {
                                        MessageBox.Show($"Cannot add more. Only {stock} pcs available.",
                                            "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    decimal unitPrice = (newQty >= 15) ? wholesalePrice : retailPrice;
                                    decimal rowDiscount = 0;
                                    if (row.Cells["DiscountColumn"].Value != null)
                                        rowDiscount = Convert.ToDecimal(row.Cells["DiscountColumn"].Value);

                                    row.Cells["QuantityColumn"].Value = newQty;
                                    row.Cells["PriceColumn"].Value = unitPrice;
                                    row.Cells["TotalPriceColumn"].Value = (unitPrice * newQty - rowDiscount);

                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                decimal unitPrice = (inputQty >= 15) ? wholesalePrice : retailPrice;

                                int newRowIndex = dgvProduct.Rows.Add();
                                DataGridViewRow newRow = dgvProduct.Rows[newRowIndex];

                                newRow.Cells["ProductIDColumn"].Value = productId;
                                newRow.Cells["BarcodeColumn"].Value = barcode;
                                newRow.Cells["ProductNameColumn"].Value = productName;
                                newRow.Cells["DescriptionColumn"].Value = description;
                                newRow.Cells["QuantityColumn"].Value = inputQty;
                                newRow.Cells["PriceColumn"].Value = unitPrice;
                                newRow.Cells["TotalPriceColumn"].Value = unitPrice * inputQty;
                                newRow.Cells["StockColumn"].Value = stock;
                                newRow.Cells["DiscountColumn"].Value = 0; // no discount yet

                                newRow.Tag = new { Retail = retailPrice, Wholesale = wholesalePrice };
                            }

                            dgvProduct.ClearSelection();

                            ComputeTotals();
                        }
                        else
                        {
                            MessageBox.Show("No product found with this barcode.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }


        private int GetAvailableStockFromInventory(string barcode)
        {
            int stock = 0;
            string query = "SELECT QuantityInStock FROM inventory i " +
                           "INNER JOIN product p ON i.idProduct = p.idProduct " +
                           "WHERE p.Barcode = @barcode";

            using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
            {

                {
                    cmd.Parameters.AddWithValue("@barcode", barcode);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        stock = Convert.ToInt32(result);
                }
            }
            return stock;
        }


        private void AddProductToCart(string barcode, string name, decimal retailPrice, decimal wholesalePrice, int wholesaleThreshold)
        {
            try
            {
                // 🔹 Open connection
                ConnectionModule.openCon();

                // 🔹 Get available stock from Inventory
                string query = @"
            SELECT i.QuantityInStock 
            FROM Product p
            INNER JOIN Inventory i ON p.ProductID = i.ProductID
            WHERE p.Barcode = @barcode";

                int availableStock = 0;
                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@barcode", barcode);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        availableStock = Convert.ToInt32(result);
                }

                // 🚫 No stock available
                if (availableStock <= 0)
                {
                    MessageBox.Show("This product is out of stock.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🔹 Get desired quantity (default 1)
                int qty = 1;
                if (!string.IsNullOrWhiteSpace(txtQuantity.Text) && int.TryParse(txtQuantity.Text, out int q) && q > 0)
                    qty = q;

                // 🚫 Check kung requested qty > stock
                if (qty > availableStock)
                {
                    MessageBox.Show($"Only {availableStock} pcs available in stock.", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🔹 Determine unit price
                decimal unitPrice = GetUnitPrice(retailPrice, wholesalePrice, qty, wholesaleThreshold);

                // 🔹 Check kung existing na sa cart
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    if (row.Cells["BarcodeColumn"].Value != null && row.Cells["BarcodeColumn"].Value.ToString() == barcode)
                    {
                        int existingQty = Convert.ToInt32(row.Cells["QuantityColumn"].Value);
                        int newQty = existingQty + qty;

                        // ⚠ Prevent exceeding available stock
                        if (newQty > availableStock)
                        {
                            MessageBox.Show(
                                $"Cannot add more. Only {availableStock} pcs available.",
                                "Insufficient Stock",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            return;
                        }

                        // Update price if threshold reached
                        unitPrice = GetUnitPrice(retailPrice, wholesalePrice, newQty, wholesaleThreshold);

                        row.Cells["QuantityColumn"].Value = newQty;
                        row.Cells["PriceColumn"].Value = unitPrice.ToString("N2");
                        row.Cells["TotalPriceColumn"].Value = (unitPrice * newQty).ToString("N2");

                        ComputeTotals();
                        return;
                    }
                }

                // 🔹 If new item and within stock
                int newRow = dgvProduct.Rows.Add();
                dgvProduct.Rows[newRow].Cells["BarcodeColumn"].Value = barcode;
                dgvProduct.Rows[newRow].Cells["ProductNameColumn"].Value = name;
                dgvProduct.Rows[newRow].Cells["QuantityColumn"].Value = qty;
                dgvProduct.Rows[newRow].Cells["PriceColumn"].Value = unitPrice.ToString("N2");
                dgvProduct.Rows[newRow].Cells["TotalPriceColumn"].Value = (unitPrice * qty).ToString("N2");

                // 🔹 Add StockColumn only once
                if (!dgvProduct.Columns.Contains("StockColumn"))
                {
                    dgvProduct.Columns.Add("StockColumn", "Stock");
                    dgvProduct.Columns["StockColumn"].Visible = false;
                }

                // Store stock and price info
                dgvProduct.Rows[newRow].Cells["StockColumn"].Value = availableStock;
                dgvProduct.Rows[newRow].Cells["PriceColumn"].Tag = new { Retail = retailPrice, Wholesale = wholesalePrice };

                ComputeTotals();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while adding product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }


        private decimal GetUnitPrice(decimal retailPrice, decimal wholesalePrice, int qty, int wholesaleThreshold)
        {
            if (qty >= wholesaleThreshold)
                return wholesalePrice;
            else
                return retailPrice;
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {

        }


        public void SetupDgvProduct()
        {
            dgvProduct.Columns.Clear();

            // Hidden columns
            dgvProduct.Columns.Add("ProductIDColumn", "ProductID");
            dgvProduct.Columns["ProductIDColumn"].Visible = false;
            dgvProduct.Columns.Add("BarcodeColumn", "Barcode");
            dgvProduct.Columns["BarcodeColumn"].Visible = false;

            // Visible columns
            dgvProduct.Columns.Add("ProductNameColumn", "Product Name");
            dgvProduct.Columns.Add("DescriptionColumn", "Description");
            dgvProduct.Columns.Add("QuantityColumn", "Quantity");
            dgvProduct.Columns.Add("PriceColumn", "Price");
            dgvProduct.Columns.Add("TotalPriceColumn", "Total");
            dgvProduct.Columns.Add("DiscountColumn", "Discount");

            // Hidden stock column
            dgvProduct.Columns.Add("StockColumn", "Stock");
            dgvProduct.Columns["StockColumn"].Visible = false;

            // Button columns
            DataGridViewButtonColumn plusCol = new DataGridViewButtonColumn();
            plusCol.Name = "PlusColumn";
            plusCol.HeaderText = "";
            plusCol.Text = "➕";
            plusCol.UseColumnTextForButtonValue = true;
            plusCol.Width = 70;
            dgvProduct.Columns.Add(plusCol);

            DataGridViewButtonColumn minusCol = new DataGridViewButtonColumn();
            minusCol.Name = "MinusColumn";
            minusCol.HeaderText = "";
            minusCol.Text = "➖";
            minusCol.UseColumnTextForButtonValue = true;
            minusCol.Width = 70;
            dgvProduct.Columns.Add(minusCol);

            // Styling
            StyleDataGridView(dgvProduct);
            dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvProduct.Columns["ProductNameColumn"].Width = 150;
            dgvProduct.Columns["DescriptionColumn"].Width = 120;
            dgvProduct.Columns["QuantityColumn"].Width = 80;
            dgvProduct.Columns["PriceColumn"].Width = 100;
            dgvProduct.Columns["TotalPriceColumn"].Width = 100;

            // ✅ Numeric formatting
            dgvProduct.Columns["PriceColumn"].DefaultCellStyle.Format = "N2";       // 180.00
            dgvProduct.Columns["TotalPriceColumn"].DefaultCellStyle.Format = "N2";  // 180.00
            dgvProduct.Columns["DiscountColumn"].DefaultCellStyle.Format = "N0";    // 20

            ComputeTotals();
        }



        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header or invalid indexes
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvProduct.Rows[e.RowIndex].IsNewRow) return;

            DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

            // ✅ Check if already discounted
            decimal existingDiscount = row.Cells["DiscountColumn"].Value != null
                ? Convert.ToDecimal(row.Cells["DiscountColumn"].Value)
                : 0;

            if (existingDiscount > 0 &&
                (dgvProduct.Columns[e.ColumnIndex].Name == "PlusColumn" ||
                 dgvProduct.Columns[e.ColumnIndex].Name == "MinusColumn"))
            {
                MessageBox.Show("Cannot change quantity for a discounted item.", "Action Not Allowed",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // prevent any quantity changes
            }

            // ===== PLUS COLUMN =====
            if (dgvProduct.Columns[e.ColumnIndex].Name == "PlusColumn")
            {
                int qty = Convert.ToInt32(row.Cells["QuantityColumn"].Value);
                int stock = Convert.ToInt32(row.Cells["StockColumn"].Value);

                if (qty + 1 > stock)
                {
                    MessageBox.Show($"Cannot add more. Only {stock} pcs available in stock.",
                        "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                qty++;
                row.Cells["QuantityColumn"].Value = qty;

                // Update price based on quantity
                if (row.Tag != null)
                {
                    dynamic priceInfo = row.Tag;
                    decimal newUnitPrice = (qty >= 15) ? priceInfo.Wholesale : priceInfo.Retail;
                    row.Cells["PriceColumn"].Value = newUnitPrice;
                    row.Cells["TotalPriceColumn"].Value = (newUnitPrice * qty).ToString("N2");
                }
                else
                {
                    decimal price = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
                    row.Cells["TotalPriceColumn"].Value = (qty * price).ToString("N2");
                }

                ComputeTotals();
            }

            // ===== MINUS COLUMN =====
            else if (dgvProduct.Columns[e.ColumnIndex].Name == "MinusColumn")
            {
                int qty = 0;
                if (row.Cells["QuantityColumn"].Value != null)
                    int.TryParse(row.Cells["QuantityColumn"].Value.ToString(), out qty);

                if (qty > 1)
                {
                    qty--;
                    row.Cells["QuantityColumn"].Value = qty;

                    if (row.Tag != null)
                    {
                        dynamic priceInfo = row.Tag;
                        decimal newUnitPrice = (qty >= 15) ? priceInfo.Wholesale : priceInfo.Retail;
                        row.Cells["PriceColumn"].Value = newUnitPrice;
                        row.Cells["TotalPriceColumn"].Value = (newUnitPrice * qty).ToString("N2");
                    }
                    else
                    {
                        decimal price = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
                        row.Cells["TotalPriceColumn"].Value = (qty * price).ToString("N2");
                    }

                    ComputeTotals();
                }
                else if (qty == 1)
                {
                    if (MessageBox.Show("Remove this item from cart?", "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dgvProduct.Rows.RemoveAt(e.RowIndex);
                        ComputeTotals();
                    }
                }
            }
        }



        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // valid row
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

                // Toggle selection
                row.Selected = !row.Selected;

                // 🔹 Important: alisin ang current focus para walang extra highlight sa gilid
                dgvProduct.CurrentCell = null;
            }
        }
        private void StyleDataGridView(DataGridView dgv)
        {
            // General appearance
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

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
            // Disable user resizing
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;

            // Columns auto-size based on content
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;


            // Prevent sorting icons & set minimum width
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (col.Name != "PlusColumn" && col.Name != "MinusColumn")
                    col.MinimumWidth = 100; // only for normal columns

            }

            // Single row selection
            dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.MultiSelect = true; // kung gusto mo makapili ng multiple products
            dgvProduct.RowHeadersVisible = false;
            dgvProduct.ClearSelection();


        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                string barcode = txtBarcode.Text.Trim();
                LoadDataByBarcode(barcode);
                txtBarcode.Clear(); // optional: para automatic clear after enter
                txtQuantity.Clear();
                dgvProduct.ClearSelection();
            }
        }
        private void dgvProduct_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // valid row lang
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

                // toggle selection
                row.Selected = !row.Selected;

                // prevent default behavior (para di auto select)
                dgvProduct.CurrentCell = null;
            }
        }


        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvProduct.CurrentRow != null && int.TryParse(txtQuantity.Text, out int qty) && qty > 0)
            //{
            //    DataGridViewRow row = dgvProduct.CurrentRow;

            //    row.Cells["QuantityColumn"].Value = qty;

            //    decimal unitPrice = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
            //    row.Cells["TotalPriceColumn"].Value = (unitPrice * qty).ToString("N2");

            //    ComputeTotals();

            //}
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // ❌ Check if DGV is empty first
            if (dgvProduct.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("No products in the transaction.",
                                "Discount Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ❌ Check if discount already applied
            bool anyDiscountApplied = dgvProduct.Rows.Cast<DataGridViewRow>()
                .Any(r => r.Cells["DiscountColumn"].Value != null && Convert.ToDecimal(r.Cells["DiscountColumn"].Value) > 0);

            if (anyDiscountApplied)
            {
                MessageBox.Show("Discount has already been applied for this transaction.",
                                "Discount Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 🧾 Non-essential categories
            List<string> nonEssentialCategories = new List<string>
{
    "Alcohol",
    "Cigarettes",
    "Tobacco",
    "Liquor",
    "Hygiene",
    "Perfume",
    "Cologne",
    "Cosmetics",
    "Makeup",
    "Skincare",
    "Haircare",
    "Deodorant",
    "Soap (Luxury)",
    "Shampoo",
    "Lotion",
    "Conditioner",
    "Toiletries",
    "Air Freshener",
    "Pet Food",
    "Pet Supplies",

};

            List<DataGridViewRow> eligibleItems = new List<DataGridViewRow>();
            bool pointsRedeemed = !string.IsNullOrWhiteSpace(txtRedeemedPoints.Text) && txtRedeemedPoints.Text != "0";

            if (pointsRedeemed)
            {
                MessageBox.Show("Discount cannot be applied because points have already been redeemed in this transaction.",
                                "Discount Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // exit early
            }

            bool hasWholesaleItem = false;
            bool hasNonEssentialCategory = false;

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();

                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    if (row.IsNewRow) continue;

                    int productId = Convert.ToInt32(row.Cells["ProductIDColumn"].Value);

                    // Check category
                    string query = @"SELECT c.CategoryName 
                         FROM category c 
                         INNER JOIN product p ON p.CategoryID = c.CategoryID 
                         WHERE p.ProductID = @ProductID";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string category = result.ToString().Trim();
                            if (nonEssentialCategories.Any(c => c.Equals(category, StringComparison.OrdinalIgnoreCase)))
                            {
                                hasNonEssentialCategory = true;
                                continue; // Skip non-essential
                            }
                        }
                    }

                    // Check wholesale price
                    decimal price = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
                    if (row.Tag != null)
                    {
                        decimal wholesalePrice = Convert.ToDecimal(
                            row.Tag.GetType().GetProperty("Wholesale").GetValue(row.Tag, null)
                        );
                        if (price <= wholesalePrice)
                        {
                            hasWholesaleItem = true;
                            continue;
                        }
                    }

                    eligibleItems.Add(row); // Add to eligible list
                }
            }

            // Decide which message to show
            if (eligibleItems.Count == 0)
            {
                if (hasWholesaleItem)
                {
                    MessageBox.Show("Discount cannot be applied on wholesale items.",
                                    "Discount Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (hasNonEssentialCategory)
                {
                    MessageBox.Show("Discount cannot be applied on non-essential categories.",
                                    "Discount Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("No eligible items found for discount.",
                                    "Discount Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }



            // Proceed to discount form
            using (Discount discountForm = new Discount(eligibleItems))
            {
                if (discountForm.ShowDialog() == DialogResult.OK)
                {
                    decimal totalDiscount = 0;

                    foreach (var result in discountForm.DiscountedItems)
                    {
                        var row = result.Row;
                        decimal price = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
                        int qty = Convert.ToInt32(row.Cells["QuantityColumn"].Value);

                        decimal subtotal = (price * qty) - result.DiscountAmount;
                        row.Cells["TotalPriceColumn"].Value = subtotal;

                        // Store applied discount info
                        row.Cells["DiscountColumn"].Value = result.DiscountAmount;
                        AppliedDiscountType = result.DiscountType;
                        AppliedDiscountFullName = result.DiscountFullName;
                        AppliedDiscountIDNumber = result.DiscountIDNumber;

                        totalDiscount += result.DiscountAmount;
                    }

                    txtDiscount.Text = totalDiscount.ToString("N2");
                    isVatExemptApplied = discountForm.IsVatExempt;
                    ComputeTotals();
                }
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            using (ViewProduct popup = new ViewProduct())
            {
                if (popup.ShowDialog() == DialogResult.OK)
                {
                    bool productExists = false;

                    try
                    {
                        ConnectionModule.openCon();

                        string query = @"
                SELECT p.ProductID, i.QuantityInStock, p.RetailPrice, p.WholeSalePrice
                FROM Product p
                INNER JOIN Inventory i ON p.ProductID = i.ProductID
                WHERE p.ProductName = @pname";

                        int stock = 0;
                        decimal retailPrice = 0;
                        decimal wholesalePrice = 0;
                        int productId = 0;

                        using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                        {
                            cmd.Parameters.AddWithValue("@pname", popup.SelectedProductName);
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    productId = Convert.ToInt32(reader["ProductID"]);
                                    stock = Convert.ToInt32(reader["QuantityInStock"]);
                                    retailPrice = Convert.ToDecimal(reader["RetailPrice"]);
                                    wholesalePrice = Convert.ToDecimal(reader["WholeSalePrice"]);
                                }
                                else
                                {
                                    MessageBox.Show("Product not found in database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }

                        if (stock <= 0)
                        {
                            MessageBox.Show($"The product \"{popup.SelectedProductName}\" is out of stock.",
                                "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        foreach (DataGridViewRow row in dgvProduct.Rows)
                        {
                            if (row.Cells["ProductNameColumn"].Value != null &&
                                row.Cells["DescriptionColumn"].Value != null &&
                                row.Cells["ProductNameColumn"].Value.ToString() == popup.SelectedProductName &&
                                row.Cells["DescriptionColumn"].Value.ToString() == popup.SelectedDescription)
                            {
                                // 🚫 Prevent quantity add if discounted
                                decimal existingDiscount = row.Cells["DiscountColumn"].Value != null
                                    ? Convert.ToDecimal(row.Cells["DiscountColumn"].Value)
                                    : 0;

                                if (existingDiscount > 0)
                                {
                                    MessageBox.Show("Cannot add quantity for a discounted item.",
                                        "Action Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                int existingQty = Convert.ToInt32(row.Cells["QuantityColumn"].Value);
                                int newQty = existingQty + popup.SelectedQuantity;

                                if (newQty > stock)
                                {
                                    MessageBox.Show($"Cannot add more. Only {stock} pcs available.",
                                        "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                decimal finalPrice = (newQty >= 15) ? wholesalePrice : retailPrice;

                                row.Cells["QuantityColumn"].Value = newQty;
                                row.Cells["PriceColumn"].Value = finalPrice.ToString("N2");
                                row.Cells["TotalPriceColumn"].Value = (newQty * finalPrice).ToString("N2");

                                productExists = true;
                                break;
                            }
                        }


                        if (!productExists)
                        {
                            if (popup.SelectedQuantity > stock)
                            {
                                MessageBox.Show($"Only {stock} pcs available.",
                                    "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            decimal finalPrice = (popup.SelectedQuantity >= 15) ? wholesalePrice : retailPrice;

                            int rowIndex = dgvProduct.Rows.Add();
                            DataGridViewRow newRow = dgvProduct.Rows[rowIndex];
                            newRow.Cells["ProductIDColumn"].Value = productId;
                            newRow.Cells["ProductNameColumn"].Value = popup.SelectedProductName;
                            newRow.Cells["DescriptionColumn"].Value = popup.SelectedDescription;
                            newRow.Cells["QuantityColumn"].Value = popup.SelectedQuantity;
                            newRow.Cells["PriceColumn"].Value = finalPrice.ToString("N2");
                            newRow.Cells["TotalPriceColumn"].Value = (finalPrice * popup.SelectedQuantity).ToString("N2");
                            newRow.Cells["StockColumn"].Value = stock;
                            newRow.Cells["DiscountColumn"].Value = 0; // no discount yet
                            newRow.Tag = new { Retail = retailPrice, Wholesale = wholesalePrice };
                        }

                        dgvProduct.ClearSelection();
                        ComputeTotals();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while adding product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        ConnectionModule.closeCon();
                    }
                }
            }
        }


        private void guna2Button5_Click(object sender, EventArgs e)
        {
            decimal total = 0m, vat = 0m, discount = 0m, net = 0m;
            decimal.TryParse(txtTotal.Text, out total);
            decimal.TryParse(txtVatAmount.Text, out vat);
            decimal.TryParse(txtDiscount.Text, out discount);
            decimal.TryParse(txtTotal.Text, out net);

            int? memberId = null;
            string memberName = ""; // default empty

            if (!string.IsNullOrWhiteSpace(txtMemberID.Text))
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    // Get MemberID and full name from FirstName + LastName
                    string query = "SELECT MemberID, FirstName, LastName FROM members WHERE MemberCode=@code LIMIT 1";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@code", txtMemberID.Text.Trim());
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MemberId = reader["MemberID"] != DBNull.Value ? Convert.ToInt32(reader["MemberID"]) : (int?)null;
                                string first = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : "";
                                string last = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : "";
                                memberName = (first + " " + last).Trim();
                            }
                        }
                    }
                }
            }


            Payment popup = new Payment(total, vat, discount, net, dgvProduct, RedeemPoints, MemberId);

            // Pass Member info
            popup.MemberCode = txtMemberID.Text.Trim();
            popup.MemberName = memberName; // ✅ Pass the name
            popup.DiscountType = AppliedDiscountType;
            popup.DiscountFullName = AppliedDiscountFullName;
            popup.DiscountIDNumber = AppliedDiscountIDNumber;


            if (popup.ShowDialog() == DialogResult.OK)
            {
                dynamic paymentInfo = popup.Tag;
                decimal cash = paymentInfo.Cash;
                decimal gcash = paymentInfo.GCash;
                decimal change = paymentInfo.Change;
                string method = paymentInfo.PaymentMethod;

                ResetPOS();
                txtPoints.Clear();

                MessageBox.Show("Transaction saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private decimal SafeToDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            string clean = text.Replace("₱", "").Replace(",", "").Trim();

            if (decimal.TryParse(clean, NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal value))
                return value;

            return 0;
        }

        private void ProcessSaleWithPoints()
        {
            try
            {
                decimal finalNetAmount = netAmount; // total ng cart
                decimal redeemPoints = RedeemPoints;
                decimal total = SafeToDecimal(txtTotal.Text);
                decimal vat = SafeToDecimal(txtVatAmount.Text);
                decimal discount = SafeToDecimal(txtDiscount.Text);
                int? memberId = MemberId;

                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            // ✅ Resolve MemberID gamit ang MemberCode kung wala pa
                            if (!memberId.HasValue && !string.IsNullOrEmpty(txtMemberID.Text))
                            {
                                string getIdQuery = "SELECT MemberID FROM members WHERE MemberCode=@code LIMIT 1";
                                using (var getIdCmd = new MySqlCommand(getIdQuery, con, trans))
                                {
                                    getIdCmd.Parameters.AddWithValue("@code", txtMemberID.Text.Trim());
                                    var result = getIdCmd.ExecuteScalar();
                                    if (result != null)
                                        memberId = Convert.ToInt32(result);
                                }
                            }

                            decimal earnedPoints = memberId.HasValue ? Math.Round(finalNetAmount * 0.01m, 2) : 0;

                            // 1️⃣ Insert sale
                            string insertSales = @"
INSERT INTO sales
(TransactionDate, MemberID, TotalAmount, VatAmount, DiscountAmount, NetAmount, RedeemedPoints, EarnedPoints, PaymentMethod, CashierName, isVoided)
VALUES (@date, @member, @total, @vat, @discount, @net, @redeem, @earned, 'Points', @cashier, 0)";
                            using (var cmd = new MySqlCommand(insertSales, con, trans))
                            {
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@member", (object)memberId ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@total", total);
                                cmd.Parameters.AddWithValue("@vat", vat);
                                cmd.Parameters.AddWithValue("@discount", discount);
                                cmd.Parameters.AddWithValue("@net", finalNetAmount);
                                cmd.Parameters.AddWithValue("@redeem", redeemPoints);
                                cmd.Parameters.AddWithValue("@earned", earnedPoints);
                                cmd.Parameters.AddWithValue("@cashier", ConnectionModule.Session.FullName);
                                cmd.ExecuteNonQuery();
                            }

                            long saleId = Convert.ToInt64(new MySqlCommand("SELECT LAST_INSERT_ID();", con, trans).ExecuteScalar());

                            // 2️⃣ Process each cart item with batch-level stock deduction
                            foreach (DataGridViewRow row in dgvProduct.Rows)
                            {
                                if (row.IsNewRow) continue;

                                int productId = Convert.ToInt32(row.Cells["ProductIDColumn"].Value);
                                int qtyToDeduct = Convert.ToInt32(row.Cells["QuantityColumn"].Value);
                                decimal price = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
                                decimal subtotal = Convert.ToDecimal(row.Cells["TotalPriceColumn"].Value);

                                // Insert sales details
                                string insertDetails = @"
                                INSERT INTO salesdetails
                                (SaleID, ProductID, Quantity, UnitPrice, SubTotal, IsVoided)
                                VALUES (@saleId, @productId, @qty, @price, @subtotal, 0)";
                                using (var cmdDetails = new MySqlCommand(insertDetails, con, trans))
                                {
                                    cmdDetails.Parameters.AddWithValue("@saleId", saleId);
                                    cmdDetails.Parameters.AddWithValue("@productId", productId);
                                    cmdDetails.Parameters.AddWithValue("@qty", qtyToDeduct);
                                    cmdDetails.Parameters.AddWithValue("@price", price);
                                    cmdDetails.Parameters.AddWithValue("@subtotal", subtotal);
                                    cmdDetails.ExecuteNonQuery();
                                }

                                // Deduct stock per batch
                                string getBatches = @"
                                    SELECT dd.idDetail, dd.QtyDelivered, d.DeliveryDate, dd.ExpirationDate
                                    FROM delivery_details dd
                                    INNER JOIN delivery d ON dd.idDelivery = d.idDelivery
                                    WHERE dd.ProductID=@productId
                                    ORDER BY d.DeliveryDate ASC";
                                var batches = new List<(int idDetail, int qtyDelivered, string expDate)>();
                                using (var cmdBatches = new MySqlCommand(getBatches, con, trans))
                                {
                                    cmdBatches.Parameters.AddWithValue("@productId", productId);
                                    using (var reader = cmdBatches.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            int idDetail = Convert.ToInt32(reader["idDetail"]);
                                            int batchQty = Convert.ToInt32(reader["QtyDelivered"]);
                                            string expDate = reader["ExpirationDate"] == DBNull.Value ? "No Expiration" : reader["ExpirationDate"].ToString();
                                            batches.Add((idDetail, batchQty, expDate));
                                        }
                                    }
                                }

                                int remainingQty = qtyToDeduct;
                                foreach (var batch in batches)
                                {
                                    if (remainingQty <= 0) break;

                                    // Check sold qty
                                    string soldQtyQuery = @"SELECT IFNULL(SUM(Quantity),0) FROM inventory_movements 
                                    WHERE idDetail=@idDetail AND ProductID=@productId AND MovementType='OUT'";
                                    int soldQty = Convert.ToInt32(new MySqlCommand(soldQtyQuery, con, trans)
                                    {
                                        Parameters = {
                                    new MySqlParameter("@idDetail", batch.idDetail),
                                    new MySqlParameter("@productId", productId)
                                }
                                    }.ExecuteScalar());

                                    int availableQty = batch.qtyDelivered - soldQty;
                                    if (availableQty <= 0) continue;

                                    int deduct = Math.Min(availableQty, remainingQty);
                                    remainingQty -= deduct;

                                    // Update inventory and insert movement
                                    string updateInventory = "UPDATE inventory SET QuantityInStock = QuantityInStock - @qty WHERE ProductID = @productId";
                                    using (var cmdInv = new MySqlCommand(updateInventory, con, trans))
                                    {
                                        cmdInv.Parameters.AddWithValue("@qty", deduct);
                                        cmdInv.Parameters.AddWithValue("@productId", productId);
                                        cmdInv.ExecuteNonQuery();
                                    }

                                    string insertMovement = @"
INSERT INTO inventory_movements
(idDetail, ProductID, MovementType, Quantity, ExpirationDate, MovementDate, Source, ReferenceID, Remarks)
VALUES (@idDetail, @productId, 'OUT', @qty, @expDate, NOW(), 'Points Sale', @saleId, CONCAT('Redeemed points, batch exp: ', @expDate))";
                                    using (var cmdMove = new MySqlCommand(insertMovement, con, trans))
                                    {
                                        cmdMove.Parameters.AddWithValue("@idDetail", batch.idDetail);
                                        cmdMove.Parameters.AddWithValue("@productId", productId);
                                        cmdMove.Parameters.AddWithValue("@qty", deduct);
                                        cmdMove.Parameters.AddWithValue("@expDate", batch.expDate);
                                        cmdMove.Parameters.AddWithValue("@saleId", saleId);
                                        cmdMove.ExecuteNonQuery();
                                    }
                                }

                                if (remainingQty > 0)
                                    throw new Exception($"Not enough stock for ProductID {productId}");
                            }

                            // 3️⃣ Update member points
                            if (memberId.HasValue)
                            {
                                string updatePoints = @"UPDATE members SET Points = Points - @redeem + @earned WHERE MemberID=@memberId";
                                using (var cmdPoints = new MySqlCommand(updatePoints, con, trans))
                                {
                                    cmdPoints.Parameters.AddWithValue("@redeem", redeemPoints);
                                    cmdPoints.Parameters.AddWithValue("@earned", earnedPoints);
                                    cmdPoints.Parameters.AddWithValue("@memberId", memberId.Value);
                                    cmdPoints.ExecuteNonQuery();
                                }

                                string insertHistory = @"INSERT INTO points_history (MemberID, SaleID, PointsEarned, PointsRedeemed)
VALUES (@memberId, @saleId, @earned, @redeem)";
                                using (var cmdHist = new MySqlCommand(insertHistory, con, trans))
                                {
                                    cmdHist.Parameters.AddWithValue("@memberId", memberId.Value);
                                    cmdHist.Parameters.AddWithValue("@saleId", saleId);
                                    cmdHist.Parameters.AddWithValue("@earned", earnedPoints);
                                    cmdHist.Parameters.AddWithValue("@redeem", redeemPoints);
                                    cmdHist.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();

                            // Auto-print points-only receipt
                            PrintReceiptPoints(saleId, redeemPoints, earnedPoints);

                            MessageBox.Show("Payment completed using points!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            // Clear POS
                            txtMemberID.Clear();
                            dgvProduct.Rows.Clear();
                            txtTotal.Text = "0.00";
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            MessageBox.Show("Error processing points payment: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void txtMemberID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            string memberCode = txtMemberID.Text.Trim();
            if (string.IsNullOrEmpty(memberCode)) return;

            // ⚠️ Allow opening MemberPoints even if cart is empty
            bool isCartEmpty = dgvProduct.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow);


            // ✅ Check if any discount is already applied
            bool anyDiscountApplied = dgvProduct.Rows.Cast<DataGridViewRow>()
                .Any(r => r.Cells["DiscountColumn"].Value != null &&
                          decimal.TryParse(r.Cells["DiscountColumn"].Value.ToString(), out decimal val) &&
                          val > 0);

            if (anyDiscountApplied)
            {
                MessageBox.Show("You cannot use member points because a discount has already been applied.",
                                "Member Points Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MemberPoints frm = new MemberPoints(memberCode))
            {
                decimal currentTotal = string.IsNullOrWhiteSpace(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text);
                frm.TotalAmount = Convert.ToDecimal(txtTotal.Text);
                frm.CartIsEmpty = dgvProduct.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RedeemPoints = frm.RedeemedPoints;

                    // ✅ Extra safety: prevent applying points if cart is empty
                    if (isCartEmpty && RedeemPoints > 0)
                    {
                        MessageBox.Show("You cannot redeem points without any products in the cart.",
                                        "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        RedeemPoints = 0;
                        return;
                    }

                    if (frm.PointsCoverTotal)
                    {
                        ProcessSaleWithPoints();
                        ResetPOS();
                        return;
                    }
                    else if (RedeemPoints > 0)
                    {
                        decimal remainingTotal = currentTotal - RedeemPoints;
                        if (remainingTotal < 0) remainingTotal = 0;

                        txtRedeemedPoints.Text = RedeemPoints.ToString("N2");
                        txtPoints.Text = RedeemPoints.ToString("N2");
                        txtTotal.Text = remainingTotal.ToString("N2");
                        netAmount = remainingTotal;

                        txtMemberID.Enabled = false;
                    }
                }
            }
        }
        private void ResetPOS()
        {
            // Clear cart
            dgvProduct.Rows.Clear();

            // Reset all totals
            txtSubTotal.Text = "0.00";
            txtDiscount.Text = "0.00";
            txtVatableSales.Text = "0.00";
            txtVatAmount.Text = "0.00";
            txtVatExempt.Text = "0.00";
            txtTotal.Text = "0.00";
            txtRedeemedPoints.Text = "0.00";
            txtQuantity.Text = "";

            // Reset member info
            txtMemberID.Clear();
            txtMemberID.Enabled = true; // ✅ re-enable for new transaction

            MemberId = null;

            netAmount = 0;
        }



        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            try
            {
                using (Register regForm = new Register())
                {
                    var result = regForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // dito na lang optional refresh kung kailangan
                        // LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Register form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtTransactionHistory_Click(object sender, EventArgs e)
        {
            using (TransactionHistory th = new TransactionHistory(ConnectionModule.Session.FullName))
            {
                th.ShowDialog();
            }

        }

        private void POS_Load(object sender, EventArgs e)
        {
            dgvProduct.ClearSelection();
            dgvProduct.CurrentCell = null;

            txtBarcode.KeyPress += DigitsOnly_KeyPress;
            txtDiscount.KeyPress += DigitsOnly_KeyPress;
            txtPoints.KeyPress += DigitsOnly_KeyPress;
            txtQuantity.KeyPress += DigitsOnly_KeyPress;
            txtSubTotal.KeyPress += DigitsOnly_KeyPress;
            txtTotal.KeyPress += DigitsOnly_KeyPress;
            txtVatableSales.KeyPress += DigitsOnly_KeyPress;
            txtVatAmount.KeyPress += DigitsOnly_KeyPress;
            txtVatExempt.KeyPress += DigitsOnly_KeyPress;
            txtRedeemedPoints.KeyPress += DigitsOnly_KeyPress;




            // Address → letters + numbers + space + . , - allowed (pero bawal paste pa rin)
            //  txtAddress.KeyPress += BlockAddressCharacters_KeyPress;

            // Bawal mag-right click copy/paste
            txtBarcode.ContextMenu = new ContextMenu();
            txtDiscount.ContextMenu = new ContextMenu();
            txtPoints.ContextMenu = new ContextMenu();
            txtQuantity.ContextMenu = new ContextMenu();
            txtSubTotal.ContextMenu = new ContextMenu();
            txtTotal.ContextMenu = new ContextMenu();
            txtVatableSales.ContextMenu = new ContextMenu();
            txtVatAmount.ContextMenu = new ContextMenu();
            txtVatExempt.ContextMenu = new ContextMenu();
            txtRedeemedPoints.ContextMenu = new ContextMenu();


            // Bawal Ctrl+V / Ctrl+C / Ctrl+X / Shift+Insert / Ctrl+Insert
            txtBarcode.KeyDown += BlockCopyPaste_KeyDown;
            txtDiscount.KeyDown += BlockCopyPaste_KeyDown;
            txtPoints.KeyDown += BlockCopyPaste_KeyDown;
            txtQuantity.KeyDown += BlockCopyPaste_KeyDown;
            txtSubTotal.KeyDown += BlockCopyPaste_KeyDown;
            txtTotal.KeyDown += BlockCopyPaste_KeyDown;
            txtVatableSales.KeyDown += BlockCopyPaste_KeyDown;
            txtVatAmount.KeyDown += BlockCopyPaste_KeyDown;
            txtVatExempt.KeyDown += BlockCopyPaste_KeyDown;
            txtRedeemedPoints.KeyDown += BlockCopyPaste_KeyDown;



            txtBarcode.KeyPress += BlockMultipleSpaces_KeyPress;
            txtDiscount.KeyPress += BlockMultipleSpaces_KeyPress;
            txtPoints.KeyPress += BlockMultipleSpaces_KeyPress;
            txtSubTotal.KeyPress += BlockMultipleSpaces_KeyPress;
            txtTotal.KeyPress += BlockMultipleSpaces_KeyPress;
            txtVatableSales.KeyPress += BlockMultipleSpaces_KeyPress;
            txtVatAmount.KeyPress += BlockMultipleSpaces_KeyPress;
            txtVatExempt.KeyPress += BlockMultipleSpaces_KeyPress;
            txtQuantity.KeyPress += BlockMultipleSpaces_KeyPress;
            txtRedeemedPoints.KeyPress += BlockMultipleSpaces_KeyPress;



            // Disable shortcuts
            txtBarcode.ShortcutsEnabled = false;
            txtDiscount.ShortcutsEnabled = false;
            txtPoints.ShortcutsEnabled = false;
            txtSubTotal.ShortcutsEnabled = false;
            txtTotal.ShortcutsEnabled = false;
            txtVatableSales.ShortcutsEnabled = false;
            txtVatAmount.ShortcutsEnabled = false;
            txtVatExempt.ShortcutsEnabled = false;
            txtQuantity.ShortcutsEnabled = false;
            txtRedeemedPoints.ShortcutsEnabled = false;
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

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvProduct_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProduct.Columns[e.ColumnIndex].Name == "PriceColumn" ||
       dgvProduct.Columns[e.ColumnIndex].Name == "TotalPriceColumn")
            {
                if (e.Value != null)
                {
                    decimal val = Convert.ToDecimal(e.Value);
                    e.Value = val.ToString("N2"); // format lang sa display
                    e.FormattingApplied = true;
                }
            }
        }

        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btnSave.PerformClick(); // 👉 simulate click sa btnSave
            }
        }

        private void guna2Button5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                guna2Button5.PerformClick(); // 🟢 simulate button click
                e.Handled = true; // optional, para di ma-trigger ulit ng ibang control
            }
        }

        private void btnDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                btnDiscount.PerformClick(); // 🟢 simulate button click
                e.Handled = true; // optional, para di ma-trigger ulit ng ibang control
            }
        }

        private void guna2Button4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                guna2Button4.PerformClick(); // 🟢 simulate button click
                e.Handled = true; // optional, para di ma-trigger ulit ng ibang control
            }
        }

        private void guna2Button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                guna2Button2.PerformClick(); // 🟢 simulate button click
                e.Handled = true; // optional, para di ma-trigger ulit ng ibang control
            }
        }

        private void txtEndShift_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6)
            {
                txtEndShift.PerformClick(); // 🟢 simulate button click
                e.Handled = true; // optional, para di ma-trigger ulit ng ibang control
            }
        }

        private void txtTransactionHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
            {
                txtTransactionHistory.PerformClick(); // 🟢 simulate button click
                e.Handled = true; // optional, para di ma-trigger ulit ng ibang control
            }
        }

        private void PrintReceiptPoints(long saleId, decimal redeemedPoints, decimal earnedPoints)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (sender, e) =>
            {
                int paperWidth = 180; // 58mm receipt
                int marginLeft = 5;
                int startY = 5;
                int lineHeight = 20;

                Font fontTitle = new Font("Arial", 8, FontStyle.Bold);
                Font fontBody = new Font("Arial", 7, FontStyle.Regular);
                Font fontBold = new Font("Arial", 7, FontStyle.Bold);
                Brush brush = Brushes.Black;

                // Header
                string storeName = "MANEJA GROCERY STORE";
                string storeAddress = "A22 A Reyes, New Lower Bicutan, Taguig City";
             

                int textWidth = (int)e.Graphics.MeasureString(storeName, fontTitle).Width;
                e.Graphics.DrawString(storeName, fontTitle, brush, (paperWidth - textWidth) / 2, startY);
                startY += lineHeight;

                textWidth = (int)e.Graphics.MeasureString(storeAddress, fontBody).Width;
                e.Graphics.DrawString(storeAddress, fontBody, brush, (paperWidth - textWidth) / 2, startY);
                startY += lineHeight;

                e.Graphics.DrawString(new string('=', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                // Sale Info
                e.Graphics.DrawString("Receipt #: " + saleId, fontBold, brush, marginLeft, startY); startY += lineHeight;
                e.Graphics.DrawString("Cashier: " + ConnectionModule.Session.FullName, fontBody, brush, marginLeft, startY); startY += lineHeight;
                e.Graphics.DrawString("Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), fontBody, brush, marginLeft, startY); startY += lineHeight;
                // 🕒 VALID UNTIL (15 hours from now)
                DateTime validUntil = DateTime.Now.AddHours(15);
                string validText = "Valid Until: " + validUntil.ToString("MM/dd/yyyy hh:mm tt");

                e.Graphics.DrawString(new string('-', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                // Items
                int colQtyX = marginLeft;
                int colItemX = colQtyX + 25;
                int colPriceX = paperWidth - 55;

                e.Graphics.DrawString("QTY", fontBold, brush, colQtyX, startY);
                e.Graphics.DrawString("ITEM", fontBold, brush, colItemX, startY);
                e.Graphics.DrawString("PRICE", fontBold, brush, colPriceX, startY);
                startY += lineHeight;

                e.Graphics.DrawString(new string('-', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    if (row.IsNewRow) continue;

                    string qty = row.Cells["QuantityColumn"].Value.ToString();
                    string item = row.Cells["ProductNameColumn"].Value.ToString();
                    string price = Convert.ToDecimal(row.Cells["TotalPriceColumn"].Value).ToString("N2");

                    e.Graphics.DrawString(qty, fontBody, brush, colQtyX, startY);
                    e.Graphics.DrawString(item, fontBody, brush, colItemX, startY);
                    e.Graphics.DrawString(price, fontBody, brush, colPriceX, startY);
                    startY += lineHeight;
                }

                e.Graphics.DrawString(new string('=', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                // Totals
                e.Graphics.DrawString("Total:", fontBold, brush, marginLeft, startY);
                e.Graphics.DrawString(redeemedPoints.ToString("N2"), fontBody, brush, colPriceX, startY);
                startY += lineHeight * 2;

                // Points Info
                e.Graphics.DrawString("Points Redeemed: " + redeemedPoints.ToString("N2"), fontBody, brush, marginLeft, startY);
                startY += lineHeight;
                e.Graphics.DrawString("Points Earned: " + earnedPoints.ToString("N2"), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                e.Graphics.DrawString(new string('-', 28), fontBody, brush, marginLeft, startY);

                startY += lineHeight;
               


                e.Graphics.DrawString(new string('-', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                // Footer
                string footer = "** THANK YOU FOR SHOPPING! **";
                textWidth = (int)e.Graphics.MeasureString(footer, fontBold).Width;
                e.Graphics.DrawString(footer, fontBold, brush, (paperWidth - textWidth) / 2, startY);
            };

            printDoc.Print();
        }




        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one item to void.",
                                "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to void the selected item(s)?",
                                "Confirm Void", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                {
                    dgvProduct.Rows.Remove(row);
                }

                // Refresh totals after removing
                ComputeTotals();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // 🔹 Optional: kung gusto mo palaging i-reset kahit walang items
            if (dgvProduct.Rows.Count == 0 && string.IsNullOrWhiteSpace(txtMemberID.Text))
            {
                MessageBox.Show("Nothing to cancel.", "Cancel Transaction",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to cancel the entire transaction?",
                                "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // Clear cart (kung may laman)
            dgvProduct.Rows.Clear();

            // Reset totals
            txtSubTotal.Text = "0.00";
            txtDiscount.Text = "0.00";
            txtPoints.Text = "";
            txtVatableSales.Text = "0.00";
            txtVatAmount.Text = "0.00";
            txtVatExempt.Text = "0.00";
            txtTotal.Text = "0.00";

            // Reset member info & points
            txtMemberID.Text = "";
            txtRedeemedPoints.Text = "0";

            ResetPOS();

            MessageBox.Show("Transaction has been cancelled and POS has been reset.",
                            "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private void DrawTwoColumnText(Graphics g, string leftText, string rightText, Font font, Brush brush, int left, int right, int y)
        {
            g.DrawString(leftText, font, brush, left, y);
            SizeF size = g.MeasureString(rightText, font);
            g.DrawString(rightText, font, brush, right - size.Width, y);
        }


        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (printMode == "shift_summary")
            {
                GetSalesSummary();

                int paperWidth = 180; // 58mm receipt
                int marginLeft = 5;
                int marginRight = paperWidth - 5;
                int y = 10;
                int lineHeight = 18;

                Font fontHeader = new Font("Consolas", 12, FontStyle.Bold);
                Font fontRegular = new Font("Consolas", 9);
                Font fontBold = new Font("Arial", 8, FontStyle.Bold);
                Brush brush = Brushes.Black;

                // === STORE NAME ===
                string storeName = "MANEJA GROCERY STORE";
                int textWidth = (int)e.Graphics.MeasureString(storeName, fontBold).Width;
                e.Graphics.DrawString(storeName, fontBold, brush, (paperWidth - textWidth) / 2, y);
                y += 25;

                // === HEADER ===
                string headerTitle = "★ SALES SUMMARY ★";
                textWidth = (int)e.Graphics.MeasureString(headerTitle, fontHeader).Width;
                e.Graphics.DrawString(headerTitle, fontHeader, brush, (paperWidth - textWidth) / 2, y);
                y += 30;

                e.Graphics.DrawString("Cashier: " + ConnectionModule.Session.FullName, fontRegular, brush, marginLeft, y); y += lineHeight;
                e.Graphics.DrawString("Date: " + DateTime.Now.ToString("MMMM dd, yyyy"), fontRegular, brush, marginLeft, y); y += lineHeight;
                e.Graphics.DrawString("Shift Start: " + ConnectionModule.Session.ShiftStart.ToString("hh:mm tt"), fontRegular, brush, marginLeft, y); y += lineHeight;
                e.Graphics.DrawString("Shift End:   " + DateTime.Now.ToString("hh:mm tt"), fontRegular, brush, marginLeft, y); y += lineHeight + 5;

                e.Graphics.DrawString("------------------------------", fontRegular, brush, marginLeft, y); y += lineHeight;

                // === SALES TOTALS ===
                e.Graphics.DrawString("=== SALES TOTALS ===", fontRegular, brush, marginLeft, y); y += lineHeight;
                DrawTwoColumnText(e.Graphics, "GROSS SALES:", "₱" + grossSales.ToString("N2"), fontRegular, brush, marginLeft, marginRight, y); y += lineHeight;
                DrawTwoColumnText(e.Graphics, "TOTAL DISCOUNTS:", "₱" + totalDiscounts.ToString("N2"), fontRegular, brush, marginLeft, marginRight, y); y += lineHeight;
                DrawTwoColumnText(e.Graphics, "NET SALES:", "₱" + netSales.ToString("N2"), fontRegular, brush, marginLeft, marginRight, y); y += lineHeight;
                DrawTwoColumnText(e.Graphics, "VOIDED SALES:", "₱" + voidedSales.ToString("N2"), fontRegular, brush, marginLeft, marginRight, y); y += lineHeight + 5;

                // === PAYMENT BREAKDOWN ===
                e.Graphics.DrawString("=== PAYMENT BREAKDOWN ===", fontRegular, brush, marginLeft, y); y += lineHeight;
                DrawTwoColumnText(e.Graphics, "CASH:", "₱" + cashSales.ToString("N2"), fontRegular, brush, marginLeft, marginRight, y); y += lineHeight;
                DrawTwoColumnText(e.Graphics, "GCASH:", "₱" + gcashSales.ToString("N2"), fontRegular, brush, marginLeft, marginRight, y); y += lineHeight;
                DrawTwoColumnText(e.Graphics, "POINTS:", "₱" + pointsSales.ToString("N2"), fontRegular, brush, marginLeft, marginRight, y); y += lineHeight + 5;

              

                e.Graphics.DrawString("------------------------------", fontRegular, brush, marginLeft, y); y += lineHeight;
                string thanksMessage = "** THANK YOU FOR YOUR HARD WORK! **";
                textWidth = (int)e.Graphics.MeasureString(thanksMessage, fontBold).Width;
                e.Graphics.DrawString(thanksMessage, fontBold, brush, (paperWidth - textWidth) / 2, y);
            }
        }


        private void GetSalesSummary()
        {
            string cashier = ConnectionModule.Session.FullName;
            DateTime shiftStart = ConnectionModule.Session.ShiftStart;
            DateTime shiftEnd = DateTime.Now;

            // Reset totals
            cashSales = 0;
            gcashSales = 0;
            pointsSales = 0;
            voidedSales = 0;
            grossSales = 0;
            totalDiscounts = 0;
            netSales = 0;
            seniorDiscount = 0;
            pwdDiscount = 0;

            using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
            {
                con.Open();

                string query = @"


SELECT
    -- 🧾 Gross sales (lahat ng sales kahit may void)
    IFNULL(SUM(s.TotalAmount),0) AS GrossSales,

    -- 🎟️ Total discounts (exclude voided sales)
    IFNULL(SUM(CASE WHEN s.IsVoided=0 THEN s.DiscountAmount ELSE 0 END),0) AS TotalDiscounts,

    -- 💰 Net sales (non-voided minus partial void)
    IFNULL(SUM(
        CASE WHEN s.IsVoided=0 
        THEN s.NetAmount - IFNULL((
            SELECT SUM(sd.VoidedAmount)
            FROM salesdetails sd
            WHERE sd.SaleID = s.SaleID
        ),0)
        ELSE 0 END
    ),0) AS NetSales,

  -- 🚫 Voided sales (full + partial)
(
    -- full voided transactions
    IFNULL(SUM(CASE WHEN s.IsVoided=1 THEN s.TotalAmount ELSE 0 END),0)
) +
(
    -- partial voided items
    IFNULL(SUM(
        CASE WHEN s.IsVoided=0 THEN (
            SELECT IFNULL(SUM(sd.VoidedAmount),0)
            FROM salesdetails sd
            WHERE sd.SaleID = s.SaleID
        ) ELSE 0 END
    ),0)
) AS VoidedSales,


    -- 💵 Cash (non-voided minus partial void)
    IFNULL(SUM(
        CASE 
            WHEN s.IsVoided=0 AND s.PaymentMethod LIKE '%Cash:%'
            THEN (s.NetAmount - IFNULL((
                SELECT SUM(sd.VoidedAmount)
                FROM salesdetails sd
                WHERE sd.SaleID = s.SaleID
            ),0))
            *
            (
                CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Cash:',-1),';',1)) AS DECIMAL(10,2)) /
                (
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Cash:',-1),';',1)) AS DECIMAL(10,2)) +
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'GCash:',-1),';',1)) AS DECIMAL(10,2)) +
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Points:',-1),';',1)) AS DECIMAL(10,2))
                )
            )
            ELSE 0
        END
    ),0) AS CashSales,

    -- 📱 GCash proportionate (non-voided minus partial void)
    IFNULL(SUM(
        CASE 
            WHEN s.IsVoided=0 AND s.PaymentMethod LIKE '%GCash:%'
            THEN (s.NetAmount - IFNULL((
                SELECT SUM(sd.VoidedAmount)
                FROM salesdetails sd
                WHERE sd.SaleID = s.SaleID
            ),0))
            *
            (
                CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'GCash:',-1),';',1)) AS DECIMAL(10,2)) /
                (
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Cash:',-1),';',1)) AS DECIMAL(10,2)) +
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'GCash:',-1),';',1)) AS DECIMAL(10,2)) +
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Points:',-1),';',1)) AS DECIMAL(10,2))
                )
            )
            ELSE 0
        END
    ),0) AS GCashSales,

    -- 🪙 Points (non-voided)
    IFNULL(SUM(CASE WHEN s.IsVoided=0 AND s.PaymentMethod LIKE '%Points:%' THEN s.NetAmount ELSE 0 END),0) AS PointsSales,

    -- Discounts by type
    IFNULL(SUM(CASE WHEN s.DiscountType='SENIOR' AND s.IsVoided=0 THEN s.DiscountAmount ELSE 0 END),0) AS SeniorDiscount,
    IFNULL(SUM(CASE WHEN s.DiscountType='PWD' AND s.IsVoided=0 THEN s.DiscountAmount ELSE 0 END),0) AS PwdDiscount

FROM sales s
WHERE s.TransactionDate BETWEEN @start AND @end
  AND s.CashierName=@cashier;


";

                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@start", shiftStart);
                    cmd.Parameters.AddWithValue("@end", shiftEnd);
                    cmd.Parameters.AddWithValue("@cashier", cashier);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            grossSales = dr["GrossSales"] != DBNull.Value ? Convert.ToDecimal(dr["GrossSales"]) : 0;
                            totalDiscounts = dr["TotalDiscounts"] != DBNull.Value ? Convert.ToDecimal(dr["TotalDiscounts"]) : 0;
                            netSales = dr["NetSales"] != DBNull.Value ? Convert.ToDecimal(dr["NetSales"]) : 0;
                            voidedSales = dr["VoidedSales"] != DBNull.Value ? Convert.ToDecimal(dr["VoidedSales"]) : 0;

                            cashSales = dr["CashSales"] != DBNull.Value ? Convert.ToDecimal(dr["CashSales"]) : 0;
                            gcashSales = dr["GCashSales"] != DBNull.Value ? Convert.ToDecimal(dr["GCashSales"]) : 0;
                            pointsSales = dr["PointsSales"] != DBNull.Value ? Convert.ToDecimal(dr["PointsSales"]) : 0;

                            seniorDiscount = dr["SeniorDiscount"] != DBNull.Value ? Convert.ToDecimal(dr["SeniorDiscount"]) : 0;
                            pwdDiscount = dr["PwdDiscount"] != DBNull.Value ? Convert.ToDecimal(dr["PwdDiscount"]) : 0;
                        }
                    }
                }
            }
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {

            // ✅ 1. Prevent End Shift if there are items in dgvProduct
            if (dgvProduct.Rows.Count > 0)
            {
                MessageBox.Show("Cannot end shift while there are still items in the product list. Please clear them first.",
                                "End Shift Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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



            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    // 🔐 3. Hash the entered password using SHA256 (your existing method)
                    string hashedPassword = HashPassword(enteredPassword);

                    // 🔹 4. Verify admin credentials
                    string verifyQuery = @"SELECT COUNT(*) FROM Users
                                   WHERE Role='Admin' AND PasswordHash=@PasswordHash AND Status='Active'";
                    using (MySqlCommand verifyCmd = new MySqlCommand(verifyQuery, con))
                    {
                        verifyCmd.Parameters.Add("@PasswordHash", MySqlDbType.VarChar, 64).Value = hashedPassword;
                        bool isAdminValid = Convert.ToInt32(verifyCmd.ExecuteScalar()) > 0;

                        if (!isAdminValid)
                        {
                            MessageBox.Show("Invalid admin password. You cannot end shift.",
                                            "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // ✅ 5. Admin verified – continue with your existing End Shift logic
                    string cashier = ConnectionModule.Session.FullName;
                    DateTime shiftStartTime = ConnectionModule.Session.ShiftStart;
                    DateTime shiftEndTime = DateTime.Now;



                    // 🔹 GET SHIFT SUMMARY (voided sales excluded from totals)
                    string query = @"


SELECT
    -- 🧾 Gross sales (lahat ng sales kahit may void)
    IFNULL(SUM(s.TotalAmount),0) AS GrossSales,

    -- 🎟️ Total discounts (exclude voided sales)
    IFNULL(SUM(CASE WHEN s.IsVoided=0 THEN s.DiscountAmount ELSE 0 END),0) AS TotalDiscounts,

    -- 💰 Net sales (non-voided minus partial void)
    IFNULL(SUM(
        CASE WHEN s.IsVoided=0 
        THEN s.NetAmount - IFNULL((
            SELECT SUM(sd.VoidedAmount)
            FROM salesdetails sd
            WHERE sd.SaleID = s.SaleID
        ),0)
        ELSE 0 END
    ),0) AS NetSales,

  -- 🚫 Voided sales (full + partial)
(
    -- full voided transactions
    IFNULL(SUM(CASE WHEN s.IsVoided=1 THEN s.TotalAmount ELSE 0 END),0)
) +
(
    -- partial voided items
    IFNULL(SUM(
        CASE WHEN s.IsVoided=0 THEN (
            SELECT IFNULL(SUM(sd.VoidedAmount),0)
            FROM salesdetails sd
            WHERE sd.SaleID = s.SaleID
        ) ELSE 0 END
    ),0)
) AS VoidedSales,


    -- 💵 Cash (non-voided minus partial void)
    IFNULL(SUM(
        CASE 
            WHEN s.IsVoided=0 AND s.PaymentMethod LIKE '%Cash:%'
            THEN (s.NetAmount - IFNULL((
                SELECT SUM(sd.VoidedAmount)
                FROM salesdetails sd
                WHERE sd.SaleID = s.SaleID
            ),0))
            *
            (
                CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Cash:',-1),';',1)) AS DECIMAL(10,2)) /
                (
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Cash:',-1),';',1)) AS DECIMAL(10,2)) +
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'GCash:',-1),';',1)) AS DECIMAL(10,2)) +
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Points:',-1),';',1)) AS DECIMAL(10,2))
                )
            )
            ELSE 0
        END
    ),0) AS CashSales,

    -- 📱 GCash proportionate (non-voided minus partial void)
    IFNULL(SUM(
        CASE 
            WHEN s.IsVoided=0 AND s.PaymentMethod LIKE '%GCash:%'
            THEN (s.NetAmount - IFNULL((
                SELECT SUM(sd.VoidedAmount)
                FROM salesdetails sd
                WHERE sd.SaleID = s.SaleID
            ),0))
            *
            (
                CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'GCash:',-1),';',1)) AS DECIMAL(10,2)) /
                (
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Cash:',-1),';',1)) AS DECIMAL(10,2)) +
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'GCash:',-1),';',1)) AS DECIMAL(10,2)) +
                    CAST(TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(s.PaymentMethod,'Points:',-1),';',1)) AS DECIMAL(10,2))
                )
            )
            ELSE 0
        END
    ),0) AS GCashSales,

    -- 🪙 Points (non-voided)
    IFNULL(SUM(CASE WHEN s.IsVoided=0 AND s.PaymentMethod LIKE '%Points:%' THEN s.NetAmount ELSE 0 END),0) AS PointsSales,

    -- Discounts by type
    IFNULL(SUM(CASE WHEN s.DiscountType='SENIOR' AND s.IsVoided=0 THEN s.DiscountAmount ELSE 0 END),0) AS SeniorDiscount,
    IFNULL(SUM(CASE WHEN s.DiscountType='PWD' AND s.IsVoided=0 THEN s.DiscountAmount ELSE 0 END),0) AS PwdDiscount

FROM sales s
WHERE s.TransactionDate BETWEEN @start AND @end
  AND s.CashierName=@cashier;


";

                    decimal grossSales = 0, totalDiscounts = 0, voidedSales = 0, netSales = 0;
                    decimal cashSales = 0, gcashSales = 0, pointsSales = 0;
                    decimal seniorDiscount = 0, pwdDiscount = 0;

                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@start", shiftStartTime);
                        cmd.Parameters.AddWithValue("@end", shiftEndTime);
                        cmd.Parameters.AddWithValue("@cashier", cashier);

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                grossSales = Convert.ToDecimal(dr["GrossSales"]);
                                totalDiscounts = Convert.ToDecimal(dr["TotalDiscounts"]);
                                netSales = Convert.ToDecimal(dr["NetSales"]);
                                voidedSales = Convert.ToDecimal(dr["VoidedSales"]);

                                cashSales = Convert.ToDecimal(dr["CashSales"]);
                                gcashSales = Convert.ToDecimal(dr["GCashSales"]);
                                pointsSales = Convert.ToDecimal(dr["PointsSales"]);

                                seniorDiscount = Convert.ToDecimal(dr["SeniorDiscount"]);
                                pwdDiscount = Convert.ToDecimal(dr["PwdDiscount"]);
                            }
                        }
                    }

                    // 🔹 INSERT INTO shift_logs
                    string insert = @"
INSERT INTO shift_logs
(CashierName, ShiftStart, ShiftEnd, TotalSales, TotalDiscounts, VoidedTotal, NetSales, CashSales, GCashSales, PointsSales, SeniorDiscount, PwdDiscount)
VALUES (@u, @s, @e, @ts, @td, @v, @n, @cash, @gcash, @points, @senior, @pwd)
";

                    using (var cmd = new MySqlCommand(insert, con))
                    {
                        cmd.Parameters.AddWithValue("@u", cashier);
                        cmd.Parameters.AddWithValue("@s", shiftStartTime);
                        cmd.Parameters.AddWithValue("@e", shiftEndTime);
                        cmd.Parameters.AddWithValue("@ts", grossSales);
                        cmd.Parameters.AddWithValue("@td", totalDiscounts);
                        cmd.Parameters.AddWithValue("@v", voidedSales);
                        cmd.Parameters.AddWithValue("@n", netSales);
                        cmd.Parameters.AddWithValue("@cash", cashSales);
                        cmd.Parameters.AddWithValue("@gcash", gcashSales);
                        cmd.Parameters.AddWithValue("@points", pointsSales);
                        cmd.Parameters.AddWithValue("@senior", seniorDiscount);
                        cmd.Parameters.AddWithValue("@pwd", pwdDiscount);
                        cmd.ExecuteNonQuery();
                    }

                    // 🔹 PRINT SHIFT SUMMARY RECEIPT
                    printMode = "shift_summary";
                    printDocument1.Print();

                    MessageBox.Show("Shift ended successfully. Receipt printed.", "End Shift", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Close or hide current form


                    try
                    {
                        ConnectionModule.openCon();

                        // ✅ Update the last login log for this user
                        string updateLogQuery = @"
UPDATE login_logs
SET LogoutTime = NOW(),
    Status = 'Logged Out'
WHERE Username = @username
  AND Status = 'Logged In'
ORDER BY LogID DESC
LIMIT 1";

                        using (MySqlCommand cmd = new MySqlCommand(updateLogQuery, ConnectionModule.con))
                        {
                            cmd.Parameters.AddWithValue("@username", ConnectionModule.Session.Username);
                            cmd.ExecuteNonQuery();
                        }

                        // ✅ Optional: Insert Audit Trail for logout
                      //  ConnectionModule.InsertAuditTrail("Logout", "Users", $"User {ConnectionModule.Session.Username} logged out.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating logout: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        ConnectionModule.closeCon();
                    }

                    // Show Login form
                    Loginform loginForm = new Loginform();
                    loginForm.Show();
                    this.Close();
                    // 🔹 Reset shift start for next login
                    //   ConnectionModule.Session.ShiftStart = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ending shift: " + ex.Message);

            }
        }
    }
}
