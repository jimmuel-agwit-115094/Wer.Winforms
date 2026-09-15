using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Wer.Winforms.Toolkit.Controls;

namespace Wer.Winforms.Demo
{
    public partial class SubForm : WerForm
    {
        private List<Users> _allUsers;

        public SubForm()
        {
            InitializeComponent();
            LoadUsers();
            dgUsers.SelectedRowChanged += (s, id) =>
        werLabel1.Text = "Selected: " + id;
        }

        private void LoadUsers()
        {
            _allUsers = new List<Users>
            {
                new Users { Id = 1, FirstName = "Juan", LastName = "Dela Cruz", Email = "juan@email.com", Role = "Admin", Department = "IT", Status = "Active", DateJoined = new DateTime(2023, 1, 15), Phone = "0917-123-4567", Salary = 45000.00m },
                new Users { Id = 2, FirstName = "Maria", LastName = "Santos", Email = "maria@email.com", Role = "Manager", Department = "HR", Status = "Active", DateJoined = new DateTime(2023, 3, 22), Phone = "0918-234-5678", Salary = 55000.00m },
                new Users { Id = 3, FirstName = "Pedro", LastName = "Reyes", Email = "pedro@email.com", Role = "Staff", Department = "Finance", Status = "Active", DateJoined = new DateTime(2023, 5, 10), Phone = "0919-345-6789", Salary = 32000.00m },
                new Users { Id = 4, FirstName = "Ana", LastName = "Garcia", Email = "ana@email.com", Role = "Staff", Department = "Marketing", Status = "Active", DateJoined = new DateTime(2023, 7, 1), Phone = "0920-456-7890", Salary = 35000.00m },
                new Users { Id = 5, FirstName = "Carlos", LastName = "Mendoza", Email = "carlos@email.com", Role = "Lead", Department = "IT", Status = "Active", DateJoined = new DateTime(2022, 11, 8), Phone = "0921-567-8901", Salary = 48000.00m },
                new Users { Id = 6, FirstName = "Sofia", LastName = "Cruz", Email = "sofia@email.com", Role = "Staff", Department = "Sales", Status = "Inactive", DateJoined = new DateTime(2024, 2, 14), Phone = "0922-678-9012", Salary = 30000.00m },
                new Users { Id = 7, FirstName = "Daniel", LastName = "Flores", Email = "daniel@email.com", Role = "Staff", Department = "IT", Status = "Active", DateJoined = new DateTime(2024, 4, 20), Phone = "0923-789-0123", Salary = 38000.00m },
                new Users { Id = 8, FirstName = "Angela", LastName = "Villanueva", Email = "angela@email.com", Role = "Manager", Department = "Operations", Status = "Active", DateJoined = new DateTime(2022, 8, 5), Phone = "0924-890-1234", Salary = 52000.00m },
                new Users { Id = 9, FirstName = "Kevin", LastName = "Lim", Email = "kevin@email.com", Role = "Staff", Department = "Finance", Status = "Blocked", DateJoined = new DateTime(2023, 9, 30), Phone = "0925-901-2345", Salary = 33000.00m },
                new Users { Id = 10, FirstName = "Patricia", LastName = "Tan", Email = "patricia@email.com", Role = "Admin", Department = "IT", Status = "Active", DateJoined = new DateTime(2022, 6, 18), Phone = "0926-012-3456", Salary = 50000.00m },
                new Users { Id = 11, FirstName = "Miguel", LastName = "Ramos", Email = "miguel@email.com", Role = "Staff", Department = "HR", Status = "Inactive", DateJoined = new DateTime(2024, 1, 7), Phone = "0927-123-4567", Salary = 31000.00m },
                new Users { Id = 12, FirstName = "Isabella", LastName = "Aquino", Email = "isabella@email.com", Role = "Lead", Department = "Marketing", Status = "Active", DateJoined = new DateTime(2023, 6, 25), Phone = "0928-234-5678", Salary = 46000.00m },
                new Users { Id = 13, FirstName = "Rafael", LastName = "Bautista", Email = "rafael@email.com", Role = "Staff", Department = "Sales", Status = "Active", DateJoined = new DateTime(2024, 3, 12), Phone = "0929-345-6789", Salary = 34000.00m },
                new Users { Id = 14, FirstName = "Camille", LastName = "Gonzales", Email = "camille@email.com", Role = "Staff", Department = "Operations", Status = "Blocked", DateJoined = new DateTime(2023, 10, 28), Phone = "0930-456-7890", Salary = 32000.00m },
                new Users { Id = 15, FirstName = "Jose", LastName = "Rivera", Email = "jose@email.com", Role = "Manager", Department = "IT", Status = "Active", DateJoined = new DateTime(2022, 4, 3), Phone = "0931-567-8901", Salary = 58000.00m },
            };

            dgUsers.PrimaryKeyColumn = "Id";
            dgUsers.ShowEditColumn = true;
            dgUsers.FieldOptions = new[] { "FirstName", "LastName", "Email", "Role", "Department", "Status", "DateJoined", "Salary" };
            dgUsers.TotalAmountColumn = "Salary";
            dgUsers.PageSize = 10;

            // Tab filter
            dgUsers.TabChanged += (s, args) => FilterByTab();
            FilterByTab();

            dgUsers.EditClicked += (s, args) =>
            {
                var user = _allUsers.FirstOrDefault(u => u.Id == (int)args.PrimaryKey);
                if (user == null) return;

                var form = new UserForm();
                form.LoadUser(user);
                form.ShowDialog();

                // Refresh grid after edit
                FilterByTab();
            };
        }

        private void FilterByTab()
        {
            List<Users> filtered;

            switch (dgUsers.TabSelected)
            {
                case 0: // Active
                    filtered = _allUsers.Where(u => u.Status == "Active").ToList();
                    break;
                case 1: // Inactive
                    filtered = _allUsers.Where(u => u.Status == "Inactive").ToList();
                    break;
                case 2: // Blocked
                    filtered = _allUsers.Where(u => u.Status == "Blocked").ToList();
                    break;
                default:
                    filtered = _allUsers;
                    break;
            }

            dgUsers.DataSource = filtered;
        }

        private void ButtonsForm_Load(object sender, EventArgs e)
        {
            werButtonOrange1.IconCode = WerIcons.Delete;
            werButtonOrange1.Text = "Add User";
        }


    }
}
