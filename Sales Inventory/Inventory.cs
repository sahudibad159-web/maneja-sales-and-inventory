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
    public partial class Inventory : UserControl
    {
        public Inventory()
        {
            InitializeComponent();
            StyleDataGridView(dgvInventory);
         
           
         
           
        }



        private void LoadInventory()
        {
            try
            {
                ConnectionModule.openCon();
                string query = @"
    SELECT 
        p.ProductName,
        i.Description,
        i.QuantityInStock,
        CASE 
            WHEN i.QuantityInStock <= 0 THEN 'Out of Stock'
            ELSE 'In Stock'
        END AS Status
    FROM inventory i
    INNER JOIN product p ON i.ProductID = p.ProductID";


                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvInventory.DataSource = dt;

                int minRows = 5;
                while (dgvInventory.Rows.Count < minRows)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                dgvInventory.Columns["ProductName"].HeaderText = "Product";
                dgvInventory.Columns["Description"].HeaderText = "Description";
                dgvInventory.Columns["QuantityInStock"].HeaderText = "Stocks";
                dgvInventory.Columns["Status"].HeaderText = "Status"; // ✅ Added column
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
            // Center align content horizontally and vertically in all cells
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
        private void CheckInventoryBatchesForExpiration()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();

                    DateTime today = DateTime.Now.Date;
                    int expiredCount = 0;
                    int nearlyCount = 0;

                    // Get all delivery batches that haven't been processed yet
                                    string query = @"
                                SELECT d.idDetail, d.idDelivery, d.ProductID, d.QtyDelivered, d.ExpirationDate, 
                       p.ProductName, i.Description
                FROM delivery_details d
                INNER JOIN product p ON d.ProductID = p.ProductID
                INNER JOIN inventory i ON i.ProductID = p.ProductID
                WHERE d.Status != 'Expired'
                ORDER BY STR_TO_DATE(d.ExpirationDate, '%Y-%m-%d') ASC;
                "; // earliest expiration first

                    DataTable dtBatches = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(query, con))
                    {
                        da.Fill(dtBatches);
                    }

                    foreach (DataRow row in dtBatches.Rows)
                    {
                        int idDetail = Convert.ToInt32(row["idDetail"]);
                        int productID = Convert.ToInt32(row["ProductID"]);
                        string productName = row["ProductName"].ToString();
                        int qtyDelivered = Convert.ToInt32(row["QtyDelivered"]);
                        string descriptionName = row["Description"].ToString();
                        string expValue = row["ExpirationDate"].ToString().Trim();
                        if (!DateTime.TryParse(expValue, out DateTime expDate))
                            continue;

                        double daysRemaining = (expDate.Date - today).TotalDays;

                        if (daysRemaining <= 0) // Expired
                        {
                            InsertExpiredProduct(con, productID, idDetail, productName, descriptionName, qtyDelivered, expDate);
                            ReduceInventoryByExpiredBatch(con, productID, qtyDelivered);
                            UpdateDeliveryDetailStatus(con, idDetail, "Expired");
                            expiredCount++;
                        }
                        else if (daysRemaining <= 30) // Nearly expired
                        {
                            InsertNearlyExpiredProduct(con, productID, idDetail, productName, descriptionName, qtyDelivered, expDate, (int)daysRemaining);
                            nearlyCount++;
                        }
                    }

                    // Optional: feedback
                    // MessageBox.Show($"Expired: {expiredCount}, Nearly Expired: {nearlyCount}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking inventory batches for expiration: " + ex.Message);
            }
        }

        private void ReduceInventoryByExpiredBatch(MySqlConnection con, int productID, int qtyToReduce)
        {
            // Get current stock
            string getQtyQuery = "SELECT QuantityInStock FROM inventory WHERE ProductID=@ProductID LIMIT 1";
            object result = new MySqlCommand(getQtyQuery, con) { Parameters = { new MySqlParameter("@ProductID", productID) } }.ExecuteScalar();

            if (result == null) return; // no inventory row

            int currentQty = Convert.ToInt32(result);
            if (currentQty <= 0) return;

            int newQty = Math.Max(0, currentQty - qtyToReduce);

            string updateQuery = "UPDATE inventory SET QuantityInStock=@newQty WHERE ProductID=@ProductID";
            using (MySqlCommand cmd = new MySqlCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("@newQty", newQty);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.ExecuteNonQuery();
            }
        }


        // Insert or update expired product
        private void InsertExpiredProduct(MySqlConnection con, int productID, int idDetail, string productName, string descriptionName, int qty, DateTime expDate)
        {
            string checkQuery = @"SELECT Quantity FROM expired_products 
                          WHERE idDetail=@idDetail 
                            AND DATE(ExpirationDate)=@ExpirationDate";
            object result = new MySqlCommand(checkQuery, con)
            {
                Parameters =
        {
            new MySqlParameter("@idDetail", idDetail),
            new MySqlParameter("@ExpirationDate", expDate.Date)
        }
            }.ExecuteScalar();

            if (result != null)
            {
                // 🧩 If existing record, update quantity and description (optional)
                string updateQuery = @"UPDATE expired_products 
                               SET Quantity = Quantity + @Quantity,
                                   Description = @Description
                               WHERE idDetail=@idDetail 
                                 AND DATE(ExpirationDate)=@ExpirationDate";
                using (MySqlCommand cmd = new MySqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Quantity", qty);
                    cmd.Parameters.AddWithValue("@Description", descriptionName);
                    cmd.Parameters.AddWithValue("@idDetail", idDetail);
                    cmd.Parameters.AddWithValue("@ExpirationDate", expDate.Date);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                // 🧩 If not existing, insert new record
                string insertQuery = @"INSERT INTO expired_products
                               (idInventory, idDetail, ProductName, Description, Quantity, ExpirationDate)
                               VALUES ((SELECT idInventory FROM inventory WHERE ProductID=@ProductID LIMIT 1),
                                       @idDetail, @ProductName, @Description, @Quantity, @ExpirationDate)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@idDetail", idDetail);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@Description", descriptionName);
                    cmd.Parameters.AddWithValue("@Quantity", qty);
                    cmd.Parameters.AddWithValue("@ExpirationDate", expDate.Date);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Insert or update nearly expired product
        private void InsertNearlyExpiredProduct(
      MySqlConnection con,
      int productID,
      int idDetail,
      string productName,
      string descriptionName,
      int qty,
      DateTime expDate,
      int daysRemaining)
        {
            // 1️⃣ Check kung existing na ang record (by ProductName + ExpirationDate + Description)
            string duplicateCheck = @"
        SELECT COUNT(*) 
        FROM nearly_expired_products 
        WHERE ProductName = @ProductName 
          AND DATE(ExpirationDate) = @ExpirationDate
          AND Description = @Description";

            using (MySqlCommand checkCmd = new MySqlCommand(duplicateCheck, con))
            {
                checkCmd.Parameters.AddWithValue("@ProductName", productName);
                checkCmd.Parameters.AddWithValue("@ExpirationDate", expDate.Date);
                checkCmd.Parameters.AddWithValue("@Description", descriptionName);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    // 2️⃣ Update existing record instead of inserting new one
                    string updateQuery = @"
                UPDATE nearly_expired_products 
                SET Quantity = @Quantity,
                    DaysRemaining = @DaysRemaining
                WHERE ProductName = @ProductName 
                  AND DATE(ExpirationDate) = @ExpirationDate
                  AND Description = @Description";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Quantity", qty);
                        cmd.Parameters.AddWithValue("@DaysRemaining", daysRemaining);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@ExpirationDate", expDate.Date);
                        cmd.Parameters.AddWithValue("@Description", descriptionName);
                        cmd.ExecuteNonQuery();
                    }
                    return; // ✅ Done updating, no need to insert
                }
            }

            // 3️⃣ If not existing yet → insert new
            string insertQuery = @"
        INSERT INTO nearly_expired_products
            (idInventory, idDetail, ProductName, Quantity, ExpirationDate, DaysRemaining, Description)
        VALUES (
            (SELECT idInventory FROM inventory WHERE ProductID = @ProductID LIMIT 1),
            @idDetail, 
            @ProductName, 
            @Quantity, 
            @ExpirationDate, 
            @DaysRemaining, 
            @Description)";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
            {
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@idDetail", idDetail);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@Quantity", qty);
                cmd.Parameters.AddWithValue("@ExpirationDate", expDate.Date);
                cmd.Parameters.AddWithValue("@DaysRemaining", daysRemaining);
                cmd.Parameters.AddWithValue("@Description", descriptionName);

                cmd.ExecuteNonQuery();
            }
        }




        // Mark delivery detail as expired
        private void UpdateDeliveryDetailStatus(MySqlConnection con, int idDetail, string status)
        {
            string updateQuery = "UPDATE delivery_details SET Status=@Status WHERE idDetail=@idDetail";
            using (MySqlCommand cmd = new MySqlCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@idDetail", idDetail);
                cmd.ExecuteNonQuery();
            }
        }



        private void Inventory_Load(object sender, EventArgs e)
        {
           
            dgvInventory.ClearSelection();
            dgvInventory.CurrentCell = null;

            CheckInventoryBatchesForExpiration(); // now runs after load
            LoadInventory(); // refresh inventory grid
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvInventory.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Out of Stock")
                {
                    dgvInventory.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.MistyRose;
                    dgvInventory.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                }
                else
                {
                    dgvInventory.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvInventory.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void txtSearchInventory_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchInventory.Text.Trim();

            try
            {
                using (var con = new MySqlConnection(ConnectionModule.con.ConnectionString))
                {
                    con.Open();
                    string query = @"
                SELECT 
                    p.ProductName,
                    p.Description,
                    i.QuantityInStock,
                    CASE 
                        WHEN i.QuantityInStock <= 0 THEN 'Out of Stock'
                        ELSE 'In Stock'
                    END AS Status
                FROM inventory i
                INNER JOIN product p ON i.ProductID = p.ProductID
                WHERE p.ProductName LIKE @search
                   OR p.Description LIKE @search
                ORDER BY p.ProductName ASC";

                    using (var cmd = new MySqlCommand(query, con))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvInventory.DataSource = dt;

                        // Minimum 5 rows for consistent display
                        int minRows = 5;
                        while (dgvInventory.Rows.Count < minRows)
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                        }

                        // Adjust headers
                        if (dgvInventory.Columns.Contains("ProductName"))
                            dgvInventory.Columns["ProductName"].HeaderText = "Product";

                        if (dgvInventory.Columns.Contains("Description"))
                            dgvInventory.Columns["Description"].HeaderText = "Description";

                        if (dgvInventory.Columns.Contains("QuantityInStock"))
                            dgvInventory.Columns["QuantityInStock"].HeaderText = "Stocks";

                        if (dgvInventory.Columns.Contains("Status"))
                            dgvInventory.Columns["Status"].HeaderText = "Status";

                        dgvInventory.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching inventory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
