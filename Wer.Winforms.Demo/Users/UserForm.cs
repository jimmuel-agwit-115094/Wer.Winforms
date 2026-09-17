using System.Windows.Forms;
using Wer.Winforms.Toolkit.Controls;

namespace Wer.Winforms.Demo
{
    public partial class UserForm : WerForm
    {
        public UserForm()
        {
            InitializeComponent();
        }

        public void LoadUser(Users user)
        {
            if (user == null) return;
            dgUsers.DataSource = new[] { user };
        }

        private void dgUsers_Load(object sender, System.EventArgs e)
        {

        }
    }
}
