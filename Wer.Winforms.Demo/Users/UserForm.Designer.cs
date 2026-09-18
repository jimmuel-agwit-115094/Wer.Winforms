namespace Wer.Winforms.Demo
{
    partial class UserForm
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
            this.dgUsers = new Wer.Winforms.Toolkit.Controls.WerDataGrid();
            this.werStatusIndicator1 = new Wer.Winforms.Toolkit.Controls.WerStatusIndicator();
            this.werStatusIndicator2 = new Wer.Winforms.Toolkit.Controls.WerStatusIndicator();
            this.SuspendLayout();
            // 
            // dgUsers
            // 
            this.dgUsers.BackColor = System.Drawing.Color.White;
            this.dgUsers.DataSource = null;
            this.dgUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgUsers.Location = new System.Drawing.Point(19, 21);
            this.dgUsers.Name = "dgUsers";
            this.dgUsers.PrimaryKeyColumn = null;
            this.dgUsers.ShowAddButton = true;
            this.dgUsers.ShowFooterPagination = false;
            this.dgUsers.Size = new System.Drawing.Size(1083, 589);
            this.dgUsers.TabIndex = 1;
            this.dgUsers.TabOptions = new string[] {
        "Active",
        "Inactive",
        "Blocked"};
            this.dgUsers.Load += new System.EventHandler(this.dgUsers_Load);
            // 
            // werStatusIndicator1
            // 
            this.werStatusIndicator1.BackColor = System.Drawing.Color.Transparent;
            this.werStatusIndicator1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werStatusIndicator1.Location = new System.Drawing.Point(582, 29);
            this.werStatusIndicator1.Name = "werStatusIndicator1";
            this.werStatusIndicator1.Size = new System.Drawing.Size(158, 26);
            this.werStatusIndicator1.Status = Wer.Winforms.Toolkit.Controls.WerStatus.Informative;
            this.werStatusIndicator1.TabIndex = 2;
            this.werStatusIndicator1.Text = "werStatusIndicator1";
            // 
            // werStatusIndicator2
            // 
            this.werStatusIndicator2.BackColor = System.Drawing.Color.Transparent;
            this.werStatusIndicator2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werStatusIndicator2.Location = new System.Drawing.Point(418, 29);
            this.werStatusIndicator2.Name = "werStatusIndicator2";
            this.werStatusIndicator2.Size = new System.Drawing.Size(158, 26);
            this.werStatusIndicator2.Status = Wer.Winforms.Toolkit.Controls.WerStatus.Notice;
            this.werStatusIndicator2.TabIndex = 3;
            this.werStatusIndicator2.Text = "werStatusIndicator2";
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 631);
            this.Controls.Add(this.werStatusIndicator2);
            this.Controls.Add(this.werStatusIndicator1);
            this.Controls.Add(this.dgUsers);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "User Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Toolkit.Controls.WerDataGrid dgUsers;
        private Toolkit.Controls.WerStatusIndicator werStatusIndicator1;
        private Toolkit.Controls.WerStatusIndicator werStatusIndicator2;
    }
}
