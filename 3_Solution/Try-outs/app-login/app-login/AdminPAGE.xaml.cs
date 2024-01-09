using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace app_login
{
    /// <summary>
    /// Interaction logic for AdminPAGE.xaml
    /// </summary>
    public partial class AdminPAGE : Window
    {
        public AdminPAGE()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            TutoringDataContext tut = new TutoringDataContext();

            var professors = tut.Profesors.ToList();
            Professors.ItemsSource = professors;

            var students = tut.Students.ToList();
            Students.ItemsSource = students;

            var userAccounts = tut.Users.ToList();
            accounts.ItemsSource = userAccounts;
        }

        private void accounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (accounts.SelectedItem != null)
            {
                User selectedUser = (User)accounts.SelectedItem;

                selectedUser.UserStatus = (selectedUser.UserStatus != "Active") ? "Active" : "Inactive";

                using (TutoringDataContext tut = new TutoringDataContext())
                {
                    var userInDb = tut.Users.FirstOrDefault(u => u.ID_User == selectedUser.ID_User);
                    if (userInDb != null)
                    {
                        userInDb.UserStatus = selectedUser.UserStatus;
                        tut.SubmitChanges();
                    }
                }

                accounts.Items.Refresh();
            }
        }
    }
}
