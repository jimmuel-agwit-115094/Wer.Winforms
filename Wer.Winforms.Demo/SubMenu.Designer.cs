namespace Wer.Winforms.Demo
{
    partial class SubMenu
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
            this.werHeading2 = new Wer.Winforms.Toolkit.Controls.WerHeading();
            this.werHeading1 = new Wer.Winforms.Toolkit.Controls.WerHeading();
            this.SuspendLayout();
            // 
            // werHeading2
            // 
            this.werHeading2.AutoSize = true;
            this.werHeading2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.werHeading2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.werHeading2.Location = new System.Drawing.Point(31, 25);
            this.werHeading2.Name = "werHeading2";
            this.werHeading2.Size = new System.Drawing.Size(137, 37);
            this.werHeading2.TabIndex = 8;
            this.werHeading2.Text = "Submenu";
            // 
            // werHeading1
            // 
            this.werHeading1.AutoSize = true;
            this.werHeading1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.werHeading1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.werHeading1.Location = new System.Drawing.Point(633, 404);
            this.werHeading1.Name = "werHeading1";
            this.werHeading1.Size = new System.Drawing.Size(137, 37);
            this.werHeading1.TabIndex = 9;
            this.werHeading1.Text = "Submenu";
            // 
            // SubMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.werHeading1);
            this.Controls.Add(this.werHeading2);
            this.Name = "SubMenu";
            this.Text = "SubMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Toolkit.Controls.WerHeading werHeading2;
        private Toolkit.Controls.WerHeading werHeading1;
    }
}