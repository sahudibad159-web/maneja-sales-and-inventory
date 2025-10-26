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
    public partial class AuditTrail : Form
    {
        public AuditTrail()
        {
            InitializeComponent();

            StyleDataGridView(dgvAuditTrail);
          

        }
        private void LoadAuditTrail(DateTime fromDate, DateTime toDate)
        {
            try
            {
                ConnectionModule.openCon();

                string query = @"
            SELECT AuditID, Username, ActionType, ModuleName, Description, ActionDate 
            FROM AuditTrail
            WHERE ActionDate BETWEEN @From AND @To
            ORDER BY ActionDate DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@From", fromDate);
                    cmd.Parameters.AddWithValue("@To", toDate);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dgvAuditTrail.DataSource = dt;
                    }
                }

                // Hide ID column
                if (dgvAuditTrail.Columns.Contains("AuditID"))
                    dgvAuditTrail.Columns["AuditID"].Visible = false;

                // Optional styling (if you want same look as your shift logs)
                StyleDataGridView(dgvAuditTrail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading audit trail: " + ex.Message);
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
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 35;

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

            // Columns fill evenly
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ✅ Important para hindi sobrang sikip pero readable pa rin
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Scrollbars → Horizontal + Vertical
            dgv.ScrollBars = ScrollBars.Both;

            // Pantay columns pero Description 2x bigger
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 80;

                if (col.Name == "Description")
                {
                    col.FillWeight = 2; // x2 size only (hindi sobrang laki)
                }
                else
                {
                    col.FillWeight = 1;
                }
            }
        }


        private void AuditTrail_Load(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date; // Start of selected day
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // End of selected day (23:59:59)

            LoadAuditTrail(fromDate, toDate);
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date; // Start of selected day
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // End of selected day (23:59:59)

            LoadAuditTrail(fromDate, toDate);
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date; // Start of selected day
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // End of selected day (23:59:59)

            LoadAuditTrail(fromDate, toDate);
        }
    }
}
