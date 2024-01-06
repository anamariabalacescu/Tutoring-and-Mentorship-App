using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public void setId(int id) { this.id_user = id; }
        public Window3()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        
        private void LoadData()
        {
            TutoringDataContext tut = new TutoringDataContext();

            GeneralCmds gen = new GeneralCmds();
            // Fetch Schedulings based on the provided id_std or id_prof
            var topProfessors = tut.Schedulings
                    .GroupBy(s => s.ID_Prof)
                    .Select(group => new
                    {
                        ProfessorID = group.Key,
                        SchedulingCount = group.Count()
                    })
                    .OrderByDescending(g => g.SchedulingCount)
                    .Take(10)
                    .ToList();

                // Now topProfessors contains the top 10 professors and their scheduling counts

                // Create a list to hold Professor objects
                var professorList = new List<Profesor>();

                foreach (var professorInfo in topProfessors)
                {
                    // Fetch professor details from the database using professorInfo.ProfessorID
                    var professorDetails = tut.Profesors
                        .Where(p => p.ID_Prof == professorInfo.ProfessorID)
                        .FirstOrDefault();

                    // Create a new Professor object
                    var professor = new Profesor
                    {
                        ID_Prof = (int)professorInfo.ProfessorID,
                        Nume = professorDetails?.Nume,
                        Prenume = professorDetails?.Prenume,
                        Profesie_de_baza = professorDetails?.Profesie_de_baza
                    };

                    // Add the Professor object to the list
                    professorList.Add(professor);
                }

                // Bind the list to the DataGrid
            topProfs.ItemsSource = professorList;
            topProfs.Items.Refresh();

            //for subjects we do the same

            var topSubjects = tut.Schedulings
                            .GroupBy(s => s.ID_Subj)
                            .Select(group => new
                            {
                                SubjectID = group.Key,
                                SchedulingCount = group.Count()
                            })
                            .OrderByDescending(g => g.SchedulingCount)
                            .Take(10)
                            .ToList();

            // Create a list to hold Subject objects
            var subjectList = new List<Subject>();

            foreach (var subjectInfo in topSubjects)
            {
                // Fetch subject details from the database using subjectInfo.SubjectID
                var subjectDetails = tut.Subjects
                    .Where(sub => sub.ID_Subj == subjectInfo.SubjectID)
                    .FirstOrDefault();

                // Create a new Subject object
                var subject = new Subject
                {
                    ID_Subj = (int)subjectInfo.SubjectID,
                    nume = subjectDetails?.nume,
                    // Add other properties as needed
                };

                // Add the Subject object to the list
                subjectList.Add(subject);
            }

            // Bind the list to the DataGrid
            topSubjs.ItemsSource = subjectList;

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
    }
}
