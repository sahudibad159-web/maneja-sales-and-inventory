namespace Sales_Inventory
{
    partial class POS
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRegister = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtTransactionHistory = new Guna.UI2.WinForms.Guna2Button();
            this.txtEndShift = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button5 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.btnDiscount = new Guna.UI2.WinForms.Guna2Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtRedeemedPoints = new Guna.UI2.WinForms.Guna2TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTotal = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSubTotal = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtDiscount = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVatAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtVatableSales = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtVatExempt = new Guna.UI2.WinForms.Guna2TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMemberID = new Guna.UI2.WinForms.Guna2TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBarcode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtQuantity = new Guna.UI2.WinForms.Guna2TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dgvProduct = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPoints = new Guna.UI2.WinForms.Guna2TextBox();
            this.htmlLabel1 = new MetroFramework.Drawing.Html.HtmlLabel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDocument2 = new System.Drawing.Printing.PrintDocument();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.btnRegister);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1547, 64);
            this.panel1.TabIndex = 0;
            // 
            // btnRegister
            // 
            this.btnRegister.BorderRadius = 11;
            this.btnRegister.BorderThickness = 1;
            this.btnRegister.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRegister.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRegister.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRegister.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRegister.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.btnRegister.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(1252, 9);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(216, 42);
            this.btnRegister.TabIndex = 48;
            this.btnRegister.Text = "Register Member";
            this.btnRegister.Click += new System.EventHandler(this.guna2Button7_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(536, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "POINT OF SALES";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel4.Controls.Add(this.txtTransactionHistory);
            this.panel4.Controls.Add(this.txtEndShift);
            this.panel4.Controls.Add(this.guna2Button5);
            this.panel4.Controls.Add(this.guna2Button2);
            this.panel4.Controls.Add(this.guna2Button4);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.btnDiscount);
            this.panel4.Location = new System.Drawing.Point(12, 7);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(430, 232);
            this.panel4.TabIndex = 67;
            // 
            // txtTransactionHistory
            // 
            this.txtTransactionHistory.BorderRadius = 11;
            this.txtTransactionHistory.BorderThickness = 1;
            this.txtTransactionHistory.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.txtTransactionHistory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.txtTransactionHistory.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.txtTransactionHistory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.txtTransactionHistory.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.txtTransactionHistory.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransactionHistory.ForeColor = System.Drawing.Color.White;
            this.txtTransactionHistory.Location = new System.Drawing.Point(83, 177);
            this.txtTransactionHistory.Name = "txtTransactionHistory";
            this.txtTransactionHistory.Size = new System.Drawing.Size(272, 42);
            this.txtTransactionHistory.TabIndex = 47;
            this.txtTransactionHistory.Text = "(F7) Transaction History";
            this.txtTransactionHistory.Click += new System.EventHandler(this.txtTransactionHistory_Click);
            this.txtTransactionHistory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransactionHistory_KeyDown);
            // 
            // txtEndShift
            // 
            this.txtEndShift.BorderRadius = 11;
            this.txtEndShift.BorderThickness = 1;
            this.txtEndShift.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.txtEndShift.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.txtEndShift.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.txtEndShift.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.txtEndShift.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.txtEndShift.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndShift.ForeColor = System.Drawing.Color.White;
            this.txtEndShift.Location = new System.Drawing.Point(214, 129);
            this.txtEndShift.Name = "txtEndShift";
            this.txtEndShift.Size = new System.Drawing.Size(191, 42);
            this.txtEndShift.TabIndex = 46;
            this.txtEndShift.Text = "(F6) End Shift";
            this.txtEndShift.Click += new System.EventHandler(this.guna2Button3_Click);
            this.txtEndShift.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEndShift_KeyDown);
            // 
            // guna2Button5
            // 
            this.guna2Button5.BorderRadius = 11;
            this.guna2Button5.BorderThickness = 1;
            this.guna2Button5.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button5.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button5.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button5.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.guna2Button5.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button5.ForeColor = System.Drawing.Color.White;
            this.guna2Button5.Location = new System.Drawing.Point(214, 11);
            this.guna2Button5.Name = "guna2Button5";
            this.guna2Button5.Size = new System.Drawing.Size(191, 42);
            this.guna2Button5.TabIndex = 44;
            this.guna2Button5.Text = "(F2) Payment";
            this.guna2Button5.Click += new System.EventHandler(this.guna2Button5_Click);
            this.guna2Button5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.guna2Button5_KeyDown);
            // 
            // guna2Button2
            // 
            this.guna2Button2.BorderRadius = 11;
            this.guna2Button2.BorderThickness = 1;
            this.guna2Button2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.guna2Button2.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Location = new System.Drawing.Point(17, 129);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(191, 42);
            this.guna2Button2.TabIndex = 43;
            this.guna2Button2.Text = "(F5) Cancel Transaction";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            this.guna2Button2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.guna2Button2_KeyDown);
            // 
            // guna2Button4
            // 
            this.guna2Button4.BorderRadius = 11;
            this.guna2Button4.BorderThickness = 1;
            this.guna2Button4.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button4.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button4.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button4.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.guna2Button4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button4.ForeColor = System.Drawing.Color.White;
            this.guna2Button4.Location = new System.Drawing.Point(214, 68);
            this.guna2Button4.Name = "guna2Button4";
            this.guna2Button4.Size = new System.Drawing.Size(191, 42);
            this.guna2Button4.TabIndex = 45;
            this.guna2Button4.Text = "(F4) Void Item";
            this.guna2Button4.Click += new System.EventHandler(this.guna2Button4_Click);
            this.guna2Button4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.guna2Button4_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.BorderRadius = 11;
            this.btnSave.BorderThickness = 1;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.btnSave.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(17, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(191, 42);
            this.btnSave.TabIndex = 41;
            this.btnSave.Text = "(F1) Search Item";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSave_KeyDown);
            // 
            // btnDiscount
            // 
            this.btnDiscount.BorderRadius = 11;
            this.btnDiscount.BorderThickness = 1;
            this.btnDiscount.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDiscount.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDiscount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDiscount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDiscount.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(89)))), ((int)(((byte)(124)))));
            this.btnDiscount.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiscount.ForeColor = System.Drawing.Color.White;
            this.btnDiscount.Location = new System.Drawing.Point(17, 68);
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(191, 42);
            this.btnDiscount.TabIndex = 42;
            this.btnDiscount.Text = "(F3) Discount";
            this.btnDiscount.Click += new System.EventHandler(this.guna2Button1_Click);
            this.btnDiscount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnDiscount_KeyDown);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel3.Controls.Add(this.txtRedeemedPoints);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.txtTotal);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txtSubTotal);
            this.panel3.Controls.Add(this.txtDiscount);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.txtVatAmount);
            this.panel3.Controls.Add(this.txtVatableSales);
            this.panel3.Controls.Add(this.txtVatExempt);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Location = new System.Drawing.Point(6, 254);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(429, 399);
            this.panel3.TabIndex = 66;
            // 
            // txtRedeemedPoints
            // 
            this.txtRedeemedPoints.BorderColor = System.Drawing.Color.Black;
            this.txtRedeemedPoints.BorderRadius = 11;
            this.txtRedeemedPoints.BorderThickness = 2;
            this.txtRedeemedPoints.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtRedeemedPoints.DefaultText = "";
            this.txtRedeemedPoints.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtRedeemedPoints.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtRedeemedPoints.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRedeemedPoints.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRedeemedPoints.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRedeemedPoints.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRedeemedPoints.ForeColor = System.Drawing.Color.Black;
            this.txtRedeemedPoints.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRedeemedPoints.Location = new System.Drawing.Point(186, 135);
            this.txtRedeemedPoints.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRedeemedPoints.Name = "txtRedeemedPoints";
            this.txtRedeemedPoints.PlaceholderText = "";
            this.txtRedeemedPoints.ReadOnly = true;
            this.txtRedeemedPoints.SelectedText = "";
            this.txtRedeemedPoints.Size = new System.Drawing.Size(216, 40);
            this.txtRedeemedPoints.TabIndex = 60;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(2, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(182, 23);
            this.label8.TabIndex = 59;
            this.label8.Text = "Redeemed Points";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // txtTotal
            // 
            this.txtTotal.BorderColor = System.Drawing.Color.Black;
            this.txtTotal.BorderRadius = 11;
            this.txtTotal.BorderThickness = 2;
            this.txtTotal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotal.DefaultText = "";
            this.txtTotal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTotal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTotal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotal.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.ForeColor = System.Drawing.Color.Black;
            this.txtTotal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotal.Location = new System.Drawing.Point(188, 325);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.PlaceholderText = "";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.SelectedText = "";
            this.txtTotal.Size = new System.Drawing.Size(216, 40);
            this.txtTotal.TabIndex = 56;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 27);
            this.label3.TabIndex = 41;
            this.label3.Text = "Discount:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(38, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sub Total:";
            // 
            // txtSubTotal
            // 
            this.txtSubTotal.BorderColor = System.Drawing.Color.Black;
            this.txtSubTotal.BorderRadius = 11;
            this.txtSubTotal.BorderThickness = 2;
            this.txtSubTotal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSubTotal.DefaultText = "";
            this.txtSubTotal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSubTotal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSubTotal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSubTotal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSubTotal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSubTotal.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubTotal.ForeColor = System.Drawing.Color.Black;
            this.txtSubTotal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSubTotal.Location = new System.Drawing.Point(188, 39);
            this.txtSubTotal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSubTotal.Name = "txtSubTotal";
            this.txtSubTotal.PlaceholderText = "";
            this.txtSubTotal.ReadOnly = true;
            this.txtSubTotal.SelectedText = "";
            this.txtSubTotal.Size = new System.Drawing.Size(216, 40);
            this.txtSubTotal.TabIndex = 42;
            // 
            // txtDiscount
            // 
            this.txtDiscount.BorderColor = System.Drawing.Color.Black;
            this.txtDiscount.BorderRadius = 11;
            this.txtDiscount.BorderThickness = 2;
            this.txtDiscount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDiscount.DefaultText = "";
            this.txtDiscount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtDiscount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtDiscount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDiscount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDiscount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDiscount.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscount.ForeColor = System.Drawing.Color.Black;
            this.txtDiscount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDiscount.Location = new System.Drawing.Point(186, 87);
            this.txtDiscount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.PlaceholderText = "";
            this.txtDiscount.ReadOnly = true;
            this.txtDiscount.SelectedText = "";
            this.txtDiscount.Size = new System.Drawing.Size(216, 40);
            this.txtDiscount.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 23);
            this.label4.TabIndex = 43;
            this.label4.Text = "VatTableSales:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 242);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 27);
            this.label5.TabIndex = 45;
            this.label5.Text = "VAT Amount:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(11, 290);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(153, 27);
            this.label6.TabIndex = 47;
            this.label6.Text = "VAT Exempt:";
            // 
            // txtVatAmount
            // 
            this.txtVatAmount.BorderColor = System.Drawing.Color.Black;
            this.txtVatAmount.BorderRadius = 11;
            this.txtVatAmount.BorderThickness = 2;
            this.txtVatAmount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVatAmount.DefaultText = "";
            this.txtVatAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtVatAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtVatAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtVatAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtVatAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVatAmount.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVatAmount.ForeColor = System.Drawing.Color.Black;
            this.txtVatAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVatAmount.Location = new System.Drawing.Point(188, 229);
            this.txtVatAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtVatAmount.Name = "txtVatAmount";
            this.txtVatAmount.PlaceholderText = "";
            this.txtVatAmount.ReadOnly = true;
            this.txtVatAmount.SelectedText = "";
            this.txtVatAmount.Size = new System.Drawing.Size(216, 40);
            this.txtVatAmount.TabIndex = 48;
            // 
            // txtVatableSales
            // 
            this.txtVatableSales.BorderColor = System.Drawing.Color.Black;
            this.txtVatableSales.BorderRadius = 11;
            this.txtVatableSales.BorderThickness = 2;
            this.txtVatableSales.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVatableSales.DefaultText = "";
            this.txtVatableSales.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtVatableSales.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtVatableSales.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtVatableSales.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtVatableSales.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVatableSales.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVatableSales.ForeColor = System.Drawing.Color.Black;
            this.txtVatableSales.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVatableSales.Location = new System.Drawing.Point(188, 181);
            this.txtVatableSales.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtVatableSales.Name = "txtVatableSales";
            this.txtVatableSales.PlaceholderText = "";
            this.txtVatableSales.ReadOnly = true;
            this.txtVatableSales.SelectedText = "";
            this.txtVatableSales.Size = new System.Drawing.Size(216, 40);
            this.txtVatableSales.TabIndex = 50;
            // 
            // txtVatExempt
            // 
            this.txtVatExempt.BorderColor = System.Drawing.Color.Black;
            this.txtVatExempt.BorderRadius = 11;
            this.txtVatExempt.BorderThickness = 2;
            this.txtVatExempt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVatExempt.DefaultText = "";
            this.txtVatExempt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtVatExempt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtVatExempt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtVatExempt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtVatExempt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVatExempt.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVatExempt.ForeColor = System.Drawing.Color.Black;
            this.txtVatExempt.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVatExempt.Location = new System.Drawing.Point(188, 277);
            this.txtVatExempt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtVatExempt.Name = "txtVatExempt";
            this.txtVatExempt.PlaceholderText = "";
            this.txtVatExempt.ReadOnly = true;
            this.txtVatExempt.SelectedText = "";
            this.txtVatExempt.Size = new System.Drawing.Size(216, 40);
            this.txtVatExempt.TabIndex = 49;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(60, 338);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 27);
            this.label9.TabIndex = 49;
            this.label9.Text = "Total:";
            // 
            // txtMemberID
            // 
            this.txtMemberID.BorderColor = System.Drawing.Color.Black;
            this.txtMemberID.BorderRadius = 11;
            this.txtMemberID.BorderThickness = 2;
            this.txtMemberID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMemberID.DefaultText = "";
            this.txtMemberID.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtMemberID.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtMemberID.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtMemberID.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtMemberID.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtMemberID.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemberID.ForeColor = System.Drawing.Color.Black;
            this.txtMemberID.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtMemberID.Location = new System.Drawing.Point(182, 60);
            this.txtMemberID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMemberID.MaxLength = 10;
            this.txtMemberID.Name = "txtMemberID";
            this.txtMemberID.PlaceholderText = "";
            this.txtMemberID.SelectedText = "";
            this.txtMemberID.Size = new System.Drawing.Size(216, 40);
            this.txtMemberID.TabIndex = 58;
            this.txtMemberID.TextChanged += new System.EventHandler(this.guna2TextBox9_TextChanged);
            this.txtMemberID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMemberID_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(19, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(145, 27);
            this.label10.TabIndex = 57;
            this.label10.Text = "Member ID:";
            // 
            // txtBarcode
            // 
            this.txtBarcode.BorderColor = System.Drawing.Color.Black;
            this.txtBarcode.BorderRadius = 11;
            this.txtBarcode.BorderThickness = 2;
            this.txtBarcode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBarcode.DefaultText = "";
            this.txtBarcode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBarcode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBarcode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBarcode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBarcode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBarcode.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.ForeColor = System.Drawing.Color.Black;
            this.txtBarcode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBarcode.Location = new System.Drawing.Point(182, 11);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.PlaceholderText = "";
            this.txtBarcode.SelectedText = "";
            this.txtBarcode.Size = new System.Drawing.Size(216, 40);
            this.txtBarcode.TabIndex = 60;
            this.txtBarcode.TextChanged += new System.EventHandler(this.txtBarcode_TextChanged);
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(29, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 27);
            this.label11.TabIndex = 59;
            this.label11.Text = "Barcode:";
            // 
            // txtQuantity
            // 
            this.txtQuantity.BorderColor = System.Drawing.Color.Black;
            this.txtQuantity.BorderRadius = 11;
            this.txtQuantity.BorderThickness = 2;
            this.txtQuantity.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtQuantity.DefaultText = "";
            this.txtQuantity.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtQuantity.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtQuantity.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtQuantity.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtQuantity.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtQuantity.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantity.ForeColor = System.Drawing.Color.Black;
            this.txtQuantity.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtQuantity.Location = new System.Drawing.Point(770, 11);
            this.txtQuantity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtQuantity.MaxLength = 10;
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.PlaceholderText = "";
            this.txtQuantity.SelectedText = "";
            this.txtQuantity.Size = new System.Drawing.Size(216, 40);
            this.txtQuantity.TabIndex = 64;
            this.txtQuantity.TextChanged += new System.EventHandler(this.txtQuantity_TextChanged_1);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(633, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(117, 27);
            this.label12.TabIndex = 63;
            this.label12.Text = "Quantity:";
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeColumns = false;
            this.dgvProduct.AllowUserToResizeRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Location = new System.Drawing.Point(12, 187);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.ReadOnly = true;
            this.dgvProduct.RowHeadersWidth = 51;
            this.dgvProduct.RowTemplate.Height = 24;
            this.dgvProduct.Size = new System.Drawing.Size(1081, 531);
            this.dgvProduct.TabIndex = 65;
            this.dgvProduct.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellContentClick);
            this.dgvProduct.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProduct_CellFormatting);
            this.dgvProduct.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProduct_CellMouseDown);
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.txtPoints);
            this.panel5.Controls.Add(this.htmlLabel1);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.txtQuantity);
            this.panel5.Controls.Add(this.txtMemberID);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.txtBarcode);
            this.panel5.Location = new System.Drawing.Point(12, 71);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1075, 110);
            this.panel5.TabIndex = 66;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(650, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 27);
            this.label7.TabIndex = 67;
            this.label7.Text = "Points:\r\n";
            // 
            // txtPoints
            // 
            this.txtPoints.BorderColor = System.Drawing.Color.Black;
            this.txtPoints.BorderRadius = 11;
            this.txtPoints.BorderThickness = 2;
            this.txtPoints.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPoints.DefaultText = "";
            this.txtPoints.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPoints.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPoints.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPoints.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPoints.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPoints.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoints.ForeColor = System.Drawing.Color.Black;
            this.txtPoints.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPoints.Location = new System.Drawing.Point(770, 60);
            this.txtPoints.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPoints.Name = "txtPoints";
            this.txtPoints.PlaceholderText = "";
            this.txtPoints.ReadOnly = true;
            this.txtPoints.SelectedText = "";
            this.txtPoints.Size = new System.Drawing.Size(216, 40);
            this.txtPoints.TabIndex = 66;
            // 
            // htmlLabel1
            // 
            this.htmlLabel1.AutoScroll = true;
            this.htmlLabel1.AutoScrollMinSize = new System.Drawing.Size(74, 25);
            this.htmlLabel1.AutoSize = false;
            this.htmlLabel1.BackColor = System.Drawing.SystemColors.Window;
            this.htmlLabel1.Location = new System.Drawing.Point(795, 84);
            this.htmlLabel1.Name = "htmlLabel1";
            this.htmlLabel1.Size = new System.Drawing.Size(8, 8);
            this.htmlLabel1.TabIndex = 65;
            this.htmlLabel1.Text = "htmlLabel1";
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1093, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(454, 669);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 659);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(454, 10);
            this.panel6.TabIndex = 68;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 723);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1093, 10);
            this.panel7.TabIndex = 67;
            // 
            // POS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1547, 733);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.dgvProduct);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "POS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POS";
            this.Load += new System.EventHandler(this.POS_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button btnDiscount;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button txtEndShift;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2Button guna2Button5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2TextBox txtSubTotal;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtDiscount;
        private Guna.UI2.WinForms.Guna2TextBox txtVatableSales;
        private Guna.UI2.WinForms.Guna2TextBox txtVatExempt;
        private Guna.UI2.WinForms.Guna2TextBox txtVatAmount;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2TextBox txtTotal;
        private Guna.UI2.WinForms.Guna2TextBox txtMemberID;
        private System.Windows.Forms.Label label10;
        private Guna.UI2.WinForms.Guna2TextBox txtBarcode;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2TextBox txtQuantity;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dgvProduct;
        private Guna.UI2.WinForms.Guna2Button txtTransactionHistory;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private Guna.UI2.WinForms.Guna2Button btnRegister;
        private MetroFramework.Drawing.Html.HtmlLabel htmlLabel1;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2TextBox txtPoints;
        private Guna.UI2.WinForms.Guna2TextBox txtRedeemedPoints;
        private System.Windows.Forms.Label label8;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Drawing.Printing.PrintDocument printDocument2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
    }
}