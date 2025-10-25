using Org.BouncyCastle.Asn1.X509;
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
    public partial class VoidReason : Form
    {
        public string SelectedReason { get; private set; }
        public VoidReason(string[] reasons)
        {
            
                InitializeComponent();
                cmbReason.Items.AddRange(reasons);
                cmbReason.DropDownStyle = ComboBoxStyle.DropDownList;
              
        }

        private void VoidReason_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbReason.SelectedItem == null)
            {
                MessageBox.Show("Please select a reason.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedReason = cmbReason.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
