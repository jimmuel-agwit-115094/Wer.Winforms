using System;
using System.Windows.Forms;
using Wer.Winforms.Toolkit.Controls;

namespace Wer.Winforms.Demo
{
    public partial class UserForm : WerForm
    {
        private Users _user;

        public UserForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load user data into the form fields.
        /// </summary>
        public void LoadUser(Users user)
        {
            if (user == null) return;
            _user = user;

            txtId.Value = user.Id.ToString();
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone ?? "";
            txtRole.Text = user.Role;
            txtDepartment.Text = user.Department;
            txtStatus.Text = user.Status;
            dtDateJoined.Value = user.DateJoined;
            txtSalary.Value = user.Salary;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Update user object from form fields
            if (_user != null)
            {
                _user.FirstName = txtFirstName.Text;
                _user.LastName = txtLastName.Text;
                _user.Email = txtEmail.Text;
                _user.Phone = txtPhone.Text;
                _user.Role = txtRole.Text;
                _user.Department = txtDepartment.Text;
                _user.Status = txtStatus.Text;
                _user.DateJoined = dtDateJoined.Value ?? _user.DateJoined;
                _user.Salary = txtSalary.Value;
            }

            WerMessageBox.Success.Show("User saved successfully.");
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
        }
    }
}
