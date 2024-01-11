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
        TutoringDataContext tut = new TutoringDataContext();
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
            //verif daca exista materia 
            //verificam daca o are profesorul
            var sub = tut.Subjects.Where(s => s.nume == subj).FirstOrDefault();
            if (sub != null)
            {
                //exista subiectul in baza de date fie de la prof actual fie de la un alt prof
                //verificam daca are deja materia predata
                var profS = tut.Taught_subjects.Where(t => t.ID_Subj == sub.ID_Subj && t.ID_Prof == id_prof).FirstOrDefault();
                if(profS != null)
                {
                    Error er = new Error();
                    er.ErrorMessage = "You are teaching this subject already! Please choose a new one\n";
                    er.Show();
                }
                else
                {
                    try
                    {
                        Taught_subject ts = new Taught_subject();
                        ts.ID_Subj = sub.ID_Subj;
                        ts.ID_Prof = id_prof;
                        tut.Taught_subjects.InsertOnSubmit(ts);
                        tut.SubmitChanges();

                        Done d = new Done();
                        d.SuccessMessage = "Subject added! Good luck teaching!\n";
                        d.Show();
                    }
                    catch
                    {
                        Error er = new Error();
                        er.ErrorMessage = "Could not upload your subject!\n";
                        er.Show();
                    }
                }
            }
            else
            {
                //subiectul este nou in baza de date =. se adauga in ambele tabele
                Subject snew = new Subject();
                snew.nume = subj;
                try
                {
                    tut.Subjects.InsertOnSubmit(snew);
                    tut.SubmitChanges();

                    Done d = new Done();
                    d.SuccessMessage = "Subject added! Good luck teaching!\n";
                    d.Show();
                }
                catch
                {
                    Error er = new Error();
                    er.ErrorMessage = "Could not upload your subject!\n";
                    er.Show();
                }
                //adding to taight_subj
                try
                {
                    Taught_subject ts = new Taught_subject();
                    ts.ID_Subj = snew.ID_Subj;
                    ts.ID_Prof = id_prof;
                    tut.Taught_subjects.InsertOnSubmit(ts);
                    tut.SubmitChanges();

                    Done d = new Done();
                    d.SuccessMessage = "Subject added! Good luck teaching!\n";
                    d.Show();
                }
                catch
                {
                    Error er = new Error();
                    er.ErrorMessage = "Could not upload your subject!\n";
                    er.Show();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
