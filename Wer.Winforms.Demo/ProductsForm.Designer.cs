namespace Wer.Winforms.Demo
{
    partial class ProductsForm
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
            this.dgUsers = new Wer.Winforms.Toolkit.Controls.WerDataGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.werButtonOrange1 = new Wer.Winforms.Toolkit.Controls.WerButton();
            this.werTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
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
            this.werTabControl1.Size = new System.Drawing.Size(895, 546);
            this.werTabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.dgUsers);
            this.tabPage1.Location = new System.Drawing.Point(4, 42);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(887, 500);
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
            this.dgUsers.Size = new System.Drawing.Size(881, 494);
            this.dgUsers.TabIndex = 0;
            this.dgUsers.TabOptions = new string[] {
        "Active",
        "Inactive",
        "Blocked"};
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
            this.werButtonOrange1.ColorType = Wer.Winforms.Toolkit.WerButtonColor.Orange;
            this.werButtonOrange1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werButtonOrange1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werButtonOrange1.Location = new System.Drawing.Point(273, 113);
            this.werButtonOrange1.Name = "werButtonOrange1";
            this.werButtonOrange1.Size = new System.Drawing.Size(224, 36);
            this.werButtonOrange1.TabIndex = 0;
            this.werButtonOrange1.Text = "werButtonOrange1";
            // 
            // ProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 588);
            this.Controls.Add(this.werTabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ProductsForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "Products";
            this.Load += new System.EventHandler(this.ProductsForm_Load);
            this.werTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerTabControl werTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Toolkit.Controls.WerDataGrid dgUsers;
        private System.Windows.Forms.TabPage tabPage2;
        private Toolkit.Controls.WerButton werButtonOrange1;
    }
}