using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharpFont = iTextSharp.text.Font;

namespace Sales_Inventory
{
    public partial class UC_DeliveryDetails : UserControl
    {
        public UC_DeliveryDetails()
        {
            InitializeComponent();
          StyleDataGridView(dgvDeliveryDetails);
            StyleDataGridView(dgvDeliveries);
            LoadDeliveries();
            DateTime fromDate = dtpFrom.Value.Date; // Start of selected day
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // End of selected day (23:59:59)

            LoadDeliveries(fromDate, toDate); // Load main deliveries

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
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 35;

            // Row style
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowsDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
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
        private void LoadDeliveries(DateTime? selectedDate = null)
        {
            string query = @"SELECT d.idDelivery, 
                            d.DeliveryDate, 
                            d.DeliveryReceipt, 
                            s.SupplierName, 
                            d.DeliveryStatus,
                            d.ReceivedBy
                     FROM Delivery d
                     INNER JOIN Supplier s ON d.SupplierID = s.SupplierID";

            // Add WHERE clause if a date is provided
            if (selectedDate.HasValue)
            {
                query += " WHERE DATE(d.DeliveryDate) = @DeliveryDate";
            }

            query += " ORDER BY d.DeliveryDate DESC";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);

                if (selectedDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@DeliveryDate", selectedDate.Value.Date);
                }

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDeliveries.DataSource = dt;

                // Itago ang ID column para di makita ng user
                if (dgvDeliveries.Columns.Contains("idDelivery"))
                    dgvDeliveries.Columns["idDelivery"].Visible = false;
            }
        }



        private void LoadDeliveryDetails(int deliveryID)
        {
            string query = @"
        SELECT 
            d.idDetail, 
            p.ProductName, 
            d.Description,
            d.Remarks,                   -- 🟢 Added this line
            d.QtyOrdered, 
            d.QtyDelivered, 
            d.CostPerItem,
            (d.QtyDelivered * d.CostPerItem) AS TotalCost,
            d.ExpirationDate
        FROM Delivery_Details d
        INNER JOIN Product p ON d.ProductID = p.ProductID
        WHERE d.idDelivery = @idDelivery";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idDelivery", deliveryID);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDeliveryDetails.DataSource = dt;

                if (dgvDeliveryDetails.Columns.Contains("idDetail"))
                    dgvDeliveryDetails.Columns["idDetail"].Visible = false;
            }
        }

        // 🔹 1. Load lahat ng deliveries between date range
        private void LoadDeliveries(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ConnectionModule.openCon();

                string query = @"
            SELECT 
                d.idDelivery, 
                d.DeliveryDate, 
                d.DeliveryReceipt, 
                s.SupplierName, 
                d.DeliveryStatus,
                d.ReceivedBy
            FROM Delivery d
            INNER JOIN Supplier s ON d.SupplierID = s.SupplierID
            WHERE d.DeliveryDate BETWEEN @From AND @To
            ORDER BY d.DeliveryDate DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@From", fromDate);
                    cmd.Parameters.AddWithValue("@To", toDate);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dgvDeliveries.DataSource = dt;
                    }
                }

                if (dgvDeliveries.Columns.Contains("idDelivery"))
                    dgvDeliveries.Columns["idDelivery"].Visible = false;

                StyleDataGridView(dgvDeliveries);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading deliveries: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }



        private void UC_DeliveryDetails_Load(object sender, EventArgs e)
        {
            dgvDeliveries.ClearSelection();
            dgvDeliveries.CurrentCell = null;
            dgvDeliveryDetails.ClearSelection();
            dgvDeliveryDetails.CurrentCell = null;
        }

        private void dgvDeliveryDetails_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
         
            
        }
        // 🔹 Track the last clicked row index (para sa toggle)
        private int lastSelectedRowIndex = -1;

        private void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Check if user clicked the same row again
                if (e.RowIndex == lastSelectedRowIndex)
                {
                    // Unselect everything
                    dgvDeliveries.ClearSelection();
                    dgvDeliveryDetails.DataSource = null; // optional: clear details table
                    lastSelectedRowIndex = -1; // reset tracker
                    return;
                }

                // Otherwise, select new row normally
                dgvDeliveries.ClearSelection();
                dgvDeliveries.Rows[e.RowIndex].Selected = true;

                int deliveryID = Convert.ToInt32(dgvDeliveries.Rows[e.RowIndex].Cells["idDelivery"].Value);
                LoadDeliveryDetails(deliveryID);

                // Save the currently selected row
                lastSelectedRowIndex = e.RowIndex;
            }
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
           // LoadDeliveries(dtDate.Value);
        }

        private void txtSearchDelivery_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchDelivery.Text.Trim();
            DateTime fromDate = dtpFrom.Value.Date;  // Start date
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // End date (23:59:59)

            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
                {
                    con.Open();

                    string query = @"
                SELECT 
                    d.idDelivery, 
                    d.DeliveryDate, 
                    d.DeliveryReceipt, 
                    s.SupplierName, 
                    d.DeliveryStatus,
                    d.ReceivedBy
                FROM Delivery d
                INNER JOIN Supplier s ON d.SupplierID = s.SupplierID
                WHERE (d.DeliveryReceipt LIKE @search 
                       OR s.SupplierName LIKE @search 
                       OR d.DeliveryStatus LIKE @search 
                       OR d.ReceivedBy LIKE @search)
                  AND (d.DeliveryDate BETWEEN @From AND @To)
                ORDER BY d.DeliveryDate DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                        cmd.Parameters.AddWithValue("@From", fromDate);
                        cmd.Parameters.AddWithValue("@To", toDate);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvDeliveries.DataSource = dt;
                    }

                    // Hide ID column
                    if (dgvDeliveries.Columns.Contains("idDelivery"))
                        dgvDeliveries.Columns["idDelivery"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching deliveries: " + ex.Message);
            }
        }


        private void btnPrintPDF_Click(object sender, EventArgs e)
        {
            if (dgvDeliveries.CurrentRow == null)
            {
                MessageBox.Show("Please select a delivery to print.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int deliveryID = Convert.ToInt32(dgvDeliveries.CurrentRow.Cells["idDelivery"].Value);
                string supplier = dgvDeliveries.CurrentRow.Cells["SupplierName"].Value.ToString();
                string receiptNo = dgvDeliveries.CurrentRow.Cells["DeliveryReceipt"].Value.ToString();
                string deliveryDate = Convert.ToDateTime(dgvDeliveries.CurrentRow.Cells["DeliveryDate"].Value).ToString("MMMM dd, yyyy");
                string status = dgvDeliveries.CurrentRow.Cells["DeliveryStatus"].Value.ToString();
                string receivedBy = dgvDeliveries.CurrentRow.Cells["ReceivedBy"].Value.ToString();

                // File path
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DeliveryReports");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, $"Delivery_{receiptNo}.pdf");

                // Create PDF
                Document doc = new Document(PageSize.A4, 30, 30, 40, 40);
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // Title
                var titleFont = FontFactory.GetFont("Segoe UI", 16, iTextSharpFont.BOLD);
                var headerFont = FontFactory.GetFont("Segoe UI", 11, iTextSharpFont.BOLD);
                var normalFont = FontFactory.GetFont("Segoe UI", 10, iTextSharpFont.NORMAL);

                Paragraph title = new Paragraph("Delivery Report", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                doc.Add(new Paragraph("\n"));

                // Info Section
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.AddCell(new Phrase("Delivery Receipt:", headerFont));
                infoTable.AddCell(new Phrase(receiptNo, normalFont));
                infoTable.AddCell(new Phrase("Supplier Name:", headerFont));
                infoTable.AddCell(new Phrase(supplier, normalFont));
                infoTable.AddCell(new Phrase("Delivery Date:", headerFont));
                infoTable.AddCell(new Phrase(deliveryDate, normalFont));
                infoTable.AddCell(new Phrase("Status:", headerFont));
                infoTable.AddCell(new Phrase(status, normalFont));
                infoTable.AddCell(new Phrase("Received By:", headerFont));
                infoTable.AddCell(new Phrase(receivedBy, normalFont));
                doc.Add(infoTable);
                doc.Add(new Paragraph("\n"));

                // Delivery Details Table
                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 3f, 3f, 2f, 2f, 2f, 3f });

                string[] headers = { "Product Name", "Description", "Remarks", "Qty Ordered", "Qty Delivered", "Cost per Item", "Total Cost" };
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = new BaseColor(230, 230, 230),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                // Query delivery details
                using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
                {
                    con.Open();
                    string query = @"
                SELECT p.ProductName, p.Description, d.Remarks, d.QtyOrdered, d.QtyDelivered, 
                       d.CostPerItem, (d.QtyDelivered * d.CostPerItem) AS TotalCost
                FROM Delivery_Details d
                INNER JOIN Product p ON d.ProductID = p.ProductID
                WHERE d.idDelivery = @idDelivery";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@idDelivery", deliveryID);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        table.AddCell(new Phrase(dr["ProductName"].ToString(), normalFont));
                        table.AddCell(new Phrase(dr["Description"].ToString(), normalFont));
                        table.AddCell(new Phrase(dr["Remarks"].ToString(), normalFont));
                        table.AddCell(new Phrase(dr["QtyOrdered"].ToString(), normalFont));
                        table.AddCell(new Phrase(dr["QtyDelivered"].ToString(), normalFont));
                        table.AddCell(new Phrase(Convert.ToDecimal(dr["CostPerItem"]).ToString("C2"), normalFont));
                        table.AddCell(new Phrase(Convert.ToDecimal(dr["TotalCost"]).ToString("C2"), normalFont));
                    }
                }

                doc.Add(table);
                doc.Add(new Paragraph("\nGenerated on: " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt"), normalFont));

                doc.Close();

                MessageBox.Show($"PDF successfully saved at:\n{filePath}", "PDF Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpFrom_ValueChanged_1(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date; // Start of selected day
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // End of selected day (23:59:59)

            LoadDeliveries(fromDate, toDate); // Load main deliveries
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date; // Start of selected day
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // End of selected day (23:59:59)

            LoadDeliveries(fromDate, toDate); // Load main deliveries
        }

        private void dgvDeliveries_Click(object sender, EventArgs e)
        {

        }
    }
    }
