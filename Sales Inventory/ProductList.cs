using Guna.UI2.WinForms.Suite;
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
    public partial class ProductList : UserControl
    {
        public event Action<string, string> ProductSelected;

        public ProductList()
        {
            InitializeComponent();
            LoadProducts();
            StyleDataGridView(dgvProducts);
        }
        private void LoadProducts()
        {
            try
            {
                ConnectionModule.openCon();
                string query = @"SELECT p.ProductID, p.Barcode, p.ProductName, c.CategoryName, p.Description, 
                        p.RetailPrice, p.WholeSalePrice, p.ReorderLevel, p.ReorderQuantity
                        FROM Product p
                        INNER JOIN Category c ON p.CategoryID = c.CategoryID";
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProducts.DataSource = dt;

                // Optional: Palitan ang column header para mas malinaw
                dgvProducts.Columns["CategoryName"].HeaderText = "Category";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }
        private void ProductList_Load(object sender, EventArgs e)
        {
            LoadProducts();
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
        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string productName = dgvProducts.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                string description = dgvProducts.Rows[e.RowIndex].Cells["Description"].Value.ToString();
               

                // Trigger the event
                ProductSelected?.Invoke(productName, description);

                // Optional: close parent form if hosted in popup
                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close();
                }
            }
        }
    }
}
