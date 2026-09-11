namespace Wer.Winforms.Demo
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Wer.Winforms.Toolkit.Controls.WerButton werButton1;
        private Wer.Winforms.Toolkit.Controls.WerButtonDisabled werButtonDisabled1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.werButton1 = new Wer.Winforms.Toolkit.Controls.WerButton();
            this.werButtonDisabled1 = new Wer.Winforms.Toolkit.Controls.WerButtonDisabled();
            this.werButtonWarning1 = new Wer.Winforms.Toolkit.Controls.WerButtonWarning();
            this.SuspendLayout();
            // 
            // werButton1
            // 
            this.werButton1.BorderColor = System.Drawing.Color.Empty;
            this.werButton1.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.werButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werButton1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werButton1.ForeColor = System.Drawing.Color.White;
            this.werButton1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(105)))), ((int)(((byte)(124)))));
            this.werButton1.Location = new System.Drawing.Point(33, 31);
            this.werButton1.Name = "werButton1";
            this.werButton1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(86)))), ((int)(((byte)(102)))));
            this.werButton1.Size = new System.Drawing.Size(107, 30);
            this.werButton1.TabIndex = 0;
            this.werButton1.Text = "Button";
            //
            // werButtonDisabled1
            //
            this.werButtonDisabled1.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.werButtonDisabled1.BorderWidth = 1;
            this.werButtonDisabled1.ButtonColor = System.Drawing.Color.White;
            this.werButtonDisabled1.Cursor = System.Windows.Forms.Cursors.Default;
            this.werButtonDisabled1.Enabled = false;
            this.werButtonDisabled1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werButtonDisabled1.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.werButtonDisabled1.HoverColor = System.Drawing.Color.White;
            this.werButtonDisabled1.Location = new System.Drawing.Point(150, 31);
            this.werButtonDisabled1.Name = "werButtonDisabled1";
            this.werButtonDisabled1.PressedColor = System.Drawing.Color.White;
            this.werButtonDisabled1.Size = new System.Drawing.Size(100, 36);
            this.werButtonDisabled1.TabIndex = 2;
            this.werButtonDisabled1.Text = "Button";
            //
            // werButtonWarning1
            // 
            this.werButtonWarning1.BorderColor = System.Drawing.Color.Empty;
            this.werButtonWarning1.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.werButtonWarning1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werButtonWarning1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werButtonWarning1.ForeColor = System.Drawing.Color.White;
            this.werButtonWarning1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.werButtonWarning1.Location = new System.Drawing.Point(205, 31);
            this.werButtonWarning1.Name = "werButtonWarning1";
            this.werButtonWarning1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.werButtonWarning1.Size = new System.Drawing.Size(203, 41);
            this.werButtonWarning1.TabIndex = 1;
            this.werButtonWarning1.Text = "werButtonWarning1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.werButtonWarning1);
            this.Controls.Add(this.werButtonDisabled1);
            this.Controls.Add(this.werButton1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wer.Winforms Demo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerButtonWarning werButtonWarning1;
    }
}
