using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for AddLessonSTD.xaml
    /// </summary>
    public partial class AddLessonSTD : Window
    {

        TutoringDataContext tut = new TutoringDataContext();
        private int id_usr { get; set; }
        private int id_prof { get; set; }  
        private int id_sub { get; set; }
        public AddLessonSTD(int id_usr)
        {
            this.id_usr = id_usr;
            InitializeComponent();
            AddSubjs();
        }
        private void AddSubjs()
        {
            var subjects = tut.Subjects.ToList();
            List<string> ss = new List<string>();
            foreach(var subject in subjects)
            {
                ss.Add(subject.nume);
            }
            cboSubject.ItemsSource = ss;
        }
        private void AddProfs()
        {
            var subjs_prof = tut.Taught_subjects.Where(ts => ts.ID_Subj == id_sub).ToList();
            List<int> idsp = new List<int>();
            foreach(var subj in subjs_prof)
            {
                idsp.Add(subj.ID_Prof);
            }
            
            List<string> ss = new List<string>();
            foreach (var idp in idsp)
            {
                var profs = tut.Profesors.Where(pr => pr.ID_Prof == idp).FirstOrDefault();
                ss.Add(profs.Nume + ' ' + profs.Prenume);
            }
            cboTeacher.ItemsSource = ss;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = date.SelectedDate ?? DateTime.MinValue;
            if (selectedDate != DateTime.MinValue && cboHour.SelectedItem != null)
            {
                string selectedHour = cboHour.SelectedItem.ToString();
                int hour;
                if (int.TryParse(selectedHour, out hour))
                {
                    DateTime selectedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, 0, 0);
                    Console.WriteLine("Data si ora selectate: " + selectedDateTime);

                    Scheduling newScheduling = new Scheduling
                    {
                        ID_Prof = id_prof,
                        ID_Std = id_usr,
                        ID_Subj = id_sub,
                        Programare = selectedDateTime,
                        StatusProgramare = "Pending",
                        ProgresSTD = null,
                        EVALProf = null
                    };
                    try
                    {
                        tut.Schedulings.InsertOnSubmit(newScheduling);
                        tut.SubmitChanges();
                    }
                    catch
                    {

                        Done d = new Done();
                        d.SuccessMessage = "Lesson added! Good luck learning!\n";
                        d.Show();
                    }
                }
                else
                {
                    Console.WriteLine("Ora selectata nu este valida.");
                }
            }
            else
            {
                Error er = new Error();
                er.ErrorMessage = "Date not selected!\n";
                er.Show();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void subSelect(object sender, SelectionChangedEventArgs e)
        {
            string numeSubj = cboSubject.Text;
            id_sub = tut.Subjects.Where(s => s.nume == numeSubj).FirstOrDefault().ID_Subj;
            AddProfs();
        }

        private void profSelect(object sender, SelectionChangedEventArgs e)
        {
            string profName = cboTeacher.Text;
            id_prof = tut.Profesors.Where(p => p.Nume + " " + p.Prenume == profName).FirstOrDefault().ID_Prof;
        }
    }
}
