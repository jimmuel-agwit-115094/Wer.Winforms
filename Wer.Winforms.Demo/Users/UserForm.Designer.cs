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
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 631);
            this.Controls.Add(this.dgUsers);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "User Details";
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerDataGrid dgUsers;
    }
}
