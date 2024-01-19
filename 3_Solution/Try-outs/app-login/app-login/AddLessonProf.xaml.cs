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
    /// Interaction logic for AddLessonProf.xaml
    /// </summary>
    public partial class AddLessonProf : Window
    {
        TutoringEntities tut = new TutoringEntities();
        private string subj { get; set; }
        private int id_usr { get; set; }
        private int id_prof { get; set; }
        public AddLessonProf(int idUsr)
        {
            id_usr = idUsr;
            id_prof = tut.Profesors.Where(x => x.ID_User == idUsr).FirstOrDefault().ID_Prof;
            InitializeComponent();
        }

        private void newSubj(object sender, TextChangedEventArgs e)
        {
            subj = subject.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Verificăm dacă există materia
            // Verificăm dacă o are profesorul
            var sub = tut.Subjects.FirstOrDefault(s => s.nume == subj);
            if (sub != null)
            {
                // Există subiectul în baza de date fie de la prof actual fie de la un alt prof
                // Verificăm dacă are deja materia predată
                var profS = tut.Taught_subjects.FirstOrDefault(t => t.ID_Subj == sub.ID_Subj && t.ID_Prof == id_prof);
                if (profS != null)
                {
                    Error er = new Error();
                    er.ErrorMessage = "You are teaching this subject already! Please choose a new one\n";
                    er.Show();
                }
                else
                {
                    try
                    {
                        Taught_subjects ts = new Taught_subjects
                        {
                            ID_Subj = sub.ID_Subj,
                            ID_Prof = id_prof
                        };

                        tut.Taught_subjects.Add(ts);
                        tut.SaveChanges();

                        Done d = new Done();
                        d.SuccessMessage = "Subject added! Good luck teaching!\n";
                        d.Show();
                    }
                    catch
                    {
                        HandleError();
                    }
                }
            }
            else
            {
                // Subiectul este nou în baza de date. Se adaugă în ambele tabele
                Subject snew = new Subject { nume = subj };
                try
                {
                    tut.Subjects.Add(snew);
                    tut.SaveChanges();

                    Done d = new Done();
                    d.SuccessMessage = "Subject added! Good luck teaching!\n";
                    d.Show();
                }
                catch
                {
                    HandleError();
                }

                // Adăugăm în taught_subj
                try
                {
                    Taught_subjects ts = new Taught_subjects
                    {
                        ID_Subj = snew.ID_Subj,
                        ID_Prof = id_prof
                    };

                    tut.Taught_subjects.Add(ts);
                    tut.SaveChanges();

                    Done d = new Done();
                    d.SuccessMessage = "Subject added! Good luck teaching!\n";
                    d.Show();
                }
                catch
                {
                    HandleError();
                }
            }
        }

        private void HandleError()
        {
            Error er = new Error();
            er.ErrorMessage = "Could not upload your subject!\n";
            er.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
