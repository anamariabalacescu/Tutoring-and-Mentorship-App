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
            prof.Show();
            Close();
        }

        private void Lessons(object sender, RoutedEventArgs e)
        {
            var les = new YourLessons();
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
    }
}
