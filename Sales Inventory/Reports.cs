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
    public partial class Reports : UserControl
    {
        private string _role;

        public Reports(string role)
        {
            InitializeComponent();
            _role = role;
            SetPermissions();

        }
        private void SetPermissions()
        {
            if (_role == "Staff")
            {
                // Staff lang makikita
                guna2Button1.Visible = true;  // ExpiredProduct
                guna2Button2.Visible = true;  // NearlyExpired
                guna2Button4.Visible = true;  // Critical

                // Hidden sa staff
                guna2Button3.Visible = false; // StockReport
                BtnShift_Logs.Visible = false;
                LogInLogs.Visible = false;
                btnVat.Visible = false;       // AuditTrail
            }
            else if (_role == "Admin")
            {
                // Admin makikita lahat
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Visible = true;
                }
            }
        }
        private void btnVat_Click(object sender, EventArgs e)
        {
            try
            {
                using (AuditTrail regForm = new AuditTrail())
                {
                    var result = regForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // dito na lang optional refresh kung kailangan
                        // LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening AuditTrail form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (ExpiredProduct regForm = new ExpiredProduct())
                {
                    var result = regForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // dito na lang optional refresh kung kailangan
                        // LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening ExpiredProduct form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (NearlyExpired regForm = new NearlyExpired())
                {
                    var result = regForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // dito na lang optional refresh kung kailangan
                        // LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening NearlyExpired form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (StockReport regForm = new StockReport())
                {
                    var result = regForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // dito na lang optional refresh kung kailangan
                        // LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening StockReport form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnShift_Logs_Click(object sender, EventArgs e)
        {
            try
            {
                using (ShiftLogs regForm = new ShiftLogs())
                {
                    var result = regForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // dito na lang optional refresh kung kailangan
                        // LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening ShiftLogs form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogInLogs_Click(object sender, EventArgs e)
        {
            try
            {
                using (LogInLogs regForm = new LogInLogs())
                {
                    var result = regForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // dito na lang optional refresh kung kailangan
                        // LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening LogInLogs form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    using (CriticalStock regForm = new CriticalStock())
            //    {
            //        var result = regForm.ShowDialog();

            //        if (result == DialogResult.OK)
            //        {
            //            // dito na lang optional refresh kung kailangan
            //            // LoadMembers();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error opening CriticalStock form: " + ex.Message,
            //                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (Critical regForm = new Critical())
                {
                    var result = regForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // dito na lang optional refresh kung kailangan
                        // LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening LogInLogs form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
