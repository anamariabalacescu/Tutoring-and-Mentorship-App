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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        private int id_user { get; set; }
        public Window3()
        {
            InitializeComponent();
        }

        public void setId(int id) { this.id_user = id; }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            var confirmationDialog = new LogoutConfirmationDialog();
            confirmationDialog.ShowDialog();

            if (confirmationDialog.UserConfirmed)
            {
                MainWindow main = new MainWindow();
                main.Show(); // Navigate to the main window
                Close();
            }
            // If UserConfirmed is false, do nothing, and stay on the current page.
        }

        private void Profile(object sender, RoutedEventArgs e)
        {
            var profile = new YourProfile();
            profile.setId(this.id_user);
            profile.Show();
            Close();
        }

        private void Lessons(object sender, RoutedEventArgs e)
        {
            var les = new YourLessons();
            les.Show();
            les.setId(this.id_user);
            Close();
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            var settings = new Settings();
            settings.Show();
            settings.setId(this.id_user);
            Close();
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
