namespace Wer.Winforms.Demo
{
    partial class DForm
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
            this.werButtonOrange1 = new Wer.Winforms.Toolkit.Controls.WerButtonOrange();
            this.werTextField1 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.werTextField2 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.werTextField3 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.SuspendLayout();
            // 
            // werButtonOrange1
            // 
            this.werButtonOrange1.BorderColor = System.Drawing.Color.Empty;
            this.werButtonOrange1.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.werButtonOrange1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werButtonOrange1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werButtonOrange1.ForeColor = System.Drawing.Color.White;
            this.werButtonOrange1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(107)))), ((int)(((byte)(28)))));
            this.werButtonOrange1.Location = new System.Drawing.Point(155, 293);
            this.werButtonOrange1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.werButtonOrange1.Name = "werButtonOrange1";
            this.werButtonOrange1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(88)))), ((int)(((byte)(23)))));
            this.werButtonOrange1.Size = new System.Drawing.Size(220, 52);
            this.werButtonOrange1.TabIndex = 0;
            this.werButtonOrange1.Text = "werButtonOrange1";
            // 
            // werTextField1
            // 
            this.werTextField1.BackColor = System.Drawing.Color.Transparent;
            this.werTextField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField1.LabelText = "TextField Label";
            this.werTextField1.Location = new System.Drawing.Point(26, 16);
            this.werTextField1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.werTextField1.Name = "werTextField1";
            this.werTextField1.Size = new System.Drawing.Size(350, 64);
            this.werTextField1.TabIndex = 1;
            this.werTextField1.Text = "werTextField1";
            // 
            // werTextField2
            // 
            this.werTextField2.BackColor = System.Drawing.Color.Transparent;
            this.werTextField2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField2.LabelText = "TextField Label";
            this.werTextField2.Location = new System.Drawing.Point(23, 88);
            this.werTextField2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.werTextField2.Name = "werTextField2";
            this.werTextField2.Size = new System.Drawing.Size(350, 64);
            this.werTextField2.TabIndex = 2;
            this.werTextField2.Text = "werTextField2";
            // 
            // werTextField3
            // 
            this.werTextField3.BackColor = System.Drawing.Color.Transparent;
            this.werTextField3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField3.LabelText = "TextField Label";
            this.werTextField3.Location = new System.Drawing.Point(23, 160);
            this.werTextField3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.werTextField3.Name = "werTextField3";
            this.werTextField3.Size = new System.Drawing.Size(350, 63);
            this.werTextField3.TabIndex = 3;
            this.werTextField3.Text = "werTextField3";
            // 
            // DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 409);
            this.Controls.Add(this.werTextField3);
            this.Controls.Add(this.werTextField2);
            this.Controls.Add(this.werTextField1);
            this.Controls.Add(this.werButtonOrange1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "Details";
            this.Load += new System.EventHandler(this.DForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerButtonOrange werButtonOrange1;
        private Toolkit.Controls.WerTextField werTextField1;
        private Toolkit.Controls.WerTextField werTextField2;
        private Toolkit.Controls.WerTextField werTextField3;
    }
}