using Mysqlx.Cursor;
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
    public partial class Maintenance : UserControl
    {
        public Maintenance()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (Vat regForm = new Vat())
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
                MessageBox.Show("Error opening Vat form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (DiscountMaintenance regForm = new DiscountMaintenance())
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
                MessageBox.Show("Error opening DiscountMaintenance form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (UserRegistration regForm = new UserRegistration())
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
                MessageBox.Show("Error opening DiscountMaintenance form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (MemberYarn regForm = new MemberYarn())
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
                MessageBox.Show("Error opening Members form: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
