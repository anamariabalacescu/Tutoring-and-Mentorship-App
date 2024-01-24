using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    /// Interaction logic for ProgressPROF.xaml
    /// </summary>
    public partial class ProgressPROF : Window
    {
        TutoringEntities tut = new TutoringEntities();
        GeneralCmds gen = new GeneralCmds();
        private int id_usr { get; set; }
        private int id_sub {  get; set; }
        private int id_prof { get; set; }
        private int id_std { get; set; }
        private string type {  get; set; }
        public int EvalProf { get; set; }
        public int ProgresStd { get; set; }
        public DateTime Programare { get; set; }
        public ProgressPROF(int id_usr)
        {
            InitializeComponent();


            type = gen.getUserType(id_usr);
            if (type == "student")
            {
                lbl.Content = "Teacher";
                id_std = gen.getStdID(id_usr);
            }
            else
            {
                id_prof = gen.getProfID(id_usr);
            }

            AddSubjs();
        }
        private void AddSubjs()
        {
            List<string> list = new List<string>();
            if(type == "profesor")
            {
                var subj = tut.Schedulings.Where(s => s.ID_Prof == id_prof).ToList(); // iau toate materiile predate de un profesor
                foreach( var subj2 in subj)
                {
                    var ss2 = tut.Subjects.Where(sj => sj.ID_Subj == subj2.ID_Subj).FirstOrDefault();
                    string subjectName = ss2.nume;

                    // Verificați dacă elementul există deja în listă
                    if (!list.Contains(subjectName))
                    {
                        list.Add(subjectName);
                    }
                }
            }
            else
            {
                var subj = tut.Schedulings.Where(s => s.ID_Std == id_std).ToList(); // iau toate materiile predate la un student
                foreach (var subj2 in subj)
                {
                    var ss2 = tut.Subjects.Where(sj => sj.ID_Subj == subj2.ID_Subj).FirstOrDefault();
                    string subjectName = ss2.nume;

                    // Verificați dacă elementul există deja în listă
                    if (!list.Contains(subjectName))
                    {
                        list.Add(subjectName);
                    }
                }
            }
            cboSubject.ItemsSource = list;
        }

        private void AddProfs_Studs()
        {
            List<string> list = new List<string>();
            if (type == "profesor") 
            {
                var profs = tut.Schedulings.Where(s => s.ID_Prof == id_prof && s.StatusProgramare=="active").ToList();
                foreach (var idp in profs)
                {
                    var studs = tut.Students.Where(pr => pr.ID_Std == idp.ID_Std).FirstOrDefault();
                    string professorName = studs.Nume + ' ' + studs.Prenume;

                    // Verificați dacă elementul există deja în listă
                    if (!list.Contains(professorName))
                    {
                        list.Add(professorName);
                    }
                }

            }
            else if(type == "student")
            {
                var studs = tut.Schedulings.Where(s => s.ID_Std == id_std && s.StatusProgramare == "active").ToList();
                foreach (var idp in studs)
                {
                    var profs = tut.Profesors.Where(pr => pr.ID_Prof == idp.ID_Prof).FirstOrDefault();
                    string professorName = profs.Nume + ' ' + profs.Prenume;

                    if (!list.Contains(professorName))
                    {
                        list.Add(professorName);
                    }
                }
            }

            cboTeacher.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var subject = cboSubject.SelectedItem?.ToString();
            var collaborator = cboTeacher.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(collaborator))
            {
                int id_subject = tut.Subjects.FirstOrDefault(s => s.nume == subject)?.ID_Subj ?? 0;

                if (type == "student")
                {
                    var meetingsInfo = tut.Schedulings
                        .Where(s => s.ID_Subj == id_subject && s.ID_Std == id_std)
                        .Select(s => new
                        {
                            ProgresStd = s.ProgresSTD ?? 0,
                            Programare = (s.Programare >= SqlDateTime.MinValue.Value && s.Programare <= SqlDateTime.MaxValue.Value) ? s.Programare : SqlDateTime.MinValue.Value,
                            Mesaj = s.MessageStd ?? ""
                        })
                        .ToList();

                    gridEvaluari.ItemsSource = meetingsInfo;
                }
                else if (type == "profesor")
                {
                    var meetingsInfo = tut.Schedulings
                        .Where(s => s.ID_Subj == id_subject && s.ID_Prof == id_prof)
                        .Select(s => new
                        {
                            EvalProf = s.EVALProf ?? 0,
                            Programare = (s.Programare >= SqlDateTime.MinValue.Value && s.Programare <= SqlDateTime.MaxValue.Value) ? s.Programare : SqlDateTime.MinValue.Value,
                            Mesaj = s.MessageProf ?? ""
                        })
                        .ToList();

                    gridEvaluari.ItemsSource = meetingsInfo;
                }
            }
        }


        private void subSelect(object sender, SelectionChangedEventArgs e)
        {
            string numeSubj = cboSubject.SelectedItem.ToString();
            id_sub = tut.Subjects.Where(s => s.nume == numeSubj).FirstOrDefault().ID_Subj;
            AddProfs_Studs();
        }

        private void studSelect(object sender, SelectionChangedEventArgs e)
        {

            string profName = cboTeacher.SelectedItem.ToString();
            if (type == "student")
                id_prof = tut.Profesors.Where(p => p.Nume + " " + p.Prenume == profName).FirstOrDefault().ID_Prof;
            else if (type == "profesor")
                id_std = tut.Students.Where(p => p.Nume + " " + p.Prenume == profName).FirstOrDefault().ID_Std;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
