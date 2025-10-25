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

namespace Sales_Inventory
{
    public partial class ShiftTransactionsForm : Form
    {
        private int ShiftID;
        public ShiftTransactionsForm(int ShiftID)
        {
            InitializeComponent();
            StyleDataGridView(dgvSaleDetails);
            this.ShiftID = ShiftID;
        }

        private void ShiftTransactionsForm_Load(object sender, EventArgs e)
        {
            LoadSaleDetails();

        }
        private void LoadSaleDetails()
        {
            try
            {
                ConnectionModule.openCon();

                // Get CashierName, ShiftStart, ShiftEnd from shift_logs
                string shiftQuery = "SELECT CashierName, ShiftStart, ShiftEnd FROM shift_logs WHERE ShiftID = @ShiftID";
                string cashierName = "";
                DateTime shiftStart = DateTime.MinValue;
                DateTime shiftEnd = DateTime.MinValue;

                using (MySqlCommand cmdShift = new MySqlCommand(shiftQuery, ConnectionModule.con))
                {
                    cmdShift.Parameters.AddWithValue("@ShiftID", ShiftID);
                    using (MySqlDataReader readerShift = cmdShift.ExecuteReader())
                    {
                        if (readerShift.Read())
                        {
                            cashierName = readerShift["CashierName"].ToString();
                            shiftStart = Convert.ToDateTime(readerShift["ShiftStart"]);
                            shiftEnd = Convert.ToDateTime(readerShift["ShiftEnd"]);
                        }
                        else
                        {
                            MessageBox.Show("Shift not found.");
                            return;
                        }
                    }
                }

                // Get all sales for that shift
                string query = @"
        SELECT SaleID, TransactionDate, MemberID, TotalAmount, VATAmount, DiscountAmount,
               NetAmount, PaymentMethod, CashierName, IsVoided, VoidReason, VoidedBy, VoidedAt,
               RedeemedPoints, EarnedPoints, DiscountType
        FROM sales
        WHERE CashierName = @CashierName
          AND TransactionDate >= @ShiftStart
          AND TransactionDate <= @ShiftEnd
        ORDER BY TransactionDate ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@CashierName", cashierName);
                    cmd.Parameters.AddWithValue("@ShiftStart", shiftStart);
                    cmd.Parameters.AddWithValue("@ShiftEnd", shiftEnd);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dgvSaleDetails.DataSource = dt;
                        // Itago ang ID column para di makita ng user
                        if (dgvSaleDetails.Columns.Contains("SaleID"))
                            dgvSaleDetails.Columns["SaleID"].Visible = false;
                        dgvSaleDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        // Highlight voided transactions in red
                        foreach (DataGridViewRow row in dgvSaleDetails.Rows)
                        {
                            if (row.Cells["IsVoided"].Value != DBNull.Value &&
                                Convert.ToBoolean(row.Cells["IsVoided"].Value))
                            {
                                row.DefaultCellStyle.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sale details: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
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
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True; // ✅ wrap header text
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.ColumnHeadersHeight = 50; // adjust for long headers

            // Row style
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
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

            // Fit columns to content
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Wrap long cell text
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Scrollbars → horizontal + vertical
            dgv.ScrollBars = ScrollBars.Both;

            // Adjust column widths for important columns
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 60;

                if (col.Name == "Description" || col.Name == "VoidReason" || col.Name == "PaymentMethod")
                {
                    col.FillWeight = 2; // slightly bigger for readability
                }
                else
                {
                    col.FillWeight = 1;
                }
            }
        }

    }
}

