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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace app_login
{
    /// <summary>
    /// Interaction logic for PageProgress.xaml
    /// </summary>
    public partial class PageProgress : Page
    {
        TutoringDataContext tut = new TutoringDataContext();
        GeneralCmds gen = new GeneralCmds();
        private int id_user {  get; set; }
        private int id_sub { get; set; }
        public PageProgress(int id_user)
        {
            InitializeComponent();
            this.id_user = id_user;

            Add_Subj();
        }

        private void LoadData()
        {
            var subject = subjects.SelectedItem?.ToString();
            var collaborator = collabs.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(collaborator))
            {
                int id_subject = tut.Subjects.FirstOrDefault(s => s.nume == subject)?.ID_Subj ?? 0;

                if (gen.getUserType(id_user) == "student")
                {
                    var meetingsInfo = tut.Schedulings
                        .Where(s => s.ID_Subj == id_subject && s.ID_Std == gen.getStdID(id_user))
                        .Select(s => new
                        {
                            EvalProf = s.EVALProf ?? 0,
                            ProgresStd = s.ProgresSTD ?? 0,
                            Programare = (s.Programare >= SqlDateTime.MinValue.Value && s.Programare <= SqlDateTime.MaxValue.Value) ? s.Programare : SqlDateTime.MinValue.Value
                        })
                        .ToList();

                    progress.ItemsSource = meetingsInfo;
                }
                else if (gen.getUserType(id_user) == "profesor")
                {
                    var meetingsInfo = tut.Schedulings
                        .Where(s => s.ID_Subj == id_subject && s.ID_Prof == gen.getProfID(id_user))
                        .Select(s => new
                        {
                            EvalProf = s.EVALProf ?? 0,
                            ProgresStd = s.ProgresSTD ?? 0,
                            Programare = (s.Programare >= SqlDateTime.MinValue.Value && s.Programare <= SqlDateTime.MaxValue.Value) ? s.Programare : SqlDateTime.MinValue.Value
                        })
                        .ToList();

                    progress.ItemsSource = meetingsInfo;
                }
            }
        }

        private void Add_Subj()
        {
            List<string> list = new List<string>();
            if (gen.getUserType(id_user) == "profesor")
            {
                var id_prof = gen.getProfID(id_user);
                var subj = tut.Schedulings.Where(s => s.ID_Prof == id_prof).ToList(); // iau toate materiile predate de un profesor
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
            else
            {
                var id_stud = gen.getStdID(id_user);
                var subj = tut.Schedulings.Where(s => s.ID_Std == id_stud).ToList(); // iau toate materiile predate la un student
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
            subjects.ItemsSource = list;
        }

        private void AddProfs_Studs()
        {
            List<string> list = new List<string>();
            if (gen.getUserType(id_user) == "profesor")
            {
                var id_prof = gen.getProfID(id_user);
                var profs = tut.Schedulings.Where(s => s.ID_Prof == id_prof).ToList();
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
            else if (gen.getUserType(id_user) == "student")
            {
                var id_stud = gen.getStdID(id_user);
                var studs = tut.Schedulings.Where(s => s.ID_Std == id_stud).ToList();
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

            collabs.ItemsSource = list;
        }

        private void SubjectsSelection(object sender, SelectionChangedEventArgs e)
        {
            string numeSubj = subjects.SelectedItem.ToString();
            id_sub = tut.Subjects.Where(s => s.nume == numeSubj).FirstOrDefault().ID_Subj;
            AddProfs_Studs();
        }

        private void collabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }
    }
}
