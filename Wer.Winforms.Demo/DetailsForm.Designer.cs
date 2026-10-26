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
            this.werTextField1 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.werBadge1 = new Wer.Winforms.Toolkit.Controls.WerBadge();
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
            this.werButton1.ButtonType = Wer.Winforms.Toolkit.WerButtonType.Secondary;
            this.werButton1.ColorType = Wer.Winforms.Toolkit.WerButtonColor.Gray;
            this.werButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.werButton1.FontSize = 13F;
            this.werButton1.Location = new System.Drawing.Point(462, 35);
            this.werButton1.Name = "werButton1";
            this.werButton1.Size = new System.Drawing.Size(202, 44);
            this.werButton1.TabIndex = 3;
            this.werButton1.Text = "Pay";
            this.werButton1.TextAlign = Wer.Winforms.Toolkit.WerButtonTextAlign.Left;
            this.werButton1.Click += new System.EventHandler(this.werButton1_Click);
            // 
            // werTextField1
            // 
            this.werTextField1.BackColor = System.Drawing.Color.Transparent;
            this.werTextField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField1.LabelText = "TextField Label";
            this.werTextField1.Location = new System.Drawing.Point(713, 11);
            this.werTextField1.Name = "werTextField1";
            this.werTextField1.Size = new System.Drawing.Size(350, 60);
            this.werTextField1.TabIndex = 4;
            this.werTextField1.TabStop = false;
            // 
            // werBadge1
            // 
            this.werBadge1.BackColor = System.Drawing.Color.Transparent;
            this.werBadge1.BadgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.werBadge1.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.werBadge1.Location = new System.Drawing.Point(556, 45);
            this.werBadge1.Name = "werBadge1";
            this.werBadge1.Size = new System.Drawing.Size(95, 24);
            this.werBadge1.TabIndex = 5;
            this.werBadge1.Text = "werBadge1";
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1565, 1040);
            this.Controls.Add(this.werBadge1);
            this.Controls.Add(this.werTextField1);
            this.Controls.Add(this.werButton1);
            this.Controls.Add(this.werCurrencyField1);
            this.Controls.Add(this.SampleGrid);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DetailsForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "Details";
            this.Load += new System.EventHandler(this.DetailsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Toolkit.Controls.WerDataGrid SampleGrid;
        private Toolkit.Controls.WerCurrencyField werCurrencyField1;
        private Toolkit.Controls.WerButton werButton1;
        private Toolkit.Controls.WerTextField werTextField1;
        private Toolkit.Controls.WerBadge werBadge1;
    }
}
