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
    /// Interaction logic for Profs.xaml
    /// </summary>
    public partial class Profs : Window
    {
        private int id_user { get; set; }
        public void setId(int id) { this.id_user = id; }
        public Profs()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            TutoringDataContext tut = new TutoringDataContext();

            // Fetch all entries from the Subjects table
            var allProfesors = tut.Profesors.ToList();

            // Bind the list to the DataGrid
            profs.ItemsSource = allProfesors;
            profs.Items.Refresh();
        }
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

        private void Close(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Home(object sender, RoutedEventArgs e)
        {
            var home = new Window3();
            home.Show();
            home.setId(this.id_user);
            Close();
        }

        private void Subjects(object sender, RoutedEventArgs e)
        {
            var subj = new Subjects();
            subj.Show();
            subj.setId(this.id_user);
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SearchBar.Text != null)
            {
                SearchPopUp s = new SearchPopUp(SearchBar.Text);
                s.Show();

            }
        }
    }
}
