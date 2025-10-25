namespace Sales_Inventory
{
    partial class ViewProduct
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
            this.dgvVieewProduct = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.txtSearchInventoryProduct = new Guna.UI2.WinForms.Guna2TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVieewProduct)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvVieewProduct
            // 
            this.dgvVieewProduct.AllowUserToResizeColumns = false;
            this.dgvVieewProduct.AllowUserToResizeRows = false;
            this.dgvVieewProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVieewProduct.Location = new System.Drawing.Point(12, 115);
            this.dgvVieewProduct.Name = "dgvVieewProduct";
            this.dgvVieewProduct.ReadOnly = true;
            this.dgvVieewProduct.RowHeadersWidth = 51;
            this.dgvVieewProduct.RowTemplate.Height = 24;
            this.dgvVieewProduct.Size = new System.Drawing.Size(813, 335);
            this.dgvVieewProduct.TabIndex = 0;
            this.dgvVieewProduct.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVieewProduct_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.guna2ControlBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(837, 50);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(315, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(217, 34);
            this.label2.TabIndex = 79;
            this.label2.Text = "View Inventory";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 469);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(837, 10);
            this.panel2.TabIndex = 58;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.FillColor = System.Drawing.Color.IndianRed;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(805, 0);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(32, 21);
            this.guna2ControlBox1.TabIndex = 80;
            // 
            // txtSearchInventoryProduct
            // 
            this.txtSearchInventoryProduct.BorderColor = System.Drawing.Color.Black;
            this.txtSearchInventoryProduct.BorderRadius = 11;
            this.txtSearchInventoryProduct.BorderThickness = 2;
            this.txtSearchInventoryProduct.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchInventoryProduct.DefaultText = "";
            this.txtSearchInventoryProduct.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearchInventoryProduct.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearchInventoryProduct.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchInventoryProduct.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchInventoryProduct.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchInventoryProduct.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchInventoryProduct.ForeColor = System.Drawing.Color.Black;
            this.txtSearchInventoryProduct.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchInventoryProduct.Location = new System.Drawing.Point(217, 57);
            this.txtSearchInventoryProduct.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearchInventoryProduct.Name = "txtSearchInventoryProduct";
            this.txtSearchInventoryProduct.PlaceholderText = "";
            this.txtSearchInventoryProduct.SelectedText = "";
            this.txtSearchInventoryProduct.Size = new System.Drawing.Size(201, 33);
            this.txtSearchInventoryProduct.TabIndex = 185;
            this.txtSearchInventoryProduct.TextChanged += new System.EventHandler(this.txtSearchInventoryProduct_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(10, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(191, 27);
            this.label9.TabIndex = 184;
            this.label9.Text = "Search Product:";
            // 
            // ViewProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 479);
            this.Controls.Add(this.txtSearchInventoryProduct);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvVieewProduct);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ViewProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewProduct";
            this.Load += new System.EventHandler(this.ViewProduct_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVieewProduct)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVieewProduct;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchInventoryProduct;
        private System.Windows.Forms.Label label9;
    }
}