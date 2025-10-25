namespace Sales_Inventory
{
    partial class StockReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvStockReport = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkShowOut = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDamageItem = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockReport)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvStockReport
            // 
            this.dgvStockReport.AllowUserToResizeColumns = false;
            this.dgvStockReport.AllowUserToResizeRows = false;
            this.dgvStockReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStockReport.Location = new System.Drawing.Point(12, 117);
            this.dgvStockReport.Name = "dgvStockReport";
            this.dgvStockReport.ReadOnly = true;
            this.dgvStockReport.RowHeadersWidth = 51;
            this.dgvStockReport.RowTemplate.Height = 24;
            this.dgvStockReport.Size = new System.Drawing.Size(1337, 608);
            this.dgvStockReport.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(581, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 37);
            this.label1.TabIndex = 78;
            this.label1.Text = "Stock Report";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1361, 78);
            this.panel1.TabIndex = 10;
            // 
            // chkShowOut
            // 
            this.chkShowOut.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkShowOut.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkShowOut.CheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.chkShowOut.CheckedState.InnerColor = System.Drawing.Color.White;
            this.chkShowOut.Location = new System.Drawing.Point(1263, 84);
            this.chkShowOut.Name = "chkShowOut";
            this.chkShowOut.Size = new System.Drawing.Size(86, 27);
            this.chkShowOut.TabIndex = 12;
            this.chkShowOut.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkShowOut.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkShowOut.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.chkShowOut.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.chkShowOut.CheckedChanged += new System.EventHandler(this.guna2ToggleSwitch1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1172, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "IN/OUT";
            // 
            // btnDamageItem
            // 
            this.btnDamageItem.BorderRadius = 11;
            this.btnDamageItem.BorderThickness = 2;
            this.btnDamageItem.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDamageItem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDamageItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDamageItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDamageItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.btnDamageItem.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDamageItem.ForeColor = System.Drawing.Color.White;
            this.btnDamageItem.Location = new System.Drawing.Point(12, 731);
            this.btnDamageItem.Name = "btnDamageItem";
            this.btnDamageItem.Size = new System.Drawing.Size(228, 33);
            this.btnDamageItem.TabIndex = 14;
            this.btnDamageItem.Text = "Damage Product";
            this.btnDamageItem.Click += new System.EventHandler(this.btnDamageItem_Click);
            // 
            // StockReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1361, 764);
            this.Controls.Add(this.btnDamageItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkShowOut);
            this.Controls.Add(this.dgvStockReport);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StockReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StockReport";
            this.Load += new System.EventHandler(this.StockReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockReport)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStockReport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkShowOut;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button btnDamageItem;
    }
}