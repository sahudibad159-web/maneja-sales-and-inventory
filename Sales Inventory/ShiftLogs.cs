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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

using iTextSharpFont = iTextSharp.text.Font;

namespace Sales_Inventory
{
    public partial class ShiftLogs : Form
    {
        private int shiftID;
        public ShiftLogs()
        {
            InitializeComponent();
            StyleDataGridView(dgvShift_logs);
          
        }

        private void ShiftLogs_Load(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date; // Get the selected start date
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // Get the selected end date (full day)

            LoadShiftLogs(fromDate, toDate); // Pass the dates to the LoadShiftLogs method
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
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.ColumnHeadersHeight = 50;

            // Row style
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowsDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            dgv.RowsDefaultCellStyle.Padding = new Padding(3);
            dgv.RowTemplate.Height = 28;

            // Selection style
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Disable adding rows by user
            dgv.AllowUserToAddRows = false;

            // Disable row headers
            dgv.RowHeadersVisible = false;

            // Allow resizing
            dgv.AllowUserToResizeColumns = true;

            // Column sizing: fit header + content
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Wrap long cell text
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Scrollbars → horizontal + vertical
            dgv.ScrollBars = ScrollBars.Both;

            // Minimum width for readability
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.MinimumWidth = 60;
                if (col.Name == "Description" || col.Name == "VoidReason" || col.Name == "PaymentMethod")
                    col.FillWeight = 2;
                else
                    col.FillWeight = 1;

                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void LoadShiftLogs(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ConnectionModule.openCon();

                string query = @"SELECT ShiftID, CashierName, ShiftStart, ShiftEnd, TotalSales, CashSales, 
                            TotalDiscounts, VoidedTotal, NetSales, GCashSales, PointsSales, 
                            SeniorDiscount, PwdDiscount
                         FROM shift_logs
                         WHERE ShiftStart BETWEEN @From AND @To
                         ORDER BY ShiftStart DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@From", fromDate);
                    cmd.Parameters.AddWithValue("@To", toDate);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dgvShift_logs.DataSource = dt;

                        // Hide the ShiftID column
                        if (dgvShift_logs.Columns.Contains("ShiftID"))
                            dgvShift_logs.Columns["ShiftID"].Visible = false;
                        dgvShift_logs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shift logs: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }




        private void dgvShift_logs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int shiftID = Convert.ToInt32(dgvShift_logs.Rows[e.RowIndex].Cells["ShiftID"].Value);
                // Open the SaleDetailsForm and pass the ShiftID
                ShiftTransactionsForm detailsForm = new ShiftTransactionsForm(shiftID);
                detailsForm.ShowDialog(); // or Show() if you want non-modal
                
            }
        }
       

        private void dgvShift_logs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPrintPDF_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fromDate = dtpFrom.Value.Date;
                DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // Include full end day

                string query = @"SELECT CashierName, ShiftStart, ShiftEnd, 
                                TotalSales, CashSales, TotalDiscounts, 
                                VoidedTotal, NetSales, GCashSales, PointsSales, 
                                SeniorDiscount, PwdDiscount
                         FROM shift_logs
                         WHERE ShiftStart BETWEEN @From AND @To
                         ORDER BY ShiftStart DESC";

                DataTable dt = new DataTable();

                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@From", fromDate);
                    cmd.Parameters.AddWithValue("@To", toDate);
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No shift logs found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ✅ Folder path
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ShiftLogsReports");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = $"ShiftLogs_Report_{fromDate:yyyyMMdd}_to_{toDate:yyyyMMdd}.pdf";
                string filePath = Path.Combine(folderPath, fileName);

                // ✅ Create PDF
                Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // Header
                var titleFont = FontFactory.GetFont("Arial", 16, iTextSharpFont.BOLD);
                var normalFont = FontFactory.GetFont("Arial", 10, iTextSharpFont.NORMAL);


                Paragraph title = new Paragraph("SHIFT LOGS REPORT\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                Paragraph dateRange = new Paragraph($"From: {fromDate:MMMM dd, yyyy}  To: {toDate:MMMM dd, yyyy}\n\n", normalFont);
                dateRange.Alignment = Element.ALIGN_CENTER;
                doc.Add(dateRange);

                // ✅ Table setup
                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.WidthPercentage = 100;
                table.SpacingBefore = 10f;

                // Add header cells
                foreach (DataColumn column in dt.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, FontFactory.GetFont("Arial", 10, iTextSharpFont.BOLD, BaseColor.WHITE)));
                    cell.BackgroundColor = new BaseColor(0, 102, 204); // Blue header
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                // Add data rows
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(item?.ToString() ?? "", FontFactory.GetFont("Arial", 9)));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                }

                doc.Add(table);

                Paragraph footer = new Paragraph($"\nGenerated on: {DateTime.Now:MMMM dd, yyyy hh:mm tt}",
                                  FontFactory.GetFont("Arial", 9, iTextSharpFont.ITALIC));
                footer.Alignment = Element.ALIGN_RIGHT;
                doc.Add(footer);

                doc.Close();

                MessageBox.Show($"PDF generated successfully!\n\nLocation: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date; // Ensure the time is set to midnight
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // Set to the end of the selected day
            LoadShiftLogs(fromDate, toDate);

        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date; // Ensure the time is set to midnight
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // Set to the end of the selected day
            LoadShiftLogs(fromDate, toDate);
        }
    }
}
