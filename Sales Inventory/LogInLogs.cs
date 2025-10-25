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
    public partial class LogInLogs : Form
    {
        public LogInLogs()
        {
            InitializeComponent();
            StyleDataGridView(dgvLoginLogs);
        }

        private void LogInLogs_Load(object sender, EventArgs e)
        {
            LoadLoginLogs();
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
        private void LoadLoginLogs()
        {
            try
            {
                ConnectionModule.openCon();

                string query = @"
            SELECT LogID, Username, Role, 
                   DATE_FORMAT(LoginTime, '%Y-%m-%d %H:%i:%s') AS LoginTime,
                   IFNULL(DATE_FORMAT(LogoutTime, '%Y-%m-%d %H:%i:%s'), '') AS LogoutTime,
                   Status
            FROM login_logs
            ORDER BY LoginTime DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dgvLoginLogs.DataSource = dt;

                    // Hide LogID column
                    if (dgvLoginLogs.Columns.Contains("LogID"))
                        dgvLoginLogs.Columns["LogID"].Visible = false;

                    // Style DataGridView
                    StyleDataGridView(dgvLoginLogs);

                    // Optional: color code Status
                    foreach (DataGridViewRow row in dgvLoginLogs.Rows)
                    {
                        if (row.Cells["Status"].Value != DBNull.Value)
                        {
                            string status = row.Cells["Status"].Value.ToString();
                            if (status.Contains("Failed"))
                                row.DefaultCellStyle.ForeColor = Color.Red;
                            else if (status.Contains("Logged In"))
                                row.DefaultCellStyle.ForeColor = Color.Green;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading login logs: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

    }
}
