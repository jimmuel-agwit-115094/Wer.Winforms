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
            this.werTextField1 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.SuspendLayout();
            // 
            // werTextField1
            // 
            this.werTextField1.BackColor = System.Drawing.Color.Transparent;
            this.werTextField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField1.LabelText = "TextField Label";
            this.werTextField1.Location = new System.Drawing.Point(231, 185);
            this.werTextField1.Name = "werTextField1";
            this.werTextField1.Size = new System.Drawing.Size(350, 60);
            this.werTextField1.TabIndex = 0;
            this.werTextField1.Text = "werTextField1";
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 415);
            this.Controls.Add(this.werTextField1);
            this.Name = "DetailsForm";
            this.Text = "Details";
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerTextField werTextField1;
    }
}