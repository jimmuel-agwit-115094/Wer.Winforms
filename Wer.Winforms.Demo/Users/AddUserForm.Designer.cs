namespace Wer.Winforms.Demo
{
    partial class AddUserForm
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
            this.werButtonPrimary1 = new Wer.Winforms.Toolkit.Controls.WerButtonPrimary();
            this.werPhoneField1 = new Wer.Winforms.Toolkit.Controls.WerPhoneField();
            this.werRichTextField1 = new Wer.Winforms.Toolkit.Controls.WerRichTextField();
            this.werTextField3 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.werTextField2 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.werTextField1 = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.werGroupBox1 = new Wer.Winforms.Toolkit.Controls.WerGroupBox();
            this.werGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // werButtonPrimary1
            // 
            this.werButtonPrimary1.BorderColor = System.Drawing.Color.Empty;
            this.werButtonPrimary1.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.werButtonPrimary1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.werButtonPrimary1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.werButtonPrimary1.ForeColor = System.Drawing.Color.White;
            this.werButtonPrimary1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(105)))), ((int)(((byte)(124)))));
            this.werButtonPrimary1.Location = new System.Drawing.Point(335, 440);
            this.werButtonPrimary1.Name = "werButtonPrimary1";
            this.werButtonPrimary1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(86)))), ((int)(((byte)(102)))));
            this.werButtonPrimary1.Size = new System.Drawing.Size(123, 36);
            this.werButtonPrimary1.TabIndex = 5;
            this.werButtonPrimary1.Text = "Save";
            // 
            // werPhoneField1
            // 
            this.werPhoneField1.BackColor = System.Drawing.Color.Transparent;
            this.werPhoneField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werPhoneField1.LabelText = "Phone Number";
            this.werPhoneField1.Location = new System.Drawing.Point(25, 356);
            this.werPhoneField1.Name = "werPhoneField1";
            this.werPhoneField1.Size = new System.Drawing.Size(433, 60);
            this.werPhoneField1.TabIndex = 4;
            // 
            // werRichTextField1
            // 
            this.werRichTextField1.BackColor = System.Drawing.Color.Transparent;
            this.werRichTextField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werRichTextField1.LabelText = "Address";
            this.werRichTextField1.Location = new System.Drawing.Point(25, 269);
            this.werRichTextField1.MaxLength = 2147483647;
            this.werRichTextField1.Name = "werRichTextField1";
            this.werRichTextField1.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1033{\\fonttbl{\\f0\\fnil Segoe UI;}" +
    "}\r\n{\\colortbl ;\\red33\\green37\\blue41;}\r\n{\\*\\generator Riched20 10.0.26100}\\viewk" +
    "ind4\\uc1 \r\n\\pard\\cf1\\f0\\fs20\\par\r\n}\r\n";
            this.werRichTextField1.Size = new System.Drawing.Size(433, 81);
            this.werRichTextField1.TabIndex = 3;
            // 
            // werTextField3
            // 
            this.werTextField3.BackColor = System.Drawing.Color.Transparent;
            this.werTextField3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField3.LabelText = "Lastname";
            this.werTextField3.Location = new System.Drawing.Point(25, 137);
            this.werTextField3.Name = "werTextField3";
            this.werTextField3.Size = new System.Drawing.Size(433, 60);
            this.werTextField3.TabIndex = 2;
            // 
            // werTextField2
            // 
            this.werTextField2.BackColor = System.Drawing.Color.Transparent;
            this.werTextField2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField2.LabelText = "Middlename";
            this.werTextField2.Location = new System.Drawing.Point(25, 203);
            this.werTextField2.Name = "werTextField2";
            this.werTextField2.Size = new System.Drawing.Size(433, 60);
            this.werTextField2.TabIndex = 1;
            // 
            // werTextField1
            // 
            this.werTextField1.BackColor = System.Drawing.Color.Transparent;
            this.werTextField1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.werTextField1.LabelText = "Firstname";
            this.werTextField1.Location = new System.Drawing.Point(25, 71);
            this.werTextField1.Name = "werTextField1";
            this.werTextField1.Size = new System.Drawing.Size(433, 60);
            this.werTextField1.TabIndex = 0;
            // 
            // werGroupBox1
            // 
            this.werGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.werGroupBox1.BodyBackColor = System.Drawing.Color.White;
            this.werGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(210)))), ((int)(((byte)(220)))));
            this.werGroupBox1.Controls.Add(this.werButtonPrimary1);
            this.werGroupBox1.Controls.Add(this.werPhoneField1);
            this.werGroupBox1.Controls.Add(this.werRichTextField1);
            this.werGroupBox1.Controls.Add(this.werTextField3);
            this.werGroupBox1.Controls.Add(this.werTextField2);
            this.werGroupBox1.Controls.Add(this.werTextField1);
            this.werGroupBox1.DividerColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.werGroupBox1.HeaderBackColor = System.Drawing.Color.White;
            this.werGroupBox1.HeadingText = "Add User";
            this.werGroupBox1.Location = new System.Drawing.Point(9, 11);
            this.werGroupBox1.Name = "werGroupBox1";
            this.werGroupBox1.Padding = new System.Windows.Forms.Padding(8, 68, 8, 8);
            this.werGroupBox1.Size = new System.Drawing.Size(488, 501);
            this.werGroupBox1.SubheadingText = "Manage Users And Credentials";
            this.werGroupBox1.TabIndex = 0;
            // 
            // AddUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 527);
            this.Controls.Add(this.werGroupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AddUserForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "Manage";
            this.Load += new System.EventHandler(this.AddUserForm_Load);
            this.werGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Toolkit.Controls.WerRichTextField werRichTextField1;
        private Toolkit.Controls.WerTextField werTextField3;
        private Toolkit.Controls.WerTextField werTextField2;
        private Toolkit.Controls.WerTextField werTextField1;
        private Toolkit.Controls.WerPhoneField werPhoneField1;
        private Toolkit.Controls.WerButtonPrimary werButtonPrimary1;
        private Toolkit.Controls.WerGroupBox werGroupBox1;
    }
}