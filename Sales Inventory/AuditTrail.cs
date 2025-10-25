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
            LoadAuditTrail();
            StyleDataGridView(dgvAuditTrail);
        }
        private void LoadAuditTrail()
        {
            try
            {
                ConnectionModule.openCon();
                string query = "SELECT AuditID, Username, ActionType, ModuleName, Description, ActionDate FROM AuditTrail ORDER BY ActionDate DESC";
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvAuditTrail.DataSource = dt;
                // Itago ang ID column para di makita ng user
                if (dgvAuditTrail.Columns.Contains("AuditID"))
                    dgvAuditTrail.Columns["AuditID"].Visible = false;

                // Format DGV
                dgvAuditTrail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvAuditTrail.ReadOnly = true;
                dgvAuditTrail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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

        }
    }
}
