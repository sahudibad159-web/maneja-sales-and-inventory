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
    public partial class NearlyExpired : Form
    {
        public NearlyExpired()
        {
            InitializeComponent();
            LoadNearlyExpiredProducts();
            StyleDataGridView(dgvNearlyExpiredProduct);
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
        private void LoadNearlyExpiredProducts()
        {
            string query = @"
        SELECT 
            n.idNearlyExpired,
            n.ProductName,
            n.Description,  -- ✅ Added column
            n.Quantity,
            n.ExpirationDate,
            GREATEST(DATEDIFF(n.ExpirationDate, CURDATE()), 0) AS DaysRemaining
        FROM nearly_expired_products n
        WHERE n.ExpirationDate > CURDATE()  -- only show future dates
        ORDER BY n.ExpirationDate ASC";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                try
                {
                    con.Open();

                    // Optional: remove already expired items from the table
                    string deleteExpiredQuery = "DELETE FROM nearly_expired_products WHERE ExpirationDate <= CURDATE()";
                    using (var cmdDel = new MySqlCommand(deleteExpiredQuery, con))
                    {
                        cmdDel.ExecuteNonQuery();
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvNearlyExpiredProduct.DataSource = dt;

                    // Hide ID column
                    if (dgvNearlyExpiredProduct.Columns.Contains("idNearlyExpired"))
                        dgvNearlyExpiredProduct.Columns["idNearlyExpired"].Visible = false;

                    // Optional: make column headers look nice
                    dgvNearlyExpiredProduct.Columns["ProductName"].HeaderText = "Product Name";
                    dgvNearlyExpiredProduct.Columns["Description"].HeaderText = "Description";
                    dgvNearlyExpiredProduct.Columns["Quantity"].HeaderText = "Quantity";
                    dgvNearlyExpiredProduct.Columns["ExpirationDate"].HeaderText = "Expiration Date";
                    dgvNearlyExpiredProduct.Columns["DaysRemaining"].HeaderText = "Days Remaining";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Nearly Expired products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }







        private void NearlyExpired_Load(object sender, EventArgs e)
        {
          
        }
    }
}
