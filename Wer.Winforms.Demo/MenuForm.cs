using System;
using System.Windows.Forms;
using Wer.Winforms.Toolkit.Controls;

namespace Wer.Winforms.Demo
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
            werLeftNavMenu1.ContentPanel = panel1;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
        }

        private void werMenuButton1_Click(object sender, EventArgs e)
        {
            werLeftNavMenu1.ShowForm<UsersForm>();
        }

        private void werMenuButton2_Click(object sender, EventArgs e)
        {
            // werLeftNavMenu1.ShowForm<CustomerForm>();
        }

        private void werMenuButton3_Click(object sender, EventArgs e)
        {
            // werLeftNavMenu1.ShowForm<SettingsForm>();
        }
    }
}
