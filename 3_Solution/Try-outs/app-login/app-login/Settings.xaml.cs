using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace app_login
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private int id_user { get; set; }
        public void setId(int id) { this.id_user = id; }
        public Settings()
        {
            InitializeComponent();
        }

        private void Profile(object sender, RoutedEventArgs e)
        {
            var prof = new YourProfile();
            prof.setId(id_user);
            prof.Show();
            Close();
        }

        private void Lessons(object sender, RoutedEventArgs e)
        {
            var les = new YourLessons();
            les.setId(id_user);
            les.Show();
            Close();
        }

        private void MenuItem_LogOut(object sender, RoutedEventArgs e)
        {
            var confirmationDialog = new LogoutConfirmationDialog();
            confirmationDialog.ShowDialog();

            if (confirmationDialog.UserConfirmed)
            {
                MainWindow main = new MainWindow();
                main.Show(); // Navigate to the main window
                Close();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void pass_Click(object sender, RoutedEventArgs e)
        {
            ChangePwd changePwd = new ChangePwd();
            changePwd.SetID(id_user);
            Settings_type.Content = changePwd;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeEmail changeEmail = new ChangeEmail();
            changeEmail.SetID(id_user);
            Settings_type.Content = changeEmail;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeUser changeUser = new ChangeUser();
            changeUser.SetID(id_user);
            Settings_type.Content = changeUser;
        }

        private void DeleteAcc(object sender, RoutedEventArgs e)
        {
            TutoringDataContext tut = new TutoringDataContext();

            GeneralCmds gen = new GeneralCmds();

            string type = gen.getUserType(id_user);

            if (type == "student")
            {
                var stdlist = tut.Students.Where(s => s.ID_User == id_user).FirstOrDefault();

                tut.Students.DeleteOnSubmit(stdlist);
            }
            else if (type == "profesor")
            {
                var proflist = tut.Profesors.Where(s => s.ID_User == id_user).FirstOrDefault();

                tut.Profesors.DeleteOnSubmit(proflist);
            }

            var userDelete = tut.Users.Where(s=> s.ID_User == id_user).FirstOrDefault();
            tut.Users.DeleteOnSubmit(userDelete);

            try
            {
                // Submit changes to the database
                tut.SubmitChanges();

                // If no exception occurred, changes were successfully submitted
                Done d = new Done();
                d.SuccessMessage = "Account deleted successfully!";
                d.Show();

                MainWindow login = new MainWindow();
                login.Show();
                Close();
            }
            catch (Exception ex)
            {
                // Handle exceptions if any occur during the submission
                Error er = new Error();
                er.ErrorMessage = "Error deleting account";
                er.Show();
            }

        }

        private void Home(object sender, RoutedEventArgs e)
        {
            var home = new Window3();
            home.setId(id_user);
            home.Show();
            Close();
        }

        private void SubjectsClick(object sender, RoutedEventArgs e)
        {
            var subj = new Subjects();
            subj.setId(id_user);
            subj.Show();
            Close();
        }

        private void ProfsClick(object sender, RoutedEventArgs e)
        {
            var profs = new Profs();
            profs.setId(id_user);
            profs.Show();
            Close();
        }
    }
}
