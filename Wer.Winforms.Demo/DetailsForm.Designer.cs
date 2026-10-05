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
            this.SampleGrid = new Wer.Winforms.Toolkit.Controls.WerDataGrid();
            this.SuspendLayout();
            // 
            // SampleGrid
            // 
            this.SampleGrid.BackColor = System.Drawing.Color.White;
            this.SampleGrid.DataSource = null;
            this.SampleGrid.Location = new System.Drawing.Point(22, 8);
            this.SampleGrid.Name = "SampleGrid";
            this.SampleGrid.PrimaryKeyColumn = null;
            this.SampleGrid.RowHeight = 40;
            this.SampleGrid.ShowFooterPagination = false;
            this.SampleGrid.ShowSearchField = false;
            this.SampleGrid.Size = new System.Drawing.Size(1521, 822);
            this.SampleGrid.TabIndex = 1;
            this.SampleGrid.TabOptions = new string[0];
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1565, 1040);
            this.Controls.Add(this.SampleGrid);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DetailsForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "Details";
            this.Load += new System.EventHandler(this.DetailsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerDataGrid SampleGrid;
    }
}
