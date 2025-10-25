using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Sales_Inventory
{
    public partial class UC_Dashboard : UserControl
    {
        public UC_Dashboard()
        {
            InitializeComponent();
            BuildDashboard();
        }

        private string GetLowStockItems()
        {
            string query = @"
                SELECT COUNT(*) 
                FROM inventory i
                INNER JOIN product p ON i.ProductID = p.ProductID
                WHERE i.QuantityInStock <= p.ReorderLevel";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                return cmd.ExecuteScalar().ToString();
            }
        }

        private string GetExpiredProducts()
        {
            string query = "SELECT COUNT(*) FROM expired_products;";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value) return "0";
                    return Convert.ToInt32(result).ToString();
                }
            }
        }


        private string GetNearlyExpiredProducts()
        {
            string query = @"
        SELECT COUNT(*) 
        FROM nearly_expired_products
        WHERE ExpirationDate >= CURDATE()
          AND ExpirationDate <= DATE_ADD(CURDATE(), INTERVAL 30 DAY)
          AND Quantity > 0;";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value) return "0";
                    return Convert.ToInt32(result).ToString();
                }
            }
        }




        private Chart GetSalesChart()
        {
            Chart salesChart = new Chart();
            salesChart.Dock = DockStyle.Fill;
            salesChart.ChartAreas.Add(new ChartArea("SalesArea"));
            Series salesSeries = new Series("Sales")
            {
                ChartType = SeriesChartType.Column
            };

            string query = @"
                SELECT DATE_FORMAT(TransactionDate, '%b') AS Month, SUM(TotalAmount) 
                FROM sales 
                GROUP BY MONTH(TransactionDate) 
                ORDER BY MONTH(TransactionDate)";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        salesSeries.Points.AddXY(reader.GetString(0), reader.GetDecimal(1));
                    }
                }
            }

            salesChart.Series.Add(salesSeries);
            salesChart.Titles.Add("Sales Overview");
            return salesChart;
        }

        private Chart GetInventoryChart()
        {
            Chart inventoryChart = new Chart();
            inventoryChart.Dock = DockStyle.Fill;
            inventoryChart.ChartAreas.Add(new ChartArea("InventoryArea"));

            Series inventorySeries = new Series("Inventory")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black
            };

            // Use actual tables
            string query = @"
        SELECT
            (SELECT COUNT(*) 
             FROM inventory i
             INNER JOIN product p ON i.ProductID = p.ProductID
             WHERE i.QuantityInStock > p.ReorderLevel) AS InStock,

            (SELECT COUNT(*) 
             FROM inventory i
             INNER JOIN product p ON i.ProductID = p.ProductID
             WHERE i.QuantityInStock <= p.ReorderLevel) AS LowStock,

            (SELECT COUNT(*) FROM expired_products) AS Expired,

            (SELECT COUNT(*) 
             FROM nearly_expired_products
             WHERE ExpirationDate >= CURDATE()
               AND ExpirationDate <= DATE_ADD(CURDATE(), INTERVAL 30 DAY)
               AND Quantity > 0) AS NearlyExpired;";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inventorySeries.Points.AddXY("In Stock", reader.IsDBNull(0) ? 0 : reader.GetInt32(0));
                        inventorySeries.Points.AddXY("Low Stock", reader.IsDBNull(1) ? 0 : reader.GetInt32(1));
                        inventorySeries.Points.AddXY("Expired", reader.IsDBNull(2) ? 0 : reader.GetInt32(2));
                        inventorySeries.Points.AddXY("Nearly Expired", reader.IsDBNull(3) ? 0 : reader.GetInt32(3));
                    }
                }
            }

            // ✅ Apply your custom colors
            Color[] colors = {
        ColorTranslator.FromHtml("#2E8B57"), // In Stock
        ColorTranslator.FromHtml("#FF7F50"), // Low Stock
        ColorTranslator.FromHtml("#49597C"), // Expired
        ColorTranslator.FromHtml("#CD6363")  // Nearly Expired
    };

            for (int i = 0; i < inventorySeries.Points.Count; i++)
            {
                inventorySeries.Points[i].Color = colors[i];
                inventorySeries.Points[i].Label = $"{inventorySeries.Points[i].AxisLabel}: {inventorySeries.Points[i].YValues[0]}";
            }

            inventoryChart.Series.Add(inventorySeries);
            inventoryChart.Titles.Add("Inventory Status");
            inventoryChart.Legends.Add(new Legend { Docking = Docking.Bottom });

            return inventoryChart;
        }


        private Panel CreateSummaryBox(string title, string value, Color backColor, int width = 180)
        {
            Panel panel = new Panel();
            panel.Size = new Size(200, 100);
            panel.BackColor = backColor;
            panel.Margin = new Padding(10);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 30;

            Label lblValue = new Label();
            lblValue.Text = value ?? "0";  // 🔑 para sure hindi mawawala
            lblValue.ForeColor = Color.White;
            lblValue.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblValue.Dock = DockStyle.Fill;
            lblValue.TextAlign = ContentAlignment.MiddleCenter;

            panel.Controls.Add(lblValue);
            panel.Controls.Add(lblTitle);

            return panel;
        }


        private string GetTotalSales()
        {
            string query = "SELECT IFNULL(SUM(TotalAmount), 0) FROM sales";
            using (MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                return Convert.ToDecimal(cmd.ExecuteScalar()).ToString("N2");
            }
        }

        private void BuildDashboard()
        {
            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.RowCount = 2;
            layout.ColumnCount = 1;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150)); // taas ng summary row
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // ====== SUMMARY BOXES (4 equal columns) ======
            TableLayoutPanel summaryPanel = new TableLayoutPanel();
            summaryPanel.Dock = DockStyle.Fill;
            summaryPanel.ColumnCount = 4; // apat na pantay
            summaryPanel.RowCount = 1;
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            summaryPanel.Controls.Add(CreateSummaryBox("Total Sales", "₱" + GetTotalSales(), ColorTranslator.FromHtml("#2E8B57")), 0, 0);
            summaryPanel.Controls.Add(CreateSummaryBox("Critical Stock ", GetLowStockItems(), ColorTranslator.FromHtml("#FF7F50")), 1, 0);
            summaryPanel.Controls.Add(CreateSummaryBox("Expired Products", GetExpiredProducts(), ColorTranslator.FromHtml("#49597C")), 2, 0);
            summaryPanel.Controls.Add(CreateSummaryBox("Nearly Expired", GetNearlyExpiredProducts(), ColorTranslator.FromHtml("#CD6363")), 3, 0);


            // ====== CHARTS SIDE BY SIDE ======
            TableLayoutPanel chartLayout = new TableLayoutPanel();
            chartLayout.Dock = DockStyle.Fill;
            chartLayout.RowCount = 1;
            chartLayout.ColumnCount = 2;
            chartLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartLayout.Padding = new Padding(20);

            Chart salesChart = GetSalesChart();
            Chart inventoryChart = GetInventoryChart();

            chartLayout.Controls.Add(salesChart, 0, 0);
            chartLayout.Controls.Add(inventoryChart, 1, 0);

            layout.Controls.Add(summaryPanel, 0, 0);
            layout.Controls.Add(chartLayout, 0, 1);

            this.Controls.Add(layout);
        }


        private void UC_Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
