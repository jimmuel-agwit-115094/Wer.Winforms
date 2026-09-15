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
            this.werTabControl1 = new Wer.Winforms.Toolkit.Controls.WerTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgUsers = new Wer.Winforms.Toolkit.Controls.WerDataGrid();
            this.werTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // werTabControl1
            // 
            this.werTabControl1.Controls.Add(this.tabPage1);
            this.werTabControl1.Controls.Add(this.tabPage2);
            this.werTabControl1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.werTabControl1.ItemSize = new System.Drawing.Size(0, 38);
            this.werTabControl1.Location = new System.Drawing.Point(0, 0);
            this.werTabControl1.Name = "werTabControl1";
            this.werTabControl1.Padding = new System.Drawing.Point(16, 0);
            this.werTabControl1.SelectedIndex = 0;
            this.werTabControl1.Size = new System.Drawing.Size(1218, 558);
            this.werTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.dgUsers);
            this.tabPage1.Location = new System.Drawing.Point(4, 42);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1210, 512);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Users";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Location = new System.Drawing.Point(4, 42);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1210, 512);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Products";
            // 
            // dgUsers
            // 
            this.dgUsers.BackColor = System.Drawing.Color.White;
            this.dgUsers.DataSource = null;
            this.dgUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgUsers.Location = new System.Drawing.Point(3, 3);
            this.dgUsers.Name = "dgUsers";
            this.dgUsers.PrimaryKeyColumn = null;
            this.dgUsers.Size = new System.Drawing.Size(1204, 506);
            this.dgUsers.TabIndex = 0;
            this.dgUsers.TabOptions = new string[] {
        "Active",
        "Inactive",
        "Blocked"};
            // 
            // SubForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 635);
            this.Controls.Add(this.werTabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SubForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "Sub Form";
            this.Load += new System.EventHandler(this.ButtonsForm_Load);
            this.werTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerTabControl werTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Toolkit.Controls.WerDataGrid dgUsers;
    }
}