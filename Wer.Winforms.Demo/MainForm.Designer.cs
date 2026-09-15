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
            this.werTopNav1 = new Wer.Winforms.Toolkit.Controls.WerTopNav();
            this.werLeftNavMenu1 = new Wer.Winforms.Toolkit.Controls.WerLeftNavMenu();
            this.werMenuButton3 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton2 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton1 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
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
    }
}
