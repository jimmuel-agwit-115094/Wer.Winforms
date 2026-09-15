namespace Wer.Winforms.Demo
{
    partial class SubForm
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.werButtonOrange1 = new Wer.Winforms.Toolkit.Controls.WerButtonOrange();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgUsers = new Wer.Winforms.Toolkit.Controls.WerDataGrid();
            this.werTabControl1 = new Wer.Winforms.Toolkit.Controls.WerTabControl();
            this.werLabel1 = new Wer.Winforms.Toolkit.Controls.WerLabel();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.werTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.werButtonOrange1);
            this.tabPage2.Location = new System.Drawing.Point(4, 42);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1229, 547);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Products";
            // 
            // werButtonOrange1
            // 
            this.werButtonOrange1.BorderColor = System.Drawing.Color.Empty;
            this.werButtonOrange1.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.werButtonOrange1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werButtonOrange1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werButtonOrange1.ForeColor = System.Drawing.Color.White;
            this.werButtonOrange1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(107)))), ((int)(((byte)(28)))));
            this.werButtonOrange1.Location = new System.Drawing.Point(273, 113);
            this.werButtonOrange1.Name = "werButtonOrange1";
            this.werButtonOrange1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(88)))), ((int)(((byte)(23)))));
            this.werButtonOrange1.Size = new System.Drawing.Size(224, 36);
            this.werButtonOrange1.TabIndex = 0;
            this.werButtonOrange1.Text = "werButtonOrange1";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.dgUsers);
            this.tabPage1.Location = new System.Drawing.Point(4, 42);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1229, 547);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Users";
            // 
            // dgUsers
            // 
            this.dgUsers.BackColor = System.Drawing.Color.White;
            this.dgUsers.DataSource = null;
            this.dgUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgUsers.Location = new System.Drawing.Point(3, 3);
            this.dgUsers.Name = "dgUsers";
            this.dgUsers.PrimaryKeyColumn = null;
            this.dgUsers.ShowAddButton = true;
            this.dgUsers.Size = new System.Drawing.Size(1223, 541);
            this.dgUsers.TabIndex = 0;
            this.dgUsers.TabOptions = new string[] {
        "Active",
        "Inactive",
        "Blocked"};
            // 
            // werTabControl1
            // 
            this.werTabControl1.Controls.Add(this.tabPage1);
            this.werTabControl1.Controls.Add(this.tabPage2);
            this.werTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.werTabControl1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTabControl1.ItemSize = new System.Drawing.Size(0, 38);
            this.werTabControl1.Location = new System.Drawing.Point(19, 21);
            this.werTabControl1.Name = "werTabControl1";
            this.werTabControl1.Padding = new System.Drawing.Point(16, 0);
            this.werTabControl1.SelectedIndex = 0;
            this.werTabControl1.Size = new System.Drawing.Size(1237, 593);
            this.werTabControl1.TabIndex = 0;
            // 
            // werLabel1
            // 
            this.werLabel1.AutoSize = true;
            this.werLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.werLabel1.Location = new System.Drawing.Point(403, 1);
            this.werLabel1.Name = "werLabel1";
            this.werLabel1.Size = new System.Drawing.Size(67, 17);
            this.werLabel1.TabIndex = 1;
            this.werLabel1.Text = "werLabel1";
            // 
            // SubForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 635);
            this.Controls.Add(this.werLabel1);
            this.Controls.Add(this.werTabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SubForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "Sub Form";
            this.Load += new System.EventHandler(this.ButtonsForm_Load);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.werTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private Toolkit.Controls.WerButtonOrange werButtonOrange1;
        private System.Windows.Forms.TabPage tabPage1;
        private Toolkit.Controls.WerDataGrid dgUsers;
        private Toolkit.Controls.WerTabControl werTabControl1;
        private Toolkit.Controls.WerLabel werLabel1;
    }
}