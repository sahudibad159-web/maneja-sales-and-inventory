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
    public partial class ViewProduct : Form
    {
       // public int SelectedProductID { get; private set; }
        public string SelectedProductName { get; private set; }
        public string SelectedDescription { get; private set; }
        public int SelectedStock { get; private set; }     // available stock from inventory
        public int SelectedQuantity { get; private set; }  // quantity to add to car
        public decimal SelectedPrice { get; private set; }
        public int SelectedProductID { get; set; }

        public ViewProduct()
        {
            InitializeComponent();
            SelectedQuantity = 1; // default kapag pinili mo sa popup
            LoadInventory();
            StyleDataGridView(dgvVieewProduct);
            dgvVieewProduct.CellDoubleClick += dgvVieewProduct_CellDoubleClick;

        }
        private void LoadInventory()
        {
            try
            {
                ConnectionModule.openCon();
                string query = @"
        SELECT 
               p.ProductID,
               p.ProductName,
               i.Description,
               i.QuantityInStock,
               p.RetailPrice
        FROM inventory i
        INNER JOIN product p ON i.ProductID = p.ProductID";

                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvVieewProduct.DataSource = dt;

                int minRows = 5;
                while (dgvVieewProduct.Rows.Count < minRows)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                // 🔹 Set column headers
                dgvVieewProduct.Columns["ProductName"].HeaderText = "Product";
                dgvVieewProduct.Columns["Description"].HeaderText = "Description";
                dgvVieewProduct.Columns["QuantityInStock"].HeaderText = "Stocks";

                // 🔹 Hide ProductID and RetailPrice columns
                dgvVieewProduct.Columns["ProductID"].Visible = false;   // 👈 ILAGAY MO DITO
                dgvVieewProduct.Columns["RetailPrice"].Visible = false;

                dgvVieewProduct.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory: " + ex.Message);
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
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = true; // kung gusto mo makapili ng multiple products
            dgv.RowHeadersVisible = false;
            dgv.ClearSelection();

            // Pantay ang columns at minimum width
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.MinimumWidth = 100;
                col.FillWeight = 1;
            }
        }
        private void ViewProduct_Load(object sender, EventArgs e)
        {
            dgvVieewProduct.ClearSelection();

        }

        private void dgvVieewProduct_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvVieewProduct.Rows[e.RowIndex];

                SelectedProductID = Convert.ToInt32(row.Cells["ProductID"].Value); // ✅ ADD THIS LINE
                SelectedProductName = row.Cells["ProductName"].Value?.ToString();
                SelectedDescription = row.Cells["Description"].Value?.ToString();
                SelectedStock = Convert.ToInt32(row.Cells["QuantityInStock"].Value);
                SelectedQuantity = 1;
                SelectedPrice = Convert.ToDecimal(row.Cells["RetailPrice"].Value);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtSearchInventoryProduct_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchInventoryProduct.Text.Trim();

            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query = @"
                SELECT 
                    p.ProductID,
                    p.ProductName,
                    i.Description,
                    i.QuantityInStock,
                    p.RetailPrice
                FROM inventory i
                INNER JOIN product p ON i.ProductID = p.ProductID
                WHERE p.ProductName LIKE @search
                   OR i.Description LIKE @search
                ORDER BY p.ProductName ASC";

                    using (var cmd = new MySqlCommand(query, con))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvVieewProduct.DataSource = dt;

                        // Hide columns
                        if (dgvVieewProduct.Columns.Contains("ProductID"))
                            dgvVieewProduct.Columns["ProductID"].Visible = false;

                        if (dgvVieewProduct.Columns.Contains("RetailPrice"))
                            dgvVieewProduct.Columns["RetailPrice"].Visible = false;

                        // Adjust headers
                        if (dgvVieewProduct.Columns.Contains("ProductName"))
                            dgvVieewProduct.Columns["ProductName"].HeaderText = "Product";

                        if (dgvVieewProduct.Columns.Contains("Description"))
                            dgvVieewProduct.Columns["Description"].HeaderText = "Description";

                        if (dgvVieewProduct.Columns.Contains("QuantityInStock"))
                            dgvVieewProduct.Columns["QuantityInStock"].HeaderText = "Stocks";

                        dgvVieewProduct.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching inventory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
