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
    public partial class MemberYarn : Form
    {
        public MemberYarn()
        {
            InitializeComponent();
            StyleDataGridView(dgvMembers);
        }
        private void LoadMembers(string searchTerm = "")
        {
            try
            {
                dgvMembers.Columns.Clear();
                dgvMembers.Rows.Clear();

                // Add visible columns
                dgvMembers.Columns.Add("MemberCode", "Member Code");
                dgvMembers.Columns.Add("FirstName", "First Name");
                dgvMembers.Columns.Add("LastName", "Last Name");
                dgvMembers.Columns.Add("ContactNumber", "Contact Number");
                dgvMembers.Columns.Add("Points", "Points");
                dgvMembers.Columns.Add("DateJoined", "Date Joined");

                // Hidden column
                dgvMembers.Columns.Add("MemberID", "MemberID");
                dgvMembers.Columns["MemberID"].Visible = false;

                using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
                {
                    con.Open();
                    string query = @"
                SELECT MemberID, MemberCode, FirstName, LastName, ContactNumber, Points, DateJoined 
                FROM members 
                WHERE CONCAT(MemberCode, ' ', FirstName, ' ', LastName) LIKE @search
                ORDER BY DateJoined DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int rowIndex = dgvMembers.Rows.Add();
                                DataGridViewRow row = dgvMembers.Rows[rowIndex];

                                row.Cells["MemberID"].Value = reader["MemberID"];
                                row.Cells["MemberCode"].Value = reader["MemberCode"].ToString();
                                row.Cells["FirstName"].Value = reader["FirstName"].ToString();
                                row.Cells["LastName"].Value = reader["LastName"].ToString();
                                row.Cells["ContactNumber"].Value = reader["ContactNumber"].ToString();
                                row.Cells["Points"].Value = Convert.ToInt32(reader["Points"]);
                                row.Cells["DateJoined"].Value = Convert.ToDateTime(reader["DateJoined"]).ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
               


                StyleDataGridView(dgvMembers);
                dgvMembers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvMembers.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading members: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            dgv.RowsDefaultCellStyle.Padding = new Padding(5);
            dgv.RowTemplate.Height = 30;

            // Selection style
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Disable adding rows by user
            dgv.AllowUserToAddRows = false;

            // Disable row headers
            dgv.RowHeadersVisible = false;

            // Columns fill evenly across available width
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.ScrollBars = ScrollBars.Vertical;
            // Pantay ang columns at minimum width
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 100; // adjust kung gusto mo mas malaki
                col.FillWeight = 1;     // pantay ang distribution
            }

            // Single row selection
            dgv.MultiSelect = false;

            // Optional: alternating row colors for better readability
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
        }

        private void MemberYarn_Load(object sender, EventArgs e)
        {
            LoadMembers();
          

        }

        private void txtSearchCategory_TextChanged(object sender, EventArgs e)
        {
            LoadMembers(txtSearchMember.Text.Trim());
        }

        private void dgvMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMembers.Columns[e.ColumnIndex].Name == "DeleteColumn")
            {
                var row = dgvMembers.Rows[e.RowIndex];
                int memberId = Convert.ToInt32(row.Cells["MemberID"].Value);

                var confirm = MessageBox.Show("Are you sure you want to delete this member?",
                                              "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
                        {
                            con.Open();
                            string deleteQuery = "DELETE FROM members WHERE MemberID=@MemberID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@MemberID", memberId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Remove row from dgv
                        dgvMembers.Rows.RemoveAt(e.RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting member: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
                }
    }
}
