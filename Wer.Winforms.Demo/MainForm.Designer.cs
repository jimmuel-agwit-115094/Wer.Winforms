namespace Wer.Winforms.Demo
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.werDateRangePicker1 = new Wer.Winforms.Toolkit.Controls.WerDateRangePicker();
            this.werBadge1 = new Wer.Winforms.Toolkit.Controls.WerBadge();
            this.werSpinner1 = new Wer.Winforms.Toolkit.Controls.WerSpinner();
            this.werCard1 = new Wer.Winforms.Toolkit.Controls.WerCard();
            this.werProgressBar1 = new Wer.Winforms.Toolkit.Controls.WerProgressBar();
            this.werTopNav1 = new Wer.Winforms.Toolkit.Controls.WerTopNav();
            this.werLeftNavMenu1 = new Wer.Winforms.Toolkit.Controls.WerLeftNavMenu();
            this.werMenuButton3 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton2 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton1 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.panel1.SuspendLayout();
            this.werLeftNavMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.werDateRangePicker1);
            this.panel1.Controls.Add(this.werBadge1);
            this.panel1.Controls.Add(this.werSpinner1);
            this.panel1.Controls.Add(this.werCard1);
            this.panel1.Controls.Add(this.werProgressBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(210, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 616);
            this.panel1.TabIndex = 2;
            // 
            // werDateRangePicker1
            // 
            this.werDateRangePicker1.BackColor = System.Drawing.Color.Transparent;
            this.werDateRangePicker1.EndDate = null;
            this.werDateRangePicker1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werDateRangePicker1.Location = new System.Drawing.Point(445, 202);
            this.werDateRangePicker1.Name = "werDateRangePicker1";
            this.werDateRangePicker1.Size = new System.Drawing.Size(500, 60);
            this.werDateRangePicker1.StartDate = null;
            this.werDateRangePicker1.TabIndex = 6;
            // 
            // werBadge1
            // 
            this.werBadge1.BackColor = System.Drawing.Color.Transparent;
            this.werBadge1.BadgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.werBadge1.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.werBadge1.Location = new System.Drawing.Point(353, 88);
            this.werBadge1.Name = "werBadge1";
            this.werBadge1.Size = new System.Drawing.Size(95, 24);
            this.werBadge1.TabIndex = 5;
            this.werBadge1.Text = "werBadge1";
            // 
            // werSpinner1
            // 
            this.werSpinner1.BackColor = System.Drawing.Color.Transparent;
            this.werSpinner1.Location = new System.Drawing.Point(134, 96);
            this.werSpinner1.Name = "werSpinner1";
            this.werSpinner1.Size = new System.Drawing.Size(40, 40);
            this.werSpinner1.SpinnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.werSpinner1.TabIndex = 4;
            this.werSpinner1.Text = "werSpinner1";
            // 
            // werCard1
            // 
            this.werCard1.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.werCard1.BackColor = System.Drawing.Color.Transparent;
            this.werCard1.Location = new System.Drawing.Point(75, 268);
            this.werCard1.Name = "werCard1";
            this.werCard1.Size = new System.Drawing.Size(390, 182);
            this.werCard1.TabIndex = 3;
            this.werCard1.Text = "werCard1";
            // 
            // werProgressBar1
            // 
            this.werProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.werProgressBar1.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.werProgressBar1.Location = new System.Drawing.Point(121, 186);
            this.werProgressBar1.Name = "werProgressBar1";
            this.werProgressBar1.Size = new System.Drawing.Size(344, 24);
            this.werProgressBar1.TabIndex = 2;
            this.werProgressBar1.Text = "werProgressBar1";
            // 
            // werTopNav1
            // 
            this.werTopNav1.BackColor = System.Drawing.Color.White;
            this.werTopNav1.Dock = System.Windows.Forms.DockStyle.Top;
            this.werTopNav1.Location = new System.Drawing.Point(210, 0);
            this.werTopNav1.Name = "werTopNav1";
            this.werTopNav1.Size = new System.Drawing.Size(1083, 41);
            this.werTopNav1.TabIndex = 1;
            this.werTopNav1.Text = "werTopNav1";
            // 
            // werLeftNavMenu1
            // 
            this.werLeftNavMenu1.AutoScroll = true;
            this.werLeftNavMenu1.BackColor = System.Drawing.Color.White;
            this.werLeftNavMenu1.ContentPanel = this.panel1;
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton3);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton2);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton1);
            this.werLeftNavMenu1.Dock = System.Windows.Forms.DockStyle.Left;
            this.werLeftNavMenu1.Location = new System.Drawing.Point(0, 0);
            this.werLeftNavMenu1.Name = "werLeftNavMenu1";
            this.werLeftNavMenu1.Padding = new System.Windows.Forms.Padding(4, 68, 4, 8);
            this.werLeftNavMenu1.Size = new System.Drawing.Size(210, 657);
            this.werLeftNavMenu1.TabIndex = 0;
            // 
            // werMenuButton3
            // 
            this.werMenuButton3.BackColor = System.Drawing.Color.White;
            this.werMenuButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton3.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton3.IsActive = false;
            this.werMenuButton3.Location = new System.Drawing.Point(4, 148);
            this.werMenuButton3.Name = "werMenuButton3";
            this.werMenuButton3.Size = new System.Drawing.Size(202, 40);
            this.werMenuButton3.TabIndex = 2;
            this.werMenuButton3.Text = "Texts";
            this.werMenuButton3.Click += new System.EventHandler(this.werMenuButton3_Click);
            // 
            // werMenuButton2
            // 
            this.werMenuButton2.BackColor = System.Drawing.Color.White;
            this.werMenuButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton2.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton2.IsActive = false;
            this.werMenuButton2.Location = new System.Drawing.Point(4, 108);
            this.werMenuButton2.Name = "werMenuButton2";
            this.werMenuButton2.Size = new System.Drawing.Size(202, 40);
            this.werMenuButton2.TabIndex = 1;
            this.werMenuButton2.Text = "Products";
            this.werMenuButton2.Click += new System.EventHandler(this.werMenuButton2_Click);
            // 
            // werMenuButton1
            // 
            this.werMenuButton1.BackColor = System.Drawing.Color.White;
            this.werMenuButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton1.IsActive = false;
            this.werMenuButton1.Location = new System.Drawing.Point(4, 68);
            this.werMenuButton1.Name = "werMenuButton1";
            this.werMenuButton1.Size = new System.Drawing.Size(202, 40);
            this.werMenuButton1.TabIndex = 0;
            this.werMenuButton1.Text = "Users";
            this.werMenuButton1.Click += new System.EventHandler(this.werMenuButton1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1293, 657);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.werTopNav1);
            this.Controls.Add(this.werLeftNavMenu1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wer.Winforms Demo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.werLeftNavMenu1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerLeftNavMenu werLeftNavMenu1;
        private Toolkit.Controls.WerMenuButton werMenuButton3;
        private Toolkit.Controls.WerMenuButton werMenuButton2;
        private Toolkit.Controls.WerMenuButton werMenuButton1;
        private Toolkit.Controls.WerTopNav werTopNav1;
        private System.Windows.Forms.Panel panel1;
        private Toolkit.Controls.WerDateRangePicker werDateRangePicker1;
        private Toolkit.Controls.WerBadge werBadge1;
        private Toolkit.Controls.WerSpinner werSpinner1;
        private Toolkit.Controls.WerCard werCard1;
        private Toolkit.Controls.WerProgressBar werProgressBar1;
    }
}
