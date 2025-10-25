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
    public partial class Delivery : Form
    {
        MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;database=sales_inventory");
        public string DeliveryReceipt { get; set; }
        public string CompanyName { get; set; }
        public DateTime DateDelivered { get; set; }
        public string ReceivedBy { get; set; }

        public event EventHandler DeliverySaved;
        public Delivery()
        {
            InitializeComponent();
            dtpDeliveryDate.MaxDate = DateTime.Now.Date;
            loadReceived();
            LoadSupplier();
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
                cmbCompanyName.SelectedIndex = -1;
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
        private void loadReceived()
        {
            try
            {
                ConnectionModule.openCon();

                // 🔹 Piliin lang yung may Role = 'Admin' o 'Staff'
                string query = "SELECT UserID, FullName FROM Users WHERE Role IN ('Admin', 'Staff')";
                MySqlCommand cmd = new MySqlCommand(query, ConnectionModule.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbReceivedBy.DisplayMember = "FullName";
                cmbReceivedBy.ValueMember = "UserID";
                cmbReceivedBy.DataSource = dt;
                cmbReceivedBy.SelectedIndex = -1; // walang default na selected
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
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
     

        private void Delivery_Load(object sender, EventArgs e)
        {
            txtDeliveryReceipt.KeyPress += DigitsOnly_KeyPress;

            txtDeliveryReceipt.ShortcutsEnabled = false;
            txtDeliveryReceipt.ContextMenu = new ContextMenu();
        }
        // 🔹 Allow only letters and digits (for IDNumber), no special chars
        // 🔹 Allow only digits
        private void DigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
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

            // Validate CompanyName: Ensure a company is selected
            if (cmbReceivedBy.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Receiver Name.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCompanyName.Focus();
                return;
            }

            // ---------------- Save Delivery ----------------
            try
            {
                con.Open();
                using (MySqlTransaction transaction = con.BeginTransaction())
                {
                    // ✅ Validate Delivery Receipt (must be unique per Supplier)
                    string checkQuery = @"
SELECT COUNT(*) 
FROM delivery 
WHERE LOWER(DeliveryReceipt) = LOWER(@Receipt) 
  AND SupplierID = @Supplier";

                    using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, con, transaction))
                    {
                        cmdCheck.Parameters.AddWithValue("@Receipt", txtDeliveryReceipt.Text.Trim());
                        cmdCheck.Parameters.AddWithValue("@Supplier", cmbCompanyName.SelectedValue);

                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar() ?? 0);
                        if (count > 0)
                        {
                            MessageBox.Show(
                                "Delivery Receipt number already exists for this Supplier. Please enter a unique one.",
                                "Duplicate Entry",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            transaction.Rollback();
                            txtDeliveryReceipt.Focus();
                            return;
                        }
                    }

           

                    transaction.Commit();
                    //MessageBox.Show("Delivery saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving delivery: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }



            DeliveryReceipt = txtDeliveryReceipt.Text.Trim();
            CompanyName = cmbCompanyName.Text.Trim();
            DateDelivered = dtpDeliveryDate.Value.Date;
            ReceivedBy = cmbReceivedBy.Text.Trim();

            DeliverySaved?.Invoke(this, EventArgs.Empty);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
