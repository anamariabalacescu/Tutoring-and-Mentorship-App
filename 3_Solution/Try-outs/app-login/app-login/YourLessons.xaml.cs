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
    /// Interaction logic for YourLessons.xaml
    /// </summary>
    public partial class YourLessons : Window
    {
        private int id_user { get; set; }
        public void setId(int id) { this.id_user = id; }
        public YourLessons()
        {
            InitializeComponent();
        }
        public int GetID()
        {
            return id_user;
        }
        private void Profile(object sender, RoutedEventArgs e)
        {
            var prof = new YourProfile();
            prof.setId(this.id_user);
            prof.Show();
            Close();
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            var settings = new Settings(); 
            settings.setId(this.id_user);
            settings.Show();
            Close();
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
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void allLessons_Click(object sender, RoutedEventArgs e)
        {
            SchedulingsPage sp = new SchedulingsPage(id_user);
            Lessons_cmd.Content = sp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProgressPROF prog = new ProgressPROF(id_user);
            prog.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PendingLessonsPage pends = new PendingLessonsPage(id_user);
            Lessons_cmd.Content = pends;
            GeneralCmds gen = new GeneralCmds();
            string tip = gen.getUserType(id_user);
            if(tip == "profesor")
            {
                AddLessonProf lp = new AddLessonProf(id_user);
                lp.Show();
            }
            else if(tip == "student")
            {
                AddLessonSTD ls = new AddLessonSTD(id_user);
                ls.Show();
            }
        }

        private void Lessons(object sender, RoutedEventArgs e)
        {

        }

        private void Profs(object sender, RoutedEventArgs e)
        {
            var profs = new Profs();
            profs.Show();
            profs.setId(this.id_user);
            Close();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SearchBar.Text != null)
            {
                SearchPopUp s = new SearchPopUp(SearchBar.Text, id_user);
                s.Show();

            }
        }
    }
}
