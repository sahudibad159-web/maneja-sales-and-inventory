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
    public partial class Category : Form
    {
        public Category()
        {
            InitializeComponent();
            LoadCategory();

        }
        // 🟢 Load all categories to DataGridView
      private void LoadCategory()
        {
            try
            {
                ConnectionModule.openCon();
                string query = "SELECT CategoryID, Category, Description FROM Category";
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
                ConnectionModule.closeCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }
        private void Category_Load(object sender, EventArgs e)
        {
            LoadCategory();
            // Modern look para sa POS / Inventory
            dgv.BorderStyle = BorderStyle.None;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215); // Windows Blue
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.BackgroundColor = Color.White;

            // Headers style
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 144, 255); // DodgerBlue
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 11, FontStyle.Bold);

            // Row style
            dgv.DefaultCellStyle.Font = new Font("Century Gothic", 10);
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.RowTemplate.Height = 35;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgv.CurrentRow.Cells["CategoryID"].Value);
                    ConnectionModule.openCon();
                    string query = "UPDATE Category SET Category=@Category, Description=@Description WHERE CategoryID=@CategoryID";
                    MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                    cmd.Parameters.AddWithValue("@Category", txtCategoryName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@CategoryID", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category updated successfully!");
                    ConnectionModule.closeCon();
                    LoadCategory();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating category: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgv.CurrentRow.Cells["CategoryID"].Value);
                    ConnectionModule.openCon();
                    string query = "DELETE FROM Category WHERE CategoryID=@CategoryID";
                    MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                    cmd.Parameters.AddWithValue("@CategoryID", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category deleted successfully!");
                    ConnectionModule.closeCon();
                    LoadCategory();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting category: " + ex.Message);
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    txtCategoryName.Text = dgv.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                    txtDescription.Text = dgv.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting category: " + ex.Message);
            }
        }
        private void ClearFields()
        {
            txtCategoryName.Clear();
            txtDescription.Clear();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                ConnectionModule.openCon();
                string query = "INSERT INTO Category (Category, Description) VALUES (@Category, @Description)";
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                cmd.Parameters.AddWithValue("@Category", txtCategoryName.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Category saved successfully!");
                ConnectionModule.closeCon();
                LoadCategory();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving category: " + ex.Message);
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
