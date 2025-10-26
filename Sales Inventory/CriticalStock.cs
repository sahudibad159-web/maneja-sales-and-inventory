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
    public partial class CriticalStock : Form
    {
        public CriticalStock()
        {
            InitializeComponent();
        }

        private void CriticalStock_Load(object sender, EventArgs e)
        {
            LoadCriticalProducts();
            StyleDataGridView(dgvCritical); // optional styling
        }
        private void LoadCriticalProducts()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
                {
                    con.Open();

                                        string query = @"
                    SELECT 
                        p.ProductID,
                        p.ProductName,
                        i.Description,
                        i.QuantityInStock,
                        i.ReorderLevel
                    FROM inventory i
                    INNER JOIN product p ON i.ProductID = p.ProductID
                    WHERE i.QuantityInStock <= i.ReorderLevel
                    ORDER BY i.QuantityInStock ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvCritical.DataSource = dt;

                    // Format column headers
                    dgvCritical.Columns["ProductName"].HeaderText = "Product";
                    dgvCritical.Columns["Description"].HeaderText = "Description";
                    dgvCritical.Columns["QuantityInStock"].HeaderText = "Stock";
                    dgvCritical.Columns["ReorderLevel"].HeaderText = "Reorder Level";

                    // Optional: hide ID column
                    if (dgvCritical.Columns.Contains("ProductID"))
                        dgvCritical.Columns["ProductID"].Visible = false;

                    dgvCritical.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading critical products: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


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

            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.RowTemplate.Height = 30;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
        }
    }
}
