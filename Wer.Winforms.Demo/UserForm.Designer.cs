namespace Wer.Winforms.Demo
{
    partial class UserForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtFirstName = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.txtLastName = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.txtEmail = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.txtRole = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.txtDepartment = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.txtStatus = new Wer.Winforms.Toolkit.Controls.WerTextField();
            this.txtPhone = new Wer.Winforms.Toolkit.Controls.WerPhoneField();
            this.dtDateJoined = new Wer.Winforms.Toolkit.Controls.WerDatePicker();
            this.txtSalary = new Wer.Winforms.Toolkit.Controls.WerCurrencyField();
            this.txtId = new Wer.Winforms.Toolkit.Controls.WerCopyTextField();
            this.btnSave = new Wer.Winforms.Toolkit.Controls.WerButtonPrimary();
            this.btnCancel = new Wer.Winforms.Toolkit.Controls.WerButtonOutlinedPrimary();
            this.SuspendLayout();
            // 
            // txtFirstName
            // 
            this.txtFirstName.BackColor = System.Drawing.Color.Transparent;
            this.txtFirstName.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtFirstName.LabelText = "First Name";
            this.txtFirstName.Location = new System.Drawing.Point(23, 118);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(408, 64);
            this.txtFirstName.TabIndex = 1;
            // 
            // txtLastName
            // 
            this.txtLastName.BackColor = System.Drawing.Color.Transparent;
            this.txtLastName.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtLastName.LabelText = "Last Name";
            this.txtLastName.Location = new System.Drawing.Point(455, 118);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(408, 64);
            this.txtLastName.TabIndex = 2;
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.Transparent;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtEmail.LabelText = "Email";
            this.txtEmail.Location = new System.Drawing.Point(23, 209);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(408, 61);
            this.txtEmail.TabIndex = 3;
            // 
            // txtRole
            // 
            this.txtRole.BackColor = System.Drawing.Color.Transparent;
            this.txtRole.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtRole.LabelText = "Role";
            this.txtRole.Location = new System.Drawing.Point(23, 301);
            this.txtRole.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(408, 61);
            this.txtRole.TabIndex = 5;
            // 
            // txtDepartment
            // 
            this.txtDepartment.BackColor = System.Drawing.Color.Transparent;
            this.txtDepartment.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtDepartment.LabelText = "Department";
            this.txtDepartment.Location = new System.Drawing.Point(455, 301);
            this.txtDepartment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(408, 61);
            this.txtDepartment.TabIndex = 6;
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.Color.Transparent;
            this.txtStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtStatus.LabelText = "Status";
            this.txtStatus.Location = new System.Drawing.Point(23, 392);
            this.txtStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(408, 61);
            this.txtStatus.TabIndex = 7;
            // 
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.Transparent;
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtPhone.LabelText = "Phone";
            this.txtPhone.Location = new System.Drawing.Point(455, 209);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(408, 61);
            this.txtPhone.TabIndex = 4;
            // 
            // dtDateJoined
            // 
            this.dtDateJoined.BackColor = System.Drawing.Color.Transparent;
            this.dtDateJoined.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dtDateJoined.LabelText = "Date Joined";
            this.dtDateJoined.Location = new System.Drawing.Point(455, 392);
            this.dtDateJoined.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtDateJoined.Name = "dtDateJoined";
            this.dtDateJoined.Size = new System.Drawing.Size(408, 61);
            this.dtDateJoined.TabIndex = 8;
            this.dtDateJoined.Value = null;
            // 
            // txtSalary
            // 
            this.txtSalary.BackColor = System.Drawing.Color.Transparent;
            this.txtSalary.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtSalary.LabelText = "Salary";
            this.txtSalary.Location = new System.Drawing.Point(23, 484);
            this.txtSalary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSalary.Name = "txtSalary";
            this.txtSalary.Size = new System.Drawing.Size(408, 60);
            this.txtSalary.TabIndex = 9;
            this.txtSalary.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.Color.Transparent;
            this.txtId.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtId.LabelText = "User ID";
            this.txtId.Location = new System.Drawing.Point(23, 26);
            this.txtId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(408, 59);
            this.txtId.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.BorderColor = System.Drawing.Color.Empty;
            this.btnSave.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(105)))), ((int)(((byte)(124)))));
            this.btnSave.Location = new System.Drawing.Point(455, 510);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(86)))), ((int)(((byte)(102)))));
            this.btnSave.Size = new System.Drawing.Size(140, 47);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.btnCancel.BorderWidth = 2;
            this.btnCancel.ButtonColor = System.Drawing.Color.White;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(124)))), ((int)(((byte)(146)))));
            this.btnCancel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.btnCancel.Location = new System.Drawing.Point(607, 510);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.btnCancel.Size = new System.Drawing.Size(140, 47);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 602);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtRole);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.dtDateJoined);
            this.Controls.Add(this.txtSalary);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UserForm";
            this.Padding = new System.Windows.Forms.Padding(19, 21, 19, 21);
            this.Text = "User Details";
            this.ResumeLayout(false);

        }

        #endregion

        private Toolkit.Controls.WerCopyTextField txtId;
        private Toolkit.Controls.WerTextField txtFirstName;
        private Toolkit.Controls.WerTextField txtLastName;
        private Toolkit.Controls.WerTextField txtEmail;
        private Toolkit.Controls.WerPhoneField txtPhone;
        private Toolkit.Controls.WerTextField txtRole;
        private Toolkit.Controls.WerTextField txtDepartment;
        private Toolkit.Controls.WerTextField txtStatus;
        private Toolkit.Controls.WerDatePicker dtDateJoined;
        private Toolkit.Controls.WerCurrencyField txtSalary;
        private Toolkit.Controls.WerButtonPrimary btnSave;
        private Toolkit.Controls.WerButtonOutlinedPrimary btnCancel;
    }
}
