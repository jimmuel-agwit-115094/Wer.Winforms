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
            this.werCurrencyField1 = new Wer.Winforms.Toolkit.Controls.WerCurrencyField();
            this.werButton1 = new Wer.Winforms.Toolkit.Controls.WerButton();
            this.SuspendLayout();
            // 
            // SampleGrid
            // 
            this.SampleGrid.BackColor = System.Drawing.Color.White;
            this.SampleGrid.DataSource = null;
            this.SampleGrid.Location = new System.Drawing.Point(22, 112);
            this.SampleGrid.Name = "SampleGrid";
            this.SampleGrid.PrimaryKeyColumn = null;
            this.SampleGrid.RowHeight = 40;
            this.SampleGrid.ShowFooterPagination = false;
            this.SampleGrid.ShowSearchField = false;
            this.SampleGrid.Size = new System.Drawing.Size(1521, 718);
            this.SampleGrid.TabIndex = 1;
            this.SampleGrid.TabOptions = new string[0];
            // 
            // werCurrencyField1
            // 
            this.werCurrencyField1.BackColor = System.Drawing.Color.Transparent;
            this.werCurrencyField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werCurrencyField1.LabelText = "Currency Label";
            this.werCurrencyField1.Location = new System.Drawing.Point(22, 11);
            this.werCurrencyField1.Name = "werCurrencyField1";
            this.werCurrencyField1.ReadOnly = true;
            this.werCurrencyField1.Size = new System.Drawing.Size(350, 60);
            this.werCurrencyField1.TabIndex = 2;
            this.werCurrencyField1.TabStop = false;
            this.werCurrencyField1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // werButton1
            // 
            this.werButton1.BoldFont = true;
            this.werButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.werButton1.FontSize = 13F;
            this.werButton1.Location = new System.Drawing.Point(462, 35);
            this.werButton1.Name = "werButton1";
            this.werButton1.Size = new System.Drawing.Size(192, 36);
            this.werButton1.TabIndex = 3;
            this.werButton1.Text = "Pay";
            this.werButton1.Click += new System.EventHandler(this.werButton1_Click);
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1565, 1040);
            this.Controls.Add(this.werButton1);
            this.Controls.Add(this.werCurrencyField1);
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
        private Toolkit.Controls.WerCurrencyField werCurrencyField1;
        private Toolkit.Controls.WerButton werButton1;
    }
}
