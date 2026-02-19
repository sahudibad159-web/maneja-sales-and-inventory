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
using System.Drawing.Printing;

namespace Sales_Inventory
{
    public partial class Payment : Form
    {
        public string DiscountType { get; set; }
        public string DiscountFullName { get; set; }
        public string DiscountIDNumber { get; set; }
        public string MemberName { get; set; } = ""; // ✅ Add this line
        private decimal totalAmount;

        private DataGridView posCart;
        private decimal vatAmount;
        private decimal discountAmount;
        private decimal netAmount;
        public string MemberCode { get; set; } // store the MemberCode from POS

        public int? MemberId { get; set; }
        public decimal RedeemPoints { get; set; }


        public Payment(decimal total, decimal vat, decimal discount, decimal net, DataGridView dgv, decimal redeemPoints, int? memberId)
        {
            InitializeComponent();
            totalAmount = total;
            vatAmount = vat;
            discountAmount = discount;
            netAmount = net;
            posCart = dgv; // 👉 save reference ng cart
            this.RedeemPoints = redeemPoints;
            this.MemberId = memberId;
        }



        private int targetHeight = 0;   // target height ng panelGCash
        private int step = 10;          // bilis ng animation

        private void Payment_Load(object sender, EventArgs e)
        {
            // Add payment options
            cmbPayment.Items.Clear();
            cmbPayment.Items.Add("Cash");
            cmbPayment.Items.Add("GCash");

            // Default selection = Cash
            cmbPayment.SelectedIndex = 0;

            // Setup panels
            panelCash.Visible = true;
            panelGcash.Height = 0;      // collapsed start
            panelGcash.Visible = false;

            //txtGcash.KeyPress += DecimalOnly_KeyPress;
            //txtCash.KeyPress += DecimalOnly_KeyPress;
            //txtChange.KeyPress += DecimalOnly_KeyPress;

            txtReference.KeyPress += DigitsOnly_KeyPress;
           

            txtGcash.ContextMenu = new ContextMenu();
            txtCash.ContextMenu = new ContextMenu();
            txtReference.ContextMenu = new ContextMenu();
            txtFullname.ContextMenu = new ContextMenu();
            txtChange.ContextMenu = new ContextMenu();




            txtFullname.KeyPress += BlockNumbers_KeyPress;



            txtGcash.KeyDown += BlockCopyPaste_KeyDown;
            txtCash.KeyDown += BlockCopyPaste_KeyDown;
            txtFullname.KeyDown += BlockCopyPaste_KeyDown;
            txtReference.KeyDown += BlockCopyPaste_KeyDown;
            txtChange.KeyDown += BlockCopyPaste_KeyDown;



            txtGcash.ShortcutsEnabled = false;
            txtCash.ShortcutsEnabled = false;
            txtChange.ShortcutsEnabled = false;
            txtFullname.ShortcutsEnabled = false;
            txtReference.ShortcutsEnabled = false;
        }
        private void BlockNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kung digit, wag i-allow (silent block)
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void BlockInvalidCharacters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // block lahat ng hindi letter at control
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
        //private void DecimalOnly_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    TextBox tb = sender as TextBox;
        //    if (tb == null)
        //        return;

        //    // Allow control keys
        //    if (char.IsControl(e.KeyChar))
        //        return;

        //    // Block dot as first character
        //    if (e.KeyChar == '.' && tb.SelectionStart == 0)
        //    {
        //        e.Handled = true;
        //        return;
        //    }

        //    string text = tb.Text;
        //    int selectionStart = tb.SelectionStart;
        //    int selectionLength = tb.SelectionLength;

        //    // Simulate next text
        //    if (selectionLength > 0)
        //        text = text.Remove(selectionStart, selectionLength);

        //    text = text.Insert(selectionStart, e.KeyChar.ToString());

        //    // Allow only one decimal point
        //    if (text.Count(c => c == '.') > 1)
        //    {
        //        e.Handled = true;
        //        return;
        //    }

        //    // Max 2 decimal places
        //    int dotIndex = text.IndexOf('.');
        //    if (dotIndex >= 0 && text.Length - dotIndex - 1 > 2)
        //    {
        //        e.Handled = true;
        //        return;
        //    }

        //    // Allow digits and dot only
        //    if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        //    {
        //        e.Handled = true;
        //    }
        //}


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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            if (panelGcash.Height < targetHeight)
            {
                panelGcash.Height += step;
                if (panelGcash.Height >= targetHeight)
                {
                    panelGcash.Height = targetHeight;
                    animationTimer.Stop();
                }
            }
            else if (panelGcash.Height > targetHeight)
            {
                panelGcash.Height -= step;
                if (panelGcash.Height <= targetHeight)
                {
                    panelGcash.Height = targetHeight;
                    if (targetHeight == 0) panelGcash.Visible = false;
                    animationTimer.Stop();
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbPayment_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbPayment.SelectedItem == null) return;

            string selected = cmbPayment.SelectedItem.ToString();

            if (selected == "Cash")
            {
                targetHeight = 0;
                panelGcash.Visible = false;
                panelCash.Visible = true;
                txtGcash.Text = "0.00"; // reset
                txtGcash.Enabled = false;

                panelCash.Top = (this.ClientSize.Height - panelCash.Height) / 2 - 20;
            }
            else if (selected == "GCash")
            {
                targetHeight = 150;
                panelGcash.Visible = true;
                panelCash.Visible = true;
                txtGcash.Enabled = true;

                panelCash.Top = panelGcash.Bottom + 10;
            }


            animationTimer.Start();
        }
        private void RecalculatePayment()
        {
            decimal cash = 0, gcash = 0;

            if (decimal.TryParse(txtCash.Text, out decimal c))
                cash = c;

            if (decimal.TryParse(txtGcash.Text, out decimal g))
                gcash = g;

            decimal totalPayment = cash + gcash;

            // ✅ GAMITIN ANG netAmount hindi totalAmount
            decimal pointsApplied = Math.Min(RedeemPoints, netAmount);
            decimal amountAfterPoints = netAmount - pointsApplied;
            if (amountAfterPoints < 0) amountAfterPoints = 0;

            decimal change = totalPayment - amountAfterPoints;
            if (change < 0) change = 0;

            txtChange.Text = change.ToString("N2");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            RecalculatePayment();
        }

        private void txtGcash_TextChanged(object sender, EventArgs e)
        {
            RecalculatePayment();
        }



        private void PrintReceipt(
            long saleId,
            decimal cash,
            decimal gcash,
            decimal change,
            string discountType,
            string discountName,
            string discountID,
            string memberCode,
            decimal earnedPoints,
            decimal subtotal,      // original total before discount/points
            decimal vat,
            decimal discount,
            decimal net,           // subtotal - discount - points
            decimal redeemedPoints

        )
        {
            string memberName = "";

            if (!string.IsNullOrEmpty(memberCode))
            {
                try
                {
                    ConnectionModule.openCon();
                    using (var cmd = new MySqlCommand("SELECT FullName FROM members WHERE MemberCode=@code LIMIT 1", ConnectionModule.con))
                    {
                        cmd.Parameters.AddWithValue("@code", memberCode.Trim());
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                            memberName = result.ToString();
                    }
                }
                catch { }
                finally { ConnectionModule.closeCon(); }
            }

            PrintDocument printDoc = new PrintDocument();
            PaperSize paperSize = new PaperSize("Custom", 200, 1200);
            printDoc.DefaultPageSettings.PaperSize = paperSize;
            printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            printDoc.PrintPage += (sender, e) =>
            {
                int paperWidth = 180;
                int marginLeft = 5;
                int startY = 5;
                int lineHeight = 18;

                Font fontTitle = new Font("Arial", 8, FontStyle.Bold);
                Font fontBody = new Font("Arial", 7, FontStyle.Regular);
                Font fontBold = new Font("Arial", 7, FontStyle.Bold);
                Brush brush = Brushes.Black;

                // ================= HEADER =================
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

                // ================= SALE INFO =================
                e.Graphics.DrawString("Receipt #: " + saleId, fontBold, brush, marginLeft, startY); startY += lineHeight;
                e.Graphics.DrawString("Cashier: " + ConnectionModule.Session.FullName, fontBody, brush, marginLeft, startY); startY += lineHeight;
                e.Graphics.DrawString("Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), fontBody, brush, marginLeft, startY); startY += lineHeight;

                e.Graphics.DrawString(new string('-', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                // ================= ITEMS =================
                int colQtyX = marginLeft;
                int colItemX = colQtyX + 25;
                int colPriceX = paperWidth - 55;

                e.Graphics.DrawString("QTY", fontBold, brush, colQtyX, startY);
                e.Graphics.DrawString("ITEM", fontBold, brush, colItemX, startY);
                e.Graphics.DrawString("PRICE", fontBold, brush, colPriceX, startY);
                startY += lineHeight;

                e.Graphics.DrawString(new string('-', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                foreach (DataGridViewRow row in posCart.Rows)
                {
                    if (row.IsNewRow) continue;

                    string qty = row.Cells["QuantityColumn"].Value.ToString();
                    string item = row.Cells["ProductNameColumn"].Value.ToString();
                    string price = Convert.ToDecimal(row.Cells["PriceColumn"].Value).ToString("N2");

                    e.Graphics.DrawString(qty, fontBody, brush, colQtyX, startY);
                    e.Graphics.DrawString(item, fontBody, brush, colItemX, startY);
                    e.Graphics.DrawString(price, fontBody, brush, colPriceX, startY);
                    startY += lineHeight;
                }

                e.Graphics.DrawString(new string('=', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                // ================= TOTALS =================
                int rightAlignX = paperWidth - 5; // Inadjust ko para mas sagad sa kanan

                e.Graphics.DrawString("Subtotal:", fontBold, brush, marginLeft, startY);
                string strSubtotal = subtotal.ToString("N2");
                e.Graphics.DrawString(strSubtotal, fontBody, brush, rightAlignX - e.Graphics.MeasureString(strSubtotal, fontBody).Width, startY);
                startY += lineHeight;

                // --- DITO ANG DAGDAG PARA SA VAT-EXEMPT ---
                if (discount > 0)
                {
                    decimal vatExemptSales = subtotal / 1.12m; // Tinatanggal ang 12% VAT
                    e.Graphics.DrawString("VAT-Exempt Sales:", fontBold, brush, marginLeft, startY);
                    string strVatEx = vatExemptSales.ToString("N2");
                    e.Graphics.DrawString(strVatEx, fontBody, brush, rightAlignX - e.Graphics.MeasureString(strVatEx, fontBody).Width, startY);
                    startY += lineHeight;
                }

                e.Graphics.DrawString("VAT (12%):", fontBold, brush, marginLeft, startY);
                string strVat = vat.ToString("N2");
                e.Graphics.DrawString(strVat, fontBody, brush, rightAlignX - e.Graphics.MeasureString(strVat, fontBody).Width, startY);
                startY += lineHeight;

                e.Graphics.DrawString("Discount:", fontBold, brush, marginLeft, startY);
                string strDisc = discount.ToString("N2");
                e.Graphics.DrawString(strDisc, fontBody, brush, rightAlignX - e.Graphics.MeasureString(strDisc, fontBody).Width, startY);
                startY += lineHeight;
                // ------------------------------------------

                if (redeemedPoints > 0)
                {
                    e.Graphics.DrawString("Points Used:", fontBold, brush, marginLeft, startY);
                    string strPoints = redeemedPoints.ToString("N2");
                    e.Graphics.DrawString(strPoints, fontBody, brush, rightAlignX - e.Graphics.MeasureString(strPoints, fontBody).Width, startY);
                    startY += lineHeight;
                }

                e.Graphics.DrawString("TOTAL :", fontBold, brush, marginLeft, startY);
                string strNet = net.ToString("N2");
                e.Graphics.DrawString(strNet, fontBold, brush, rightAlignX - e.Graphics.MeasureString(strNet, fontBold).Width, startY);
                startY += lineHeight * 2;

                // ================= PAYMENTS =================
                e.Graphics.DrawString("Cash:", fontBold, brush, marginLeft, startY);
                e.Graphics.DrawString(cash.ToString("N2"), fontBody, brush, rightAlignX, startY);
                startY += lineHeight;

                e.Graphics.DrawString("GCash:", fontBold, brush, marginLeft, startY);
                e.Graphics.DrawString(gcash.ToString("N2"), fontBody, brush, rightAlignX, startY);
                startY += lineHeight;

                e.Graphics.DrawString("Change:", fontBold, brush, marginLeft, startY);
                e.Graphics.DrawString(change.ToString("N2"), fontBody, brush, rightAlignX, startY);
                startY += lineHeight * 2;

                // ================= DISCOUNT INFO =================
                if (!string.IsNullOrEmpty(discountType))
                {
                    e.Graphics.DrawString("Discount Type: " + discountType, fontBody, brush, marginLeft, startY);
                    startY += lineHeight;

                    e.Graphics.DrawString("Name: " + discountName, fontBody, brush, marginLeft, startY);
                    startY += lineHeight;

                    e.Graphics.DrawString("ID: " + discountID, fontBody, brush, marginLeft, startY);
                    startY += lineHeight;
                }

                // ================= MEMBER INFO =================
                if (!string.IsNullOrEmpty(memberCode))
                {
                    e.Graphics.DrawString("Member Code: " + memberCode, fontBody, brush, marginLeft, startY);
                    startY += lineHeight;

                    if (!string.IsNullOrEmpty(memberName))
                    {
                        e.Graphics.DrawString("Member Name: " + memberName, fontBody, brush, marginLeft, startY);
                        startY += lineHeight;
                    }

                    e.Graphics.DrawString("Earned Points: " + earnedPoints.ToString("N2"), fontBody, brush, marginLeft, startY);
                    startY += lineHeight;
                }

                e.Graphics.DrawString(new string('-', 28), fontBody, brush, marginLeft, startY);
                startY += lineHeight;

                // ================= FOOTER =================
                string footer = "** THANK YOU FOR SHOPPING! **";
                textWidth = (int)e.Graphics.MeasureString(footer, fontBold).Width;
                e.Graphics.DrawString(footer, fontBold, brush, (paperWidth - textWidth) / 2, startY);
            };

            printDoc.Print();
        }



        private void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                decimal cash = string.IsNullOrWhiteSpace(txtCash.Text) ? 0 : Convert.ToDecimal(txtCash.Text);
                decimal gcash = string.IsNullOrWhiteSpace(txtGcash.Text) ? 0 : Convert.ToDecimal(txtGcash.Text);

                // Compute total after discount and points
                decimal pointsApplied = Math.Min(RedeemPoints, netAmount);
                // Hindi na tayo magbabawas ulit. 
                // Ang 'netAmount' (35.71) ay discounted na, kaya ito na ang gagamitin natin.
                decimal amountAfterDiscount = netAmount;
                if (amountAfterDiscount < 0) amountAfterDiscount = 0;

                decimal amountAfterPoints = amountAfterDiscount - pointsApplied;
                if (amountAfterPoints < 0) amountAfterPoints = 0;

                // Calculate total payment
                decimal totalPaid = cash + gcash;

                // ✅ Calculate change for resibo
                decimal change = totalPaid - amountAfterPoints;
                if (change < 0) change = 0; // if underpaid

                // ✅ Determine actual cash/GCash used for DB (limit to total due)
                decimal actualCashUsed = cash;
                decimal actualGCashUsed = gcash;

                if (totalPaid > amountAfterPoints)
                {
                    // Proportionally reduce for DB purposes only
                    decimal ratio = amountAfterPoints / totalPaid;
                    actualCashUsed = Math.Round(cash * ratio, 2);
                    actualGCashUsed = amountAfterPoints - actualCashUsed;
                }

                // Validate payment
                if (totalPaid < amountAfterPoints)
                {
                    MessageBox.Show("Insufficient payment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            // 2️⃣ Earned points only from cash/GCash portion
                            // Include cash + GCash + pointsApplied for earned points
                            decimal earnedPoints = 0;
                            if (!string.IsNullOrEmpty(MemberCode))
                            {
                                decimal totalPaymentForPoints = cash + gcash; // exclude redeemed points
                                earnedPoints = Math.Round(totalPaymentForPoints * 0.01m, 2); // 1% of actual cash paid

                            }

                            actualCashUsed = cash;
                            actualGCashUsed = gcash;

                            // 3️⃣ Payment method string
                            string paymentMethod = "";
                            if (actualCashUsed > 0) paymentMethod += $"Cash:{actualCashUsed};";   // ✅ FIXED — use actualCashUsed
                            if (actualGCashUsed > 0) paymentMethod += $"GCash:{actualGCashUsed};";
                            if (pointsApplied > 0) paymentMethod += $"Points:{pointsApplied};";
                            if (change > 0) paymentMethod += $"Change:{change};"; // optional, for record
                            if (string.IsNullOrEmpty(paymentMethod)) paymentMethod = "Points";


                            // 4️⃣ Convert pointsApplied to int for DB
                            int redeemPointsInt = (int)Math.Floor(pointsApplied); // ✅ Fixed

                            // 5️⃣ Insert sale
                            string insertSales = @"
INSERT INTO sales
(TransactionDate, MemberID, TotalAmount, VatAmount, DiscountAmount, NetAmount, RedeemedPoints, EarnedPoints, PaymentMethod, CashierName, isVoided, DiscountType)
VALUES (@date, @memberId, @total, @vat, @discount, @net, @redeem, @earned, @payment, @cashier, 0, @discountType)";
                            using (var cmd = new MySqlCommand(insertSales, con, trans))
                            {
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@memberId", (object)MemberId ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@total", totalAmount);
                                cmd.Parameters.AddWithValue("@vat", vatAmount);
                                cmd.Parameters.AddWithValue("@discount", discountAmount);
                                cmd.Parameters.AddWithValue("@net", amountAfterPoints);
                                cmd.Parameters.AddWithValue("@redeem", redeemPointsInt);
                                cmd.Parameters.AddWithValue("@earned", earnedPoints);
                                cmd.Parameters.AddWithValue("@payment", paymentMethod);
                                cmd.Parameters.AddWithValue("@cashier", ConnectionModule.Session.FullName);
                                cmd.Parameters.AddWithValue("@discountType", DiscountType ?? "");
                                cmd.ExecuteNonQuery();
                            }

                            long saleId = Convert.ToInt64(new MySqlCommand("SELECT LAST_INSERT_ID();", con, trans).ExecuteScalar());

                            // 6️⃣ Process cart items (stock deduction)
                            foreach (DataGridViewRow row in posCart.Rows)
                            {
                                if (row.IsNewRow) continue;

                                int productId = Convert.ToInt32(row.Cells["ProductIDColumn"].Value);
                                int qtyToDeduct = Convert.ToInt32(row.Cells["QuantityColumn"].Value);
                                decimal price = Convert.ToDecimal(row.Cells["PriceColumn"].Value);
                                decimal subtotal = Convert.ToDecimal(row.Cells["TotalPriceColumn"].Value);

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
SELECT dd.idDetail, dd.QtyDelivered, dd.ExpirationDate, d.DeliveryDate
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

                                    // Update inventory
                                    string updateInventory = "UPDATE inventory SET QuantityInStock = QuantityInStock - @qty WHERE ProductID = @productId AND QuantityInStock >= @qty;";
                                    using (var cmdInv = new MySqlCommand(updateInventory, con, trans))
                                    {
                                        cmdInv.Parameters.AddWithValue("@qty", deduct);
                                        cmdInv.Parameters.AddWithValue("@productId", productId);

                                        int rowsAffected = cmdInv.ExecuteNonQuery();

                                        if (rowsAffected == 0)
                                        {
                                            throw new Exception("Not enough stock available.");
                                        }
                                    }

                                    string insertMovement = @"
INSERT INTO inventory_movements
(idDetail, ProductID, MovementType, Quantity, ExpirationDate, MovementDate, Source, ReferenceID, Remarks)
VALUES (@idDetail, @productId, 'OUT', @qty, @expDate, NOW(), 'POS Sale', @referenceID, CONCAT('Sold batch exp: ', @expDate))";
                                    using (var cmdMove = new MySqlCommand(insertMovement, con, trans))
                                    {
                                        cmdMove.Parameters.AddWithValue("@idDetail", batch.idDetail);
                                        cmdMove.Parameters.AddWithValue("@productId", productId);
                                        cmdMove.Parameters.AddWithValue("@qty", deduct);
                                        cmdMove.Parameters.AddWithValue("@referenceID", saleId);
                                        cmdMove.Parameters.AddWithValue("@expDate", batch.expDate);
                                        cmdMove.ExecuteNonQuery();
                                    }
                                }

                                if (remainingQty > 0)
                                    throw new Exception($"Not enough stock for ProductID {productId}");
                            }

                            // Resolve MemberId kung null pero may MemberCodedmi
                            if (!MemberId.HasValue && !string.IsNullOrEmpty(MemberCode))
                            {
                                using (var cmd = new MySqlCommand("SELECT MemberID FROM members WHERE MemberCode=@code LIMIT 1", con, trans))
                                {
                                    cmd.Parameters.AddWithValue("@code", MemberCode.Trim());
                                    var result = cmd.ExecuteScalar();
                                    if (result != null)
                                        MemberId = Convert.ToInt32(result);
                                }
                            }

                            // Debug: makita kung may MemberId
                            if (!MemberId.HasValue)
                            {
                               // MessageBox.Show("Member not found. Points will not be applied!");
                            }


                            // 7️⃣ Update member points
                            if (MemberId.HasValue)
                            {
                                string updatePoints = @"
    UPDATE members 
    SET Points = Points - @redeem + @earned 
    WHERE MemberID = @memberId";

                                using (var cmdPoints = new MySqlCommand(updatePoints, con, trans))
                                {
                                    cmdPoints.Parameters.AddWithValue("@redeem", redeemPointsInt);
                                    cmdPoints.Parameters.AddWithValue("@earned", earnedPoints);
                                    cmdPoints.Parameters.AddWithValue("@memberId", MemberId.Value);
                                    cmdPoints.ExecuteNonQuery();
                                }


                                string insertHistory = @"
INSERT INTO points_history (MemberID, SaleID, PointsEarned, PointsRedeemed)
VALUES (@memberId, @saleId, @earned, @redeem)";
                                using (var cmdHist = new MySqlCommand(insertHistory, con, trans))
                                {
                                    cmdHist.Parameters.AddWithValue("@memberId", MemberId.Value);
                                    cmdHist.Parameters.AddWithValue("@saleId", saleId);
                                    cmdHist.Parameters.AddWithValue("@earned", earnedPoints);
                                    cmdHist.Parameters.AddWithValue("@redeem", redeemPointsInt);
                                    cmdHist.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();

                            MessageBox.Show("Payment successful!\nChange: " + change.ToString("N2"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Save info to tag for main form
                            this.Tag = new
                            {
                                Cash = cash,
                                GCash = gcash,
                                Change = change,
                                RedeemedPoints = redeemPointsInt,
                                PaymentMethod = paymentMethod
                            };

                            PrintReceipt(
                                 saleId,
                                 actualCashUsed,
                                 actualGCashUsed,
                                 change,
                                 DiscountType,
                                 DiscountFullName,
                                 DiscountIDNumber,
                                 MemberCode,
                                 earnedPoints,
                                 totalAmount,
                                 vatAmount,
                                 discountAmount,
                                 amountAfterPoints,
                                 pointsApplied
                             );

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            try { trans.Rollback(); } catch { }
                            MessageBox.Show("Error saving transaction: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void panelCash_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
