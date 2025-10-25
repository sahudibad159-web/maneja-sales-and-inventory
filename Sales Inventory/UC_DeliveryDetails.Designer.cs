namespace Sales_Inventory
{
    partial class UC_DeliveryDetails
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvDeliveryDetails = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.txtSearchDelivery = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPrintPDF = new Guna.UI2.WinForms.Guna2Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvDeliveries = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveryDetails)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveries)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDeliveryDetails
            // 
            this.dgvDeliveryDetails.AllowUserToResizeColumns = false;
            this.dgvDeliveryDetails.AllowUserToResizeRows = false;
            this.dgvDeliveryDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeliveryDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDeliveryDetails.Location = new System.Drawing.Point(0, 0);
            this.dgvDeliveryDetails.Name = "dgvDeliveryDetails";
            this.dgvDeliveryDetails.ReadOnly = true;
            this.dgvDeliveryDetails.RowHeadersWidth = 51;
            this.dgvDeliveryDetails.RowTemplate.Height = 24;
            this.dgvDeliveryDetails.Size = new System.Drawing.Size(1254, 340);
            this.dgvDeliveryDetails.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 809);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1254, 10);
            this.panel2.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 27);
            this.label1.TabIndex = 79;
            this.label1.Text = "Search Delivery:";
            // 
            // dtDate
            // 
            this.dtDate.BackColor = System.Drawing.Color.White;
            this.dtDate.Checked = true;
            this.dtDate.FillColor = System.Drawing.SystemColors.HighlightText;
            this.dtDate.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDate.Location = new System.Drawing.Point(569, 10);
            this.dtDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(215, 42);
            this.dtDate.TabIndex = 80;
            this.dtDate.Value = new System.DateTime(2025, 10, 12, 17, 2, 57, 63);
            this.dtDate.ValueChanged += new System.EventHandler(this.dtDate_ValueChanged);
            // 
            // txtSearchDelivery
            // 
            this.txtSearchDelivery.BorderColor = System.Drawing.Color.Black;
            this.txtSearchDelivery.BorderRadius = 11;
            this.txtSearchDelivery.BorderThickness = 2;
            this.txtSearchDelivery.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchDelivery.DefaultText = "";
            this.txtSearchDelivery.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearchDelivery.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearchDelivery.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchDelivery.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchDelivery.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchDelivery.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchDelivery.ForeColor = System.Drawing.Color.Black;
            this.txtSearchDelivery.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchDelivery.Location = new System.Drawing.Point(221, 19);
            this.txtSearchDelivery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearchDelivery.Name = "txtSearchDelivery";
            this.txtSearchDelivery.PlaceholderText = "";
            this.txtSearchDelivery.SelectedText = "";
            this.txtSearchDelivery.Size = new System.Drawing.Size(201, 33);
            this.txtSearchDelivery.TabIndex = 81;
            this.txtSearchDelivery.TextChanged += new System.EventHandler(this.txtSearchDelivery_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(471, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 27);
            this.label2.TabIndex = 82;
            this.label2.Text = "Date:";
            // 
            // btnPrintPDF
            // 
            this.btnPrintPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintPDF.BorderRadius = 11;
            this.btnPrintPDF.BorderThickness = 2;
            this.btnPrintPDF.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPrintPDF.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPrintPDF.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPrintPDF.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPrintPDF.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.btnPrintPDF.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintPDF.ForeColor = System.Drawing.Color.White;
            this.btnPrintPDF.Location = new System.Drawing.Point(1095, 10);
            this.btnPrintPDF.Name = "btnPrintPDF";
            this.btnPrintPDF.Size = new System.Drawing.Size(156, 35);
            this.btnPrintPDF.TabIndex = 83;
            this.btnPrintPDF.Text = "Print PDF";
            this.btnPrintPDF.Click += new System.EventHandler(this.btnPrintPDF_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvDeliveries);
            this.panel1.Controls.Add(this.btnPrintPDF);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtSearchDelivery);
            this.panel1.Controls.Add(this.dtDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1254, 421);
            this.panel1.TabIndex = 84;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // dgvDeliveries
            // 
            this.dgvDeliveries.AllowUserToResizeColumns = false;
            this.dgvDeliveries.AllowUserToResizeRows = false;
            this.dgvDeliveries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeliveries.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvDeliveries.Location = new System.Drawing.Point(0, 83);
            this.dgvDeliveries.Name = "dgvDeliveries";
            this.dgvDeliveries.ReadOnly = true;
            this.dgvDeliveries.RowHeadersWidth = 51;
            this.dgvDeliveries.RowTemplate.Height = 24;
            this.dgvDeliveries.Size = new System.Drawing.Size(1254, 338);
            this.dgvDeliveries.TabIndex = 0;
            this.dgvDeliveries.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeliveries_CellClick);
            this.dgvDeliveries.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeliveryDetails_CellDoubleClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvDeliveryDetails);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 469);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1254, 340);
            this.panel3.TabIndex = 85;
            // 
            // UC_DeliveryDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UC_DeliveryDetails";
            this.Size = new System.Drawing.Size(1254, 819);
            this.Load += new System.EventHandler(this.UC_DeliveryDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveryDetails)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveries)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvDeliveryDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtDate;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchDelivery;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button btnPrintPDF;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvDeliveries;
        private System.Windows.Forms.Panel panel3;
    }
}
