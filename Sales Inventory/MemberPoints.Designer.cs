namespace Sales_Inventory
{
    partial class MemberPoints
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnProceed = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.txtPointsToRedeem = new Guna.UI2.WinForms.Guna2TextBox();
            this.chkRedeem = new Guna.UI2.WinForms.Guna2CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCurrentPoints = new System.Windows.Forms.Label();
            this.lblMemberName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 72);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(215, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 37);
            this.label1.TabIndex = 78;
            this.label1.Text = "Member Points";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnProceed);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.txtPointsToRedeem);
            this.panel2.Controls.Add(this.chkRedeem);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.lblCurrentPoints);
            this.panel2.Controls.Add(this.lblMemberName);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(111, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(429, 314);
            this.panel2.TabIndex = 1;
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
            this.btnProceed.Location = new System.Drawing.Point(17, 248);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(177, 47);
            this.btnProceed.TabIndex = 173;
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
            this.btnCancel.Location = new System.Drawing.Point(231, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(177, 47);
            this.btnCancel.TabIndex = 172;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtPointsToRedeem
            // 
            this.txtPointsToRedeem.BorderColor = System.Drawing.Color.Black;
            this.txtPointsToRedeem.BorderRadius = 11;
            this.txtPointsToRedeem.BorderThickness = 2;
            this.txtPointsToRedeem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPointsToRedeem.DefaultText = "";
            this.txtPointsToRedeem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPointsToRedeem.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPointsToRedeem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPointsToRedeem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPointsToRedeem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPointsToRedeem.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPointsToRedeem.ForeColor = System.Drawing.Color.Black;
            this.txtPointsToRedeem.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPointsToRedeem.Location = new System.Drawing.Point(231, 182);
            this.txtPointsToRedeem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPointsToRedeem.MaxLength = 13;
            this.txtPointsToRedeem.Name = "txtPointsToRedeem";
            this.txtPointsToRedeem.PlaceholderText = "";
            this.txtPointsToRedeem.SelectedText = "";
            this.txtPointsToRedeem.Size = new System.Drawing.Size(121, 40);
            this.txtPointsToRedeem.TabIndex = 95;
            // 
            // chkRedeem
            // 
            this.chkRedeem.AutoSize = true;
            this.chkRedeem.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkRedeem.CheckedState.BorderRadius = 0;
            this.chkRedeem.CheckedState.BorderThickness = 0;
            this.chkRedeem.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkRedeem.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRedeem.Location = new System.Drawing.Point(45, 147);
            this.chkRedeem.Name = "chkRedeem";
            this.chkRedeem.Size = new System.Drawing.Size(164, 25);
            this.chkRedeem.TabIndex = 83;
            this.chkRedeem.Text = "Redeem Points";
            this.chkRedeem.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkRedeem.UncheckedState.BorderRadius = 0;
            this.chkRedeem.UncheckedState.BorderThickness = 0;
            this.chkRedeem.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(39, 199);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(186, 23);
            this.label8.TabIndex = 82;
            this.label8.Text = "Points to Redeem:";
            // 
            // lblCurrentPoints
            // 
            this.lblCurrentPoints.AutoSize = true;
            this.lblCurrentPoints.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPoints.Location = new System.Drawing.Point(214, 88);
            this.lblCurrentPoints.Name = "lblCurrentPoints";
            this.lblCurrentPoints.Size = new System.Drawing.Size(160, 23);
            this.lblCurrentPoints.TabIndex = 80;
            this.lblCurrentPoints.Text = "Member Points:";
            // 
            // lblMemberName
            // 
            this.lblMemberName.AutoSize = true;
            this.lblMemberName.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMemberName.Location = new System.Drawing.Point(39, 38);
            this.lblMemberName.Name = "lblMemberName";
            this.lblMemberName.Size = new System.Drawing.Size(164, 23);
            this.lblMemberName.TabIndex = 79;
            this.lblMemberName.Text = "Member Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 23);
            this.label2.TabIndex = 78;
            this.label2.Text = "Current Points:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(214, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(164, 23);
            this.label4.TabIndex = 77;
            this.label4.Text = "Member Name:";
            // 
            // MemberPoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MemberPoints";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MemberPoints";
            this.Load += new System.EventHandler(this.MemberPoints_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2CheckBox chkRedeem;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblCurrentPoints;
        private System.Windows.Forms.Label lblMemberName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2TextBox txtPointsToRedeem;
        private Guna.UI2.WinForms.Guna2Button btnProceed;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
    }
}