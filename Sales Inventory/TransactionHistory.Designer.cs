namespace Sales_Inventory
{
    partial class TransactionHistory
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
            this.dgvTransactions = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvTransactionItems = new System.Windows.Forms.DataGridView();
            this.btnVoidItem = new Guna.UI2.WinForms.Guna2Button();
            this.btnVoidTransaction = new Guna.UI2.WinForms.Guna2Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dtDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionItems)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTransactions
            // 
            this.dgvTransactions.AllowUserToResizeColumns = false;
            this.dgvTransactions.AllowUserToResizeRows = false;
            this.dgvTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactions.Location = new System.Drawing.Point(14, 12);
            this.dgvTransactions.Name = "dgvTransactions";
            this.dgvTransactions.ReadOnly = true;
            this.dgvTransactions.RowHeadersWidth = 51;
            this.dgvTransactions.RowTemplate.Height = 24;
            this.dgvTransactions.Size = new System.Drawing.Size(1100, 281);
            this.dgvTransactions.TabIndex = 0;
            this.dgvTransactions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransactions_CellContentClick);
            this.dgvTransactions.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTransactions_DataBindingComplete);
            this.dgvTransactions.SelectionChanged += new System.EventHandler(this.dgvTransactions_SelectionChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.dgvTransactions);
            this.panel1.Location = new System.Drawing.Point(12, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1131, 308);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.dgvTransactionItems);
            this.panel2.Location = new System.Drawing.Point(12, 452);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1131, 308);
            this.panel2.TabIndex = 2;
            // 
            // dgvTransactionItems
            // 
            this.dgvTransactionItems.AllowUserToResizeColumns = false;
            this.dgvTransactionItems.AllowUserToResizeRows = false;
            this.dgvTransactionItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactionItems.Location = new System.Drawing.Point(14, 15);
            this.dgvTransactionItems.Name = "dgvTransactionItems";
            this.dgvTransactionItems.ReadOnly = true;
            this.dgvTransactionItems.RowHeadersWidth = 51;
            this.dgvTransactionItems.RowTemplate.Height = 24;
            this.dgvTransactionItems.Size = new System.Drawing.Size(1100, 281);
            this.dgvTransactionItems.TabIndex = 0;
            this.dgvTransactionItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransactionItems_CellContentClick);
            this.dgvTransactionItems.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTransactionItems_DataBindingComplete_1);
            // 
            // btnVoidItem
            // 
            this.btnVoidItem.BorderRadius = 11;
            this.btnVoidItem.BorderThickness = 2;
            this.btnVoidItem.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnVoidItem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnVoidItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnVoidItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnVoidItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.btnVoidItem.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoidItem.ForeColor = System.Drawing.Color.White;
            this.btnVoidItem.Location = new System.Drawing.Point(301, 766);
            this.btnVoidItem.Name = "btnVoidItem";
            this.btnVoidItem.Size = new System.Drawing.Size(180, 45);
            this.btnVoidItem.TabIndex = 4;
            this.btnVoidItem.Text = "VoidItem";
            this.btnVoidItem.Click += new System.EventHandler(this.btnVoidItem_Click);
            // 
            // btnVoidTransaction
            // 
            this.btnVoidTransaction.BorderRadius = 11;
            this.btnVoidTransaction.BorderThickness = 2;
            this.btnVoidTransaction.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnVoidTransaction.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnVoidTransaction.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnVoidTransaction.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnVoidTransaction.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.btnVoidTransaction.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoidTransaction.ForeColor = System.Drawing.Color.White;
            this.btnVoidTransaction.Location = new System.Drawing.Point(23, 766);
            this.btnVoidTransaction.Name = "btnVoidTransaction";
            this.btnVoidTransaction.Size = new System.Drawing.Size(220, 45);
            this.btnVoidTransaction.TabIndex = 3;
            this.btnVoidTransaction.Text = "VoidTransaction";
            this.btnVoidTransaction.Click += new System.EventHandler(this.btnVoidTransaction_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Controls.Add(this.guna2ControlBox1);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1166, 62);
            this.panel3.TabIndex = 5;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.BackColor = System.Drawing.SystemColors.Control;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.LightCoral;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(1124, 0);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(42, 29);
            this.guna2ControlBox1.TabIndex = 58;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(395, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(344, 44);
            this.label1.TabIndex = 1;
            this.label1.Text = "Transaction History";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 815);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1166, 10);
            this.panel4.TabIndex = 57;
            // 
            // dtDate
            // 
            this.dtDate.BackColor = System.Drawing.Color.White;
            this.dtDate.BorderThickness = 2;
            this.dtDate.Checked = true;
            this.dtDate.FillColor = System.Drawing.SystemColors.HighlightText;
            this.dtDate.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDate.Location = new System.Drawing.Point(186, 68);
            this.dtDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(215, 42);
            this.dtDate.TabIndex = 58;
            this.dtDate.Value = new System.DateTime(2025, 10, 12, 17, 2, 57, 63);
            this.dtDate.ValueChanged += new System.EventHandler(this.dtDate_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 27);
            this.label2.TabIndex = 59;
            this.label2.Text = "Search Date:";
            // 
            // TransactionHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1166, 825);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnVoidItem);
            this.Controls.Add(this.btnVoidTransaction);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TransactionHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TransactionHistory";
            this.Load += new System.EventHandler(this.TransactionHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionItems)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvTransactionItems;
        private Guna.UI2.WinForms.Guna2Button btnVoidItem;
        private Guna.UI2.WinForms.Guna2Button btnVoidTransaction;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtDate;
        private System.Windows.Forms.Label label2;
    }
}