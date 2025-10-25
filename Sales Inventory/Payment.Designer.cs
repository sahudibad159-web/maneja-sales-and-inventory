namespace Sales_Inventory
{
    partial class Payment
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelCash = new System.Windows.Forms.Panel();
            this.txtChange = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtCash = new Guna.UI2.WinForms.Guna2TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelGcash = new System.Windows.Forms.Panel();
            this.txtGcash = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGcash = new System.Windows.Forms.Label();
            this.lblFullname = new System.Windows.Forms.Label();
            this.lblReference = new System.Windows.Forms.Label();
            this.txtFullname = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtReference = new Guna.UI2.WinForms.Guna2TextBox();
            this.PanelTop = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbPayment = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnProceed = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panelCash.SuspendLayout();
            this.panelGcash.SuspendLayout();
            this.PanelTop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.panelCash);
            this.panel1.Controls.Add(this.panelGcash);
            this.panel1.Controls.Add(this.PanelTop);
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(538, 382);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panelCash
            // 
            this.panelCash.Controls.Add(this.txtChange);
            this.panelCash.Controls.Add(this.txtCash);
            this.panelCash.Controls.Add(this.label6);
            this.panelCash.Controls.Add(this.label5);
            this.panelCash.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCash.Location = new System.Drawing.Point(0, 262);
            this.panelCash.Name = "panelCash";
            this.panelCash.Size = new System.Drawing.Size(538, 131);
            this.panelCash.TabIndex = 83;
            this.panelCash.Paint += new System.Windows.Forms.PaintEventHandler(this.panelCash_Paint);
            // 
            // txtChange
            // 
            this.txtChange.BorderColor = System.Drawing.Color.Black;
            this.txtChange.BorderRadius = 11;
            this.txtChange.BorderThickness = 2;
            this.txtChange.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChange.DefaultText = "";
            this.txtChange.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtChange.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtChange.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtChange.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtChange.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtChange.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChange.ForeColor = System.Drawing.Color.Black;
            this.txtChange.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtChange.Location = new System.Drawing.Point(249, 66);
            this.txtChange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChange.Name = "txtChange";
            this.txtChange.PlaceholderText = "";
            this.txtChange.ReadOnly = true;
            this.txtChange.SelectedText = "";
            this.txtChange.Size = new System.Drawing.Size(217, 48);
            this.txtChange.TabIndex = 80;
            // 
            // txtCash
            // 
            this.txtCash.BorderColor = System.Drawing.Color.Black;
            this.txtCash.BorderRadius = 11;
            this.txtCash.BorderThickness = 2;
            this.txtCash.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCash.DefaultText = "";
            this.txtCash.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCash.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCash.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCash.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCash.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCash.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCash.ForeColor = System.Drawing.Color.Black;
            this.txtCash.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCash.Location = new System.Drawing.Point(249, 10);
            this.txtCash.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCash.MaxLength = 15;
            this.txtCash.Name = "txtCash";
            this.txtCash.PlaceholderText = "";
            this.txtCash.SelectedText = "";
            this.txtCash.Size = new System.Drawing.Size(217, 48);
            this.txtCash.TabIndex = 79;
            this.txtCash.TextChanged += new System.EventHandler(this.txtCash_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(86, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 23);
            this.label6.TabIndex = 95;
            this.label6.Text = "Change:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(100, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 23);
            this.label5.TabIndex = 93;
            this.label5.Text = "Cash:";
            // 
            // panelGcash
            // 
            this.panelGcash.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelGcash.Controls.Add(this.txtGcash);
            this.panelGcash.Controls.Add(this.lblGcash);
            this.panelGcash.Controls.Add(this.lblFullname);
            this.panelGcash.Controls.Add(this.lblReference);
            this.panelGcash.Controls.Add(this.txtFullname);
            this.panelGcash.Controls.Add(this.txtReference);
            this.panelGcash.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGcash.Location = new System.Drawing.Point(0, 53);
            this.panelGcash.Name = "panelGcash";
            this.panelGcash.Size = new System.Drawing.Size(538, 209);
            this.panelGcash.TabIndex = 84;
            // 
            // txtGcash
            // 
            this.txtGcash.BorderColor = System.Drawing.Color.Black;
            this.txtGcash.BorderRadius = 11;
            this.txtGcash.BorderThickness = 2;
            this.txtGcash.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGcash.DefaultText = "";
            this.txtGcash.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtGcash.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtGcash.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGcash.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGcash.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGcash.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGcash.ForeColor = System.Drawing.Color.Black;
            this.txtGcash.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGcash.Location = new System.Drawing.Point(249, 129);
            this.txtGcash.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGcash.MaxLength = 13;
            this.txtGcash.Name = "txtGcash";
            this.txtGcash.PlaceholderText = "";
            this.txtGcash.SelectedText = "";
            this.txtGcash.Size = new System.Drawing.Size(217, 48);
            this.txtGcash.TabIndex = 78;
            this.txtGcash.TextChanged += new System.EventHandler(this.txtGcash_TextChanged);
            // 
            // lblGcash
            // 
            this.lblGcash.AutoSize = true;
            this.lblGcash.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGcash.Location = new System.Drawing.Point(97, 154);
            this.lblGcash.Name = "lblGcash";
            this.lblGcash.Size = new System.Drawing.Size(80, 23);
            this.lblGcash.TabIndex = 97;
            this.lblGcash.Text = "Gcash:";
            // 
            // lblFullname
            // 
            this.lblFullname.AutoSize = true;
            this.lblFullname.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullname.Location = new System.Drawing.Point(86, 31);
            this.lblFullname.Name = "lblFullname";
            this.lblFullname.Size = new System.Drawing.Size(108, 23);
            this.lblFullname.TabIndex = 96;
            this.lblFullname.Text = "FullName:";
            // 
            // lblReference
            // 
            this.lblReference.AutoSize = true;
            this.lblReference.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReference.Location = new System.Drawing.Point(29, 91);
            this.lblReference.Name = "lblReference";
            this.lblReference.Size = new System.Drawing.Size(201, 23);
            this.lblReference.TabIndex = 95;
            this.lblReference.Text = "Reference Number:";
            // 
            // txtFullname
            // 
            this.txtFullname.BorderColor = System.Drawing.Color.Black;
            this.txtFullname.BorderRadius = 11;
            this.txtFullname.BorderThickness = 2;
            this.txtFullname.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFullname.DefaultText = "";
            this.txtFullname.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtFullname.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtFullname.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtFullname.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtFullname.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtFullname.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFullname.ForeColor = System.Drawing.Color.Black;
            this.txtFullname.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtFullname.Location = new System.Drawing.Point(249, 6);
            this.txtFullname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFullname.MaxLength = 20;
            this.txtFullname.Name = "txtFullname";
            this.txtFullname.PlaceholderText = "";
            this.txtFullname.SelectedText = "";
            this.txtFullname.Size = new System.Drawing.Size(217, 48);
            this.txtFullname.TabIndex = 76;
            // 
            // txtReference
            // 
            this.txtReference.BorderColor = System.Drawing.Color.Black;
            this.txtReference.BorderRadius = 11;
            this.txtReference.BorderThickness = 2;
            this.txtReference.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtReference.DefaultText = "";
            this.txtReference.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtReference.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtReference.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtReference.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtReference.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtReference.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReference.ForeColor = System.Drawing.Color.Black;
            this.txtReference.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtReference.Location = new System.Drawing.Point(249, 66);
            this.txtReference.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtReference.MaxLength = 20;
            this.txtReference.Name = "txtReference";
            this.txtReference.PlaceholderText = "";
            this.txtReference.SelectedText = "";
            this.txtReference.Size = new System.Drawing.Size(217, 48);
            this.txtReference.TabIndex = 77;
            // 
            // PanelTop
            // 
            this.PanelTop.Controls.Add(this.label4);
            this.PanelTop.Controls.Add(this.cmbPayment);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(538, 53);
            this.PanelTop.TabIndex = 83;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(45, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 23);
            this.label4.TabIndex = 99;
            this.label4.Text = "Payment Method:";
            // 
            // cmbPayment
            // 
            this.cmbPayment.AutoRoundedCorners = true;
            this.cmbPayment.BackColor = System.Drawing.Color.Transparent;
            this.cmbPayment.BorderColor = System.Drawing.Color.Black;
            this.cmbPayment.BorderRadius = 17;
            this.cmbPayment.BorderThickness = 2;
            this.cmbPayment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPayment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPayment.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPayment.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPayment.Font = new System.Drawing.Font("Century Gothic", 13.8F);
            this.cmbPayment.ForeColor = System.Drawing.Color.Black;
            this.cmbPayment.ItemHeight = 30;
            this.cmbPayment.Location = new System.Drawing.Point(249, 6);
            this.cmbPayment.Name = "cmbPayment";
            this.cmbPayment.Size = new System.Drawing.Size(217, 36);
            this.cmbPayment.TabIndex = 75;
            this.cmbPayment.SelectedIndexChanged += new System.EventHandler(this.cmbPayment_SelectedIndexChanged_1);
            // 
            // btnProceed
            // 
            this.btnProceed.BorderRadius = 11;
            this.btnProceed.BorderThickness = 2;
            this.btnProceed.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnProceed.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnProceed.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnProceed.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnProceed.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.btnProceed.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.Location = new System.Drawing.Point(4, 449);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(177, 47);
            this.btnProceed.TabIndex = 81;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 11;
            this.btnCancel.BorderThickness = 2;
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.btnCancel.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(218, 449);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(177, 47);
            this.btnCancel.TabIndex = 170;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(538, 54);
            this.panel2.TabIndex = 172;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(109, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(319, 37);
            this.label1.TabIndex = 77;
            this.label1.Text = "Payment Transaction";
            // 
            // animationTimer
            // 
            this.animationTimer.Interval = 15;
            this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 506);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(538, 10);
            this.panel3.TabIndex = 173;
            // 
            // Payment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(538, 516);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Payment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payment";
            this.Load += new System.EventHandler(this.Payment_Load);
            this.panel1.ResumeLayout(false);
            this.panelCash.ResumeLayout(false);
            this.panelCash.PerformLayout();
            this.panelGcash.ResumeLayout(false);
            this.panelGcash.PerformLayout();
            this.PanelTop.ResumeLayout(false);
            this.PanelTop.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnProceed;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer animationTimer;
        private System.Windows.Forms.Panel PanelTop;
        private System.Windows.Forms.Panel panelGcash;
        private System.Windows.Forms.Panel panelCash;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPayment;
        private Guna.UI2.WinForms.Guna2TextBox txtChange;
        private Guna.UI2.WinForms.Guna2TextBox txtCash;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2TextBox txtGcash;
        private System.Windows.Forms.Label lblGcash;
        private System.Windows.Forms.Label lblFullname;
        private System.Windows.Forms.Label lblReference;
        private Guna.UI2.WinForms.Guna2TextBox txtFullname;
        private Guna.UI2.WinForms.Guna2TextBox txtReference;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
    }
}