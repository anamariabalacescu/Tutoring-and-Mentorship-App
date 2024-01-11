using System;
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
        private int id_adm { get; set; }
        public AdminPAGE(int id_adm)
        {
            this.id_adm = id_adm;
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
                selectedUser.UserStatus = (selectedUser.UserStatus != "active") ? "active" : "inactive";
                TutoringDataContext tut = new TutoringDataContext();
                var userInDb = tut.Users.FirstOrDefault(u => u.ID_User == selectedUser.ID_User);
                if (userInDb != null)
                {
                    userInDb.UserStatus = selectedUser.UserStatus;
                }
                try
                {
                    // Submit changes to the database
                    tut.SubmitChanges();

                    // If no exception occurred, changes were successfully submitted
                    Done d = new Done();
                    d.SuccessMessage = "Status changed successfully!";
                    d.Show();
                }
                catch (Exception ex)
                {
                    // Handle exceptions if any occur during the submission
                    Error er = new Error();
                    er.ErrorMessage = "Error submitting status changes";
                }
            }
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string encrpass = EncryptionMachine.Encrypt("Licenta2023!");
            if (accounts.SelectedItem != null)
            {
                User selectedUser = (User)accounts.SelectedItem;
                selectedUser.UserPassword = encrpass;
                TutoringDataContext tut = new TutoringDataContext();
                var userInDb = tut.Users.FirstOrDefault(u => u.ID_User == selectedUser.ID_User);
                if (userInDb != null)
                {
                    userInDb.UserPassword = selectedUser.UserPassword;
                }
                try
                {
                    // Submit changes to the database
                    tut.SubmitChanges();

                    // If no exception occurred, changes were successfully submitted
                    Done d = new Done();
                    d.SuccessMessage = "Status changed successfully!";
                    d.Show();
                }
                catch (Exception ex)
                {
                    // Handle exceptions if any occur during the submission
                    Error er = new Error();
                    er.ErrorMessage = "Error submitting status changes";
                }
            }
        }
    }
}
