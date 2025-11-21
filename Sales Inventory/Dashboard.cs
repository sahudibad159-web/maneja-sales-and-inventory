using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;

namespace Sales_Inventory
{
    public partial class Dashboard : Form
    {
        private string userRole;
        public Dashboard(string Role)
        {
            InitializeComponent();
            LoadForm(new UC_Dashboard());
            userRole = Role;
            SetHeader("Dashboard");
            ApplyRoleRestrictions();


        }
        private void ApplyRoleRestrictions()
        {
            if (userRole == "Cashier")
            {
                // Only show allowed buttons
                guna2Button1.Visible = true; // Category
                guna2Button2.Visible = true; // Supplier
                guna2Button3.Visible = true; // Product
                guna2Button4.Visible = true; // Delivery
                guna2Button5.Visible = true; // Dashboard
             //   guna2Button6.Visible = true; // Inventory

                // Hide restricted ones
                guna2Button7.Visible = false; // Maintenance
                guna2Button8.Visible = false; // Reports
                guna2Button9.Visible = false; // POS
            }
            else if (userRole == "Admin")
            {
                // Admin sees everything
                guna2Button1.Visible = true;
                guna2Button2.Visible = true;
                guna2Button3.Visible = true;
                guna2Button4.Visible = true;
                guna2Button5.Visible = true;
              //  guna2Button6.Visible = true;
                guna2Button7.Visible = true;
                guna2Button8.Visible = true;
                guna2Button9.Visible = true;

            }
            else
            {
                // Default: show limited for staff
                guna2Button1.Visible = true;
                guna2Button2.Visible = true;
                guna2Button3.Visible = true;
                guna2Button4.Visible = true;
                guna2Button5.Visible = true;
             //   guna2Button6.Visible = true;

                guna2Button7.Visible = false;
                guna2Button8.Visible = false;
                guna2Button9.Visible = false;
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }
        private void LoadForm(UserControl uc)
        {
            MainPanel.Controls.Clear();  // clear muna kung may laman
            uc.Dock = DockStyle.Fill;    // para full size siya sa panel
            MainPanel.Controls.Add(uc);  // add yung usercontrol
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            LoadForm(new UC_Category());
            // Update header
            SetHeader("Category");
        }
        public void SetHeader(string title)
        {
            lvlHeader.Text = title;
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            LoadForm(new UC_Supplier());
            SetHeader("Supplier");
        }
        

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            LoadForm(new UC_Products());
            SetHeader("Product");
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            LoadForm(new UC_Delivery());
            SetHeader("Delivery");

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoadForm(new UC_Dashboard());
            SetHeader("DashBoard");

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            LoadForm(new Maintenance());
            SetHeader("Maintenance");
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            LoadForm(new Reports());
            SetHeader("Reports");
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            POS posForm = new POS(userRole);
            posForm.Show();
            this.Hide();

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            // 🟡 1. Ask first before doing anything
            DialogResult result = MessageBox.Show(
                "Are you sure you want to Log Out?",
                "Confirm Log Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
            {
                // User cancelled logout
                return;
            }

            try
            {
                ConnectionModule.openCon();

                // ✅ 2. Update the last login log for this user
                string updateLogQuery = @"
            UPDATE login_logs
            SET LogoutTime = NOW(),
                Status = 'Logged Out'
            WHERE Username = @username
              AND Status = 'Logged In'
            ORDER BY LogID DESC
            LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(updateLogQuery, ConnectionModule.con))
                {
                    cmd.Parameters.AddWithValue("@username", ConnectionModule.Session.Username);
                    cmd.ExecuteNonQuery();
                }

                // ✅ 3. Optional: Insert Audit Trail for logout
             //   ConnectionModule.InsertAuditTrail("Logout", "Users", $"User {ConnectionModule.Session.Username} logged out.");

                // ✅ 4. Go back to Login
                Loginform login = new Loginform();
                login.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during logout: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectionModule.closeCon();
            }
        }


        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            LoadForm(new Inventory());
            SetHeader("Inventory");
        }
    }
}
