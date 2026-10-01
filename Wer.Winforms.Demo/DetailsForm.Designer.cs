namespace Wer.Winforms.Demo
{
    partial class DetailsForm
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
            this.werDataGrid1 = new Wer.Winforms.Toolkit.Controls.WerDataGrid();
            this.SuspendLayout();
            // 
            // werDataGrid1
            // 
            this.werDataGrid1.AddButtonText = "Add User";
            this.werDataGrid1.BackColor = System.Drawing.Color.White;
            this.werDataGrid1.DataSource = null;
            this.werDataGrid1.Location = new System.Drawing.Point(22, 8);
            this.werDataGrid1.Name = "werDataGrid1";
            this.werDataGrid1.PrimaryKeyColumn = null;
            this.werDataGrid1.ShowAddButton = true;
            this.werDataGrid1.ShowEditColumn = true;
            this.werDataGrid1.Size = new System.Drawing.Size(1145, 564);
            this.werDataGrid1.TabIndex = 0;
            this.werDataGrid1.TabOptions = new string[0];
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 612);
            this.Controls.Add(this.werDataGrid1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DetailsForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "Details";
            this.Load += new System.EventHandler(this.DetailsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerDataGrid werDataGrid1;
    }
}