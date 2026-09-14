namespace Wer.Winforms.Demo
{
    partial class MenuForm
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
            this.werTopNav1 = new Wer.Winforms.Toolkit.Controls.WerTopNav();
            this.werLeftNavMenu1 = new Wer.Winforms.Toolkit.Controls.WerLeftNavMenu();
            this.werMenuButton1 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton2 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.werMenuButton3 = new Wer.Winforms.Toolkit.Controls.WerMenuButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.werLeftNavMenu1.SuspendLayout();
            this.SuspendLayout();
            //
            // werTopNav1 — Dock Top (add last to Controls so it docks first)
            //
            this.werTopNav1.BackColor = System.Drawing.Color.White;
            this.werTopNav1.Dock = System.Windows.Forms.DockStyle.Top;
            this.werTopNav1.Location = new System.Drawing.Point(0, 0);
            this.werTopNav1.Name = "werTopNav1";
            this.werTopNav1.Size = new System.Drawing.Size(1024, 48);
            this.werTopNav1.TabIndex = 0;
            this.werTopNav1.UserName = "John Doe";
            this.werTopNav1.PageTitle = "";
            //
            // werLeftNavMenu1 — Dock Left
            //
            this.werLeftNavMenu1.AutoScroll = true;
            this.werLeftNavMenu1.BackColor = System.Drawing.Color.White;
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton3);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton2);
            this.werLeftNavMenu1.Controls.Add(this.werMenuButton1);
            this.werLeftNavMenu1.Dock = System.Windows.Forms.DockStyle.Left;
            this.werLeftNavMenu1.Location = new System.Drawing.Point(0, 48);
            this.werLeftNavMenu1.LogoText = "MyApp";
            this.werLeftNavMenu1.Name = "werLeftNavMenu1";
            this.werLeftNavMenu1.Size = new System.Drawing.Size(220, 520);
            this.werLeftNavMenu1.TabIndex = 1;
            //
            // werMenuButton1
            //
            this.werMenuButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton1.Name = "werMenuButton1";
            this.werMenuButton1.Size = new System.Drawing.Size(212, 40);
            this.werMenuButton1.TabIndex = 0;
            this.werMenuButton1.Text = "Dashboard";
            this.werMenuButton1.Click += new System.EventHandler(this.werMenuButton1_Click);
            //
            // werMenuButton2
            //
            this.werMenuButton2.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton2.Name = "werMenuButton2";
            this.werMenuButton2.Size = new System.Drawing.Size(212, 40);
            this.werMenuButton2.TabIndex = 1;
            this.werMenuButton2.Text = "Customers";
            this.werMenuButton2.Click += new System.EventHandler(this.werMenuButton2_Click);
            //
            // werMenuButton3
            //
            this.werMenuButton3.Dock = System.Windows.Forms.DockStyle.Top;
            this.werMenuButton3.Name = "werMenuButton3";
            this.werMenuButton3.Size = new System.Drawing.Size(212, 40);
            this.werMenuButton3.TabIndex = 2;
            this.werMenuButton3.Text = "Settings";
            this.werMenuButton3.Click += new System.EventHandler(this.werMenuButton3_Click);
            //
            // panel1 — Dock Fill (content area)
            //
            this.panel1.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(220, 48);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(16);
            this.panel1.Size = new System.Drawing.Size(804, 520);
            this.panel1.TabIndex = 2;
            //
            // MenuForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 568);
            // Dock order: Left nav full height, then top nav + content fill the rest
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.werTopNav1);
            this.Controls.Add(this.werLeftNavMenu1);
            this.Name = "MenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MenuForm";
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.werLeftNavMenu1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Toolkit.Controls.WerTopNav werTopNav1;
        private Toolkit.Controls.WerLeftNavMenu werLeftNavMenu1;
        private System.Windows.Forms.Panel panel1;
        private Toolkit.Controls.WerMenuButton werMenuButton1;
        private Toolkit.Controls.WerMenuButton werMenuButton2;
        private Toolkit.Controls.WerMenuButton werMenuButton3;
    }
}
