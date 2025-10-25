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
    public partial class NewDeliveryUserControl : UserControl
    {
        MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory");
        public string DeliveryReceipt { get; set; }
        public string CompanyName { get; set; }
        public DateTime DateDelivered { get; set; }
        public string ReceivedBy { get; set; } 

        public event EventHandler DeliverySaved;
        public NewDeliveryUserControl()
        {
            InitializeComponent();

        }

        private void NewDeliveryUserControl_Load(object sender, EventArgs e)
        {
            // Set MaxDate to today
            dtpDeliveryDate.MaxDate = DateTime.Now.Date;

            LoadSupplier();
            DisableContextMenu();
        }
        private void LoadSupplier()
        {
            try
            {
                ConnectionModule.openCon();
                string query = "SELECT SupplierID, SupplierName FROM Supplier";
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCompanyName.DisplayMember = "SupplierName";
                cmbCompanyName.ValueMember = "SupplierID";
                cmbCompanyName.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }

        private void DisableContextMenu()
        {
            txtDeliveryReceipt.ContextMenuStrip = new ContextMenuStrip();
            dtpDeliveryDate.ContextMenuStrip = new ContextMenuStrip();
        }

        private void ClearClipboard()
        {
            Clipboard.Clear();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
          
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          


        }

        private void cmbReceivedBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate kung may laman ang DeliveryReceipt
            if (string.IsNullOrWhiteSpace(txtDeliveryReceipt.Text))
            {
                MessageBox.Show("Please enter the Delivery Receipt number.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDeliveryReceipt.Focus();
                return;
            }

            // Validate CompanyName: Ensure a company is selected
            if (cmbCompanyName.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Supplier Name.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCompanyName.Focus();
                return;
            }

            // Validate Delivery Date: bawal future date
            if (dtpDeliveryDate.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Invalid delivery date. Hindi pwedeng future date.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Delivery Receipt: bawal duplicate for the same CompanyName
            using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM delivery WHERE DeliveryReceipt = @Receipt AND SupplierID = @Supplier", con))
            {
                cmd.Parameters.AddWithValue("@Receipt", txtDeliveryReceipt.Text.Trim());
                cmd.Parameters.AddWithValue("@Supplier", cmbCompanyName.SelectedValue);

                try
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Delivery Receipt number already exists for this Supplier. Please enter a unique one.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDeliveryReceipt.Focus();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking Delivery Receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    con.Close();
                }
            }


            DeliveryReceipt = txtDeliveryReceipt.Text.Trim();
            CompanyName = cmbCompanyName.Text.Trim();
            DateDelivered = dtpDeliveryDate.Value.Date;
            ReceivedBy = cmbReceivedBy.Text.Trim();

            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                Panel mainPanel = parentForm.Controls["MainPanel"] as Panel;
                if (mainPanel != null)
                {
                    mainPanel.Controls.Clear();

                    // Pass values here
                    UC_Delivery deliveryUC = new UC_Delivery(DeliveryReceipt, CompanyName, DateDelivered, ReceivedBy);
                    deliveryUC.Dock = DockStyle.Fill;
                    mainPanel.Controls.Add(deliveryUC);
                }
            }
        }
    }
}
