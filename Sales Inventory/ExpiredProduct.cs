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
    public partial class ExpiredProduct : Form
    {
        public ExpiredProduct()
        {

            InitializeComponent();
            StyleDataGridView(dgvExpiredProduct);
            LoadExpiredProducts();


        }
        private void LoadExpiredProducts()
        {
            string query = @"SELECT 
                        e.idExpired,
                        e.ProductName,
                        e.Quantity,
                        e.ExpirationDate
                     FROM expired_products e
                     ORDER BY e.ExpirationDate ASC";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                try
                {
                    con.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvExpiredProduct.DataSource = dt;

                    // Hide the ID column
                    if (dgvExpiredProduct.Columns.Contains("idExpired"))
                        dgvExpiredProduct.Columns["idExpired"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Expired Product form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }







        private void ExpiredProduct_Load(object sender, EventArgs e)
        {
        


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
    }
}
