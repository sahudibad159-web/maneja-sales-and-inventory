using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;


namespace Sales_Inventory
{
    public partial class StockReport : Form
    {
        public StockReport()
        {
            InitializeComponent();
            StyleDataGridView(dgvStockReport);
          
           
        }

        private void StockReport_Load(object sender, EventArgs e)
        {
            LoadStockReport();
            dgvStockReport.ClearSelection();
            dgvStockReport.CurrentCell = null;
        }


        private void LoadStockReport(bool showOut = false)
        {
            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query;

                    if (!showOut)
                    {
                        // STOCK IN = deliveries that entered inventory
                        query = @"
SELECT
    p.ProductName,
    dd.QtyDelivered,
    d.DeliveryDate,
    s.SupplierName AS Source
FROM delivery_details dd
INNER JOIN delivery d ON dd.idDelivery = d.idDelivery
INNER JOIN product p ON dd.ProductID = p.ProductID
INNER JOIN supplier s ON d.SupplierID = s.SupplierID
ORDER BY d.DeliveryDate ASC;";
                    }
                    else
                    {
                        // STOCK OUT = per batch sold/remaining
                        //                        query = @"
                        //SELECT 
                        //    dd.idDetail,
                        //    p.ProductName,
                        //    dd.QtyDelivered,
                        //    IFNULL(SUM(im.Quantity), 0) AS SoldQty,
                        //    (dd.QtyDelivered - IFNULL(SUM(im.Quantity),0)) AS RemainingQty,
                        //    CASE 
                        //        WHEN (dd.QtyDelivered - IFNULL(SUM(im.Quantity),0)) <= 0 THEN 'Out of stock'
                        //        ELSE 'Available'
                        //    END AS Status,
                        //    dd.ExpirationDate,
                        //    d.DeliveryDate
                        //FROM delivery_details dd
                        //INNER JOIN delivery d ON dd.idDelivery = d.idDelivery
                        //INNER JOIN product p ON dd.ProductID = p.ProductID
                        //LEFT JOIN inventory_movements im ON im.idDetail = dd.idDetail AND im.MovementType='OUT'
                        //GROUP BY dd.idDetail
                        //ORDER BY d.DeliveryDate ASC;";
                        query = @"
                    SELECT 
                        dd.idDetail,
                        p.ProductName,
                        dd.QtyDelivered,
                        IFNULL(SUM(im.Quantity), 0) AS SoldQty,
                        IFNULL(di.TotalDamaged, 0) AS DamageQty,
                        IFNULL(ep.Quantity, 0) AS ExpiredQty,
                        GREATEST(
                            dd.QtyDelivered 
                            - IFNULL(SUM(im.Quantity),0) 
                            - IFNULL(di.TotalDamaged,0) 
                            - IFNULL(ep.Quantity,0),
                            0
                        ) AS RemainingQty,
                        CASE 
                            WHEN GREATEST(
                                     dd.QtyDelivered 
                                     - IFNULL(SUM(im.Quantity),0) 
                                     - IFNULL(di.TotalDamaged,0) 
                                     - IFNULL(ep.Quantity,0), 0
                                 ) = 0 THEN 'Out of stock'
                            ELSE 'Available'
                        END AS Status,
                        dd.ExpirationDate,
                        d.DeliveryDate
                    FROM delivery_details dd
                    INNER JOIN delivery d ON dd.idDelivery = d.idDelivery
                    INNER JOIN product p ON dd.ProductID = p.ProductID
                    LEFT JOIN inventory_movements im 
                           ON im.idDetail = dd.idDetail AND im.MovementType='OUT'
                    LEFT JOIN (
                        SELECT idDetail, SUM(QuantityDamaged) AS TotalDamaged
                        FROM damaged_items
                        GROUP BY idDetail
                    ) di ON di.idDetail = dd.idDetail
                    LEFT JOIN (
                        SELECT idDetail, SUM(Quantity) AS Quantity
                        FROM expired_products
                        GROUP BY idDetail
                    ) ep ON ep.idDetail = dd.idDetail
                    GROUP BY dd.idDetail
                    ORDER BY d.DeliveryDate ASC;

                    ";

                    }

                    using (var cmd = new MySqlCommand(query, con))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvStockReport.DataSource = dt;

                        // 🔹 Prevent automatic row highlight after data load
                        dgvStockReport.ClearSelection();
                        dgvStockReport.CurrentCell = null;


                    }
                }

                // Hide any ID columns automatically
                foreach (DataGridViewColumn col in dgvStockReport.Columns)
                {
                    if (col.Name.ToLower().Contains("id"))
                        col.Visible = false;
                }

                // Highlight fully sold batches in red (only for Stock OUT)
                if (showOut)
                {
                    foreach (DataGridViewRow row in dgvStockReport.Rows)
                    {
                        if (row.Cells["RemainingQty"].Value != null &&
                            Convert.ToInt32(row.Cells["RemainingQty"].Value) <= 0)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stock report: " + ex.Message);
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
            dgvStockReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStockReport.MultiSelect = false;


            // Optional: alternating row colors for better readability
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            LoadStockReport(chkShowOut.Checked); // Pass true = OUT, false = IN
        }

        private void btnDamageItem_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Check if a row is selected
                if (dgvStockReport.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a product first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dgvStockReport.SelectedRows[0];

                // ✅ Check if idDetail column exists
                if (!dgvStockReport.Columns.Contains("idDetail"))
                {
                    MessageBox.Show("idDetail column not found in DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Check if RemainingQty column exists and validate
                int remainingQty = 0;
                if (dgvStockReport.Columns.Contains("RemainingQty"))
                {
                    var remainingObj = selectedRow.Cells["RemainingQty"].Value;
                    if (remainingObj == null || !int.TryParse(remainingObj.ToString(), out remainingQty))
                    {
                        MessageBox.Show("Invalid Remaining Quantity value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (remainingQty <= 0)
                    {
                        MessageBox.Show("This item has no remaining stock and cannot be marked as damaged.",
                                        "No Stock Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("RemainingQty column not found in DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idDetail = Convert.ToInt32(selectedRow.Cells["idDetail"].Value);
                string productName = selectedRow.Cells["ProductName"].Value.ToString();

                // ✅ Get ProductID from database
                int idProduct = 0;
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    using (var cmd = new MySqlCommand("SELECT ProductID FROM delivery_details WHERE idDetail = @idDetail", con))
                    {
                        cmd.Parameters.AddWithValue("@idDetail", idDetail);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                            idProduct = Convert.ToInt32(result);
                        else
                        {
                            MessageBox.Show("Product not found for this batch.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // ✅ Ask for quantity
                string qtyInput = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter quantity to mark as damaged:",
                    "Damage Quantity",
                    "1");

                if (string.IsNullOrWhiteSpace(qtyInput))
                    return;

                // ✅ Validate numeric and positive input
                if (!int.TryParse(qtyInput, out int qtyDamaged))
                {
                    MessageBox.Show("Please enter a valid numeric value for quantity.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (qtyDamaged <= 0)
                {
                    MessageBox.Show("Quantity must be greater than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Validate against remaining quantity
                if (qtyDamaged > remainingQty)
                {
                    MessageBox.Show($"Cannot damage more than available stock ({remainingQty}).", "Quantity Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Ask for remarks
                string remarks = Microsoft.VisualBasic.Interaction.InputBox(
                    "Remarks (optional):",
                    "Damage Remarks",
                    "");

                // ✅ Get the current logged-in user (example)
                string reportedBy = ConnectionModule.Session.Role ?? "Unknown";

                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    // ✅ Insert into damaged_items
                    string insertQuery = @"
                INSERT INTO damaged_items (idDetail, ProductID, ProductName, QuantityDamaged, Remarks, ReportedBy)
                VALUES (@idDetail, @ProductID, @ProductName, @QuantityDamaged, @Remarks, @ReportedBy)";
                    using (var cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@idDetail", idDetail);
                        cmd.Parameters.AddWithValue("@ProductID", idProduct);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@QuantityDamaged", qtyDamaged);
                        cmd.Parameters.AddWithValue("@Remarks", remarks);
                        cmd.Parameters.AddWithValue("@ReportedBy", reportedBy);
                        cmd.ExecuteNonQuery();
                    }

                    // ✅ Deduct from inventory
                    string updateQuery = @"
                UPDATE inventory
                SET QuantityInStock = GREATEST(QuantityInStock - @qty, 0)
                WHERE ProductID = @ProductID";
                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@qty", qtyDamaged);
                        cmd.Parameters.AddWithValue("@ProductID", idProduct);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"{qtyDamaged} unit(s) of {productName} marked as damaged.",
                                    "Item Reported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // ✅ Refresh table after update
                LoadStockReport(true);
                dgvStockReport.ClearSelection();
                dgvStockReport.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error marking damaged item: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
