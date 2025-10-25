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
    public partial class Reports : UserControl
    {
        public Reports()
        {
            InitializeComponent();
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
    }
}
