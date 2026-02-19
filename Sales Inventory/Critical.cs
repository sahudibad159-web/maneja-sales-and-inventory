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
    public partial class Critical : Form
    {
        public Critical()
        {
            InitializeComponent();
            LoadLowStockItems();
            StyleDataGridView(dgvCriticalProduct);

        }
        // --- Load data from database ---
        private void LoadLowStockItems()
        {
            string query = @"
                SELECT 
                    p.ProductID,
                    p.ProductName,
                    i.QuantityInStock,
                    i.ReorderLevel
                FROM inventory i
                INNER JOIN product p ON i.ProductID = p.ProductID
                WHERE i.QuantityInStock <= i.ReorderLevel";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                try
                {
                    con.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvCriticalProduct.DataSource = dt;

                    // Hide ProductID column
                    if (dgvCriticalProduct.Columns.Contains("ProductID"))
                        dgvCriticalProduct.Columns["ProductID"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Critical items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- Style DataGridView ---
        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.GridColor = Color.LightGray;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 35;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.RowsDefaultCellStyle.Padding = new Padding(5);
            dgv.RowTemplate.Height = 40; // bigger row for readability
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // wrap long text

            dgv.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ScrollBars = ScrollBars.Vertical;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 100;
            }

            dgv.MultiSelect = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
        }

        private void Critical_Load(object sender, EventArgs e)
        {
           
        }

    }
}
