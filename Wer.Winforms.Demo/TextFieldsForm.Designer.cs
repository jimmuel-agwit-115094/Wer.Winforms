namespace Wer.Winforms.Demo
{
    partial class TextFieldsForm
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
            this.werTextField1 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.werTelephoneField1 = new Wer.Winforms.Toolkit.Controls.WerTelephoneField();
            this.werPercentField1 = new Wer.Winforms.Toolkit.Controls.WerPercentField();
            this.SuspendLayout();
            // 
            // werTextField1
            // 
            this.werTextField1.BackColor = System.Drawing.Color.Transparent;
            this.werTextField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField1.LabelText = "TextField Label";
            this.werTextField1.Location = new System.Drawing.Point(99, 102);
            this.werTextField1.Name = "werTextField1";
            this.werTextField1.ReadOnly = true;
            this.werTextField1.Size = new System.Drawing.Size(350, 60);
            this.werTextField1.TabIndex = 0;
            this.werTextField1.Text = "werTextField1";
            // 
            // werTelephoneField1
            // 
            this.werTelephoneField1.BackColor = System.Drawing.Color.Transparent;
            this.werTelephoneField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTelephoneField1.Location = new System.Drawing.Point(99, 168);
            this.werTelephoneField1.Name = "werTelephoneField1";
            this.werTelephoneField1.ReadOnly = true;
            this.werTelephoneField1.Size = new System.Drawing.Size(350, 63);
            this.werTelephoneField1.TabIndex = 1;
            this.werTelephoneField1.Text = "(1";
            // 
            // werPercentField1
            // 
            this.werPercentField1.BackColor = System.Drawing.Color.Transparent;
            this.werPercentField1.Enabled = false;
            this.werPercentField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werPercentField1.Location = new System.Drawing.Point(157, 366);
            this.werPercentField1.Name = "werPercentField1";
            this.werPercentField1.Size = new System.Drawing.Size(335, 72);
            this.werPercentField1.TabIndex = 2;
            this.werPercentField1.Text = "werPercentField1";
            this.werPercentField1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // TextFieldsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 588);
            this.Controls.Add(this.werPercentField1);
            this.Controls.Add(this.werTelephoneField1);
            this.Controls.Add(this.werTextField1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TextFieldsForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "TextFieldsForm";
            this.Load += new System.EventHandler(this.TextFieldsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerTextField werTextField1;
        private Toolkit.Controls.WerTelephoneField werTelephoneField1;
        private Toolkit.Controls.WerPercentField werPercentField1;
    }
}