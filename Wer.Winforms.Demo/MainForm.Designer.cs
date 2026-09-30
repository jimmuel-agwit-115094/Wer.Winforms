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
            Wer.Winforms.Toolkit.Controls.SubMenuItem subMenuItem1 = new Wer.Winforms.Toolkit.Controls.SubMenuItem();
            Wer.Winforms.Toolkit.Controls.SubMenuItem subMenuItem2 = new Wer.Winforms.Toolkit.Controls.SubMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.werTopNav1 = new Wer.Winforms.Toolkit.Controls.WerTopNav();
            this.werLeftNavMenu1 = new Wer.Winforms.Toolkit.Controls.WerLeftNavMenu();
            this.werMenuButton2 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton1 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton3 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton4 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton5 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton6 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton7 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werLeftNavMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(210, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 616);
            this.panel1.TabIndex = 2;
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
            this.werLeftNavMenu1.BackColor = System.Drawing.Color.White;
            this.werLeftNavMenu1.ContentPanel = this.panel1;
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton7);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton6);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton5);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton4);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton3);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton2);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton1);
            this.werLeftNavMenu1.Dock = System.Windows.Forms.DockStyle.Left;
            this.werLeftNavMenu1.Location = new System.Drawing.Point(0, 0);
            this.werLeftNavMenu1.LogoText = "BarPos";
            this.werLeftNavMenu1.Name = "werLeftNavMenu1";
            this.werLeftNavMenu1.Size = new System.Drawing.Size(210, 657);
            this.werLeftNavMenu1.TabIndex = 0;
            // 
            // werMenuButton2
            // 
            this.werMenuButton2.BackColor = System.Drawing.Color.White;
            this.werMenuButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton2.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton2.HasActiveChild = false;
            this.werMenuButton2.IsActive = false;
            this.werMenuButton2.IsExpanded = false;
            this.werMenuButton2.IsSubItem = false;
            this.werMenuButton2.Location = new System.Drawing.Point(0, 40);
            this.werMenuButton2.Name = "werMenuButton2";
            this.werMenuButton2.Size = new System.Drawing.Size(210, 40);
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
            this.werMenuButton1.HasActiveChild = false;
            this.werMenuButton1.IsActive = false;
            this.werMenuButton1.IsExpanded = false;
            this.werMenuButton1.IsSubItem = false;
            this.werMenuButton1.Location = new System.Drawing.Point(0, 0);
            this.werMenuButton1.Name = "werMenuButton1";
            this.werMenuButton1.Size = new System.Drawing.Size(210, 40);
            subMenuItem1.Action = null;
            subMenuItem1.Text = "Active";
            subMenuItem2.Action = null;
            subMenuItem2.Text = "Test Inactuive";
            this.werMenuButton1.SubItems.Add(subMenuItem1);
            this.werMenuButton1.SubItems.Add(subMenuItem2);
            this.werMenuButton1.TabIndex = 0;
            this.werMenuButton1.Text = "Users";
            // 
            // werMenuButton3
            // 
            this.werMenuButton3.BackColor = System.Drawing.Color.White;
            this.werMenuButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton3.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton3.HasActiveChild = false;
            this.werMenuButton3.IsActive = false;
            this.werMenuButton3.IsExpanded = false;
            this.werMenuButton3.IsSubItem = false;
            this.werMenuButton3.Location = new System.Drawing.Point(0, 80);
            this.werMenuButton3.Name = "werMenuButton3";
            this.werMenuButton3.Size = new System.Drawing.Size(210, 40);
            this.werMenuButton3.TabIndex = 2;
            this.werMenuButton3.Text = "Texts";
            this.werMenuButton3.Click += new System.EventHandler(this.werMenuButton3_Click);
            // 
            // werMenuButton4
            // 
            this.werMenuButton4.BackColor = System.Drawing.Color.White;
            this.werMenuButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton4.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton4.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton4.HasActiveChild = false;
            this.werMenuButton4.IsActive = false;
            this.werMenuButton4.IsExpanded = false;
            this.werMenuButton4.IsSubItem = false;
            this.werMenuButton4.Location = new System.Drawing.Point(0, 180);
            this.werMenuButton4.Name = "werMenuButton4";
            this.werMenuButton4.Size = new System.Drawing.Size(210, 40);
            this.werMenuButton4.TabIndex = 3;
            this.werMenuButton4.Text = "werMenuButton4";
            // 
            // werMenuButton5
            // 
            this.werMenuButton5.BackColor = System.Drawing.Color.White;
            this.werMenuButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton5.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton5.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton5.HasActiveChild = false;
            this.werMenuButton5.IsActive = false;
            this.werMenuButton5.IsExpanded = false;
            this.werMenuButton5.IsSubItem = false;
            this.werMenuButton5.Location = new System.Drawing.Point(0, 220);
            this.werMenuButton5.Name = "werMenuButton5";
            this.werMenuButton5.Size = new System.Drawing.Size(210, 40);
            this.werMenuButton5.TabIndex = 4;
            this.werMenuButton5.Text = "werMenuButton5";
            // 
            // werMenuButton6
            // 
            this.werMenuButton6.BackColor = System.Drawing.Color.White;
            this.werMenuButton6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton6.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton6.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton6.HasActiveChild = false;
            this.werMenuButton6.IsActive = false;
            this.werMenuButton6.IsExpanded = false;
            this.werMenuButton6.IsSubItem = false;
            this.werMenuButton6.Location = new System.Drawing.Point(0, 260);
            this.werMenuButton6.Name = "werMenuButton6";
            this.werMenuButton6.Size = new System.Drawing.Size(210, 40);
            this.werMenuButton6.TabIndex = 5;
            this.werMenuButton6.Text = "werMenuButton6";
            // 
            // werMenuButton7
            // 
            this.werMenuButton7.BackColor = System.Drawing.Color.White;
            this.werMenuButton7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werMenuButton7.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton7.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werMenuButton7.HasActiveChild = false;
            this.werMenuButton7.IsActive = false;
            this.werMenuButton7.IsExpanded = false;
            this.werMenuButton7.IsSubItem = false;
            this.werMenuButton7.Location = new System.Drawing.Point(0, 300);
            this.werMenuButton7.Name = "werMenuButton7";
            this.werMenuButton7.Size = new System.Drawing.Size(210, 40);
            this.werMenuButton7.TabIndex = 6;
            this.werMenuButton7.Text = "werMenuButton7";
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
            this.werLeftNavMenu1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerLeftNavMenu werLeftNavMenu1;
        private Toolkit.Controls.WerMenuButton werMenuButton2;
        private Toolkit.Controls.WerMenuButton werMenuButton1;
        private Toolkit.Controls.WerTopNav werTopNav1;
        private System.Windows.Forms.Panel panel1;
        private Toolkit.Controls.WerMenuButton werMenuButton5;
        private Toolkit.Controls.WerMenuButton werMenuButton4;
        private Toolkit.Controls.WerMenuButton werMenuButton3;
        private Toolkit.Controls.WerMenuButton werMenuButton7;
        private Toolkit.Controls.WerMenuButton werMenuButton6;
    }
}
