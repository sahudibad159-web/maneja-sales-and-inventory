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
    public partial class Userdgv : Form
    {
        public Userdgv()
        {
            InitializeComponent();
            StyleDataGridView(dgvUsers);
            LoadUsers();
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
            dgv.ScrollBars = ScrollBars.Vertical;

            // Disable adding rows by user
            dgv.AllowUserToAddRows = false;

            // Disable row headers
            dgv.RowHeadersVisible = false;

            // Columns auto-size based on content
            // Let columns fill evenly, pero may padding at hindi dikit-dikit
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Optional: make text wrap neatly
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Keep readable proportions per column (optional fine-tuning)
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 80;
                col.FillWeight = 1; // para pantay lahat
            }


            // Single row selection
            dgv.MultiSelect = false;
        }


        private void LoadUsers()
        {
            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query = @"SELECT UserID, Username, PasswordHash, FullName, Role, Status, DateCreated 
                             FROM users 
                             ORDER BY DateCreated DESC";

                    using (var cmd = new MySqlCommand(query, con))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvUsers.DataSource = dt;

                        // 🔹 Hide sensitive columns
                        if (dgvUsers.Columns.Contains("UserID"))
                            dgvUsers.Columns["UserID"].Visible = false;

                        if (dgvUsers.Columns.Contains("PasswordHash"))
                            dgvUsers.Columns["PasswordHash"].Visible = false;

                        // 🔹 Adjust display headers
                        dgvUsers.Columns["Username"].HeaderText = "Username";
                        dgvUsers.Columns["FullName"].HeaderText = "Full Name";
                        dgvUsers.Columns["Role"].HeaderText = "Role";
                        dgvUsers.Columns["Status"].HeaderText = "Status";
                        dgvUsers.Columns["DateCreated"].HeaderText = "Date Created";

                        // 🔹 Optional: formatting & grid style
                        dgvUsers.ReadOnly = true;
                        dgvUsers.AllowUserToAddRows = false;
                        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgvUsers.RowHeadersVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Userdgv_Load(object sender, EventArgs e)
        {

        }
    }
}
