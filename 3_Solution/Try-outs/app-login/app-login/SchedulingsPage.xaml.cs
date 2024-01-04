using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
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
    /// Interaction logic for SchedulingsPage.xaml
    /// </summary>
    public partial class SchedulingsPage : Page
    {
        private int id_usr { get; set; }
        public SchedulingsPage(int id)
        {
            id_usr = id;
            InitializeComponent();
            LoadData();
        }
        public SchedulingsPage()
        {
            InitializeComponent();
            LoadData();
        }
        public void SetId(int id) { this.id_usr = id; }
        private void LoadData()
        {
            TutoringDataContext tut = new TutoringDataContext();

            GeneralCmds gen = new GeneralCmds();
            string usertype = gen.getUserType(id_usr);

            if (usertype == "student")
            {
                int idToFetch = gen.getStdID(id_usr);
                var schedulings = tut.Schedulings
                    .Where(s => s.ID_Std == idToFetch)
                    .Select(s => new
                    {
                        ProfessorName = tut.Profesors
                            .Where(p => p.ID_Prof == s.ID_Prof)
                            .Select(p => p.Nume + ' ' + p.Prenume)
                            .FirstOrDefault(),
                        StudentName = tut.Students
                            .Where(st => st.ID_Std == s.ID_Std)
                            .Select(st => st.Nume + ' ' + st.Prenume)
                            .FirstOrDefault(),
                        SubjectName = tut.Subjects
                            .Where(sub => sub.ID_Subj == s.ID_Subj)
                            .Select(sub => sub.nume)
                            .FirstOrDefault(),
                        Scheduled_Date = DateTime.Parse(s.Scheduled_Date)
                    })
                    .ToList();

                myDataGrid.ItemsSource = schedulings;
            }
            else if (usertype == "profesor")
            {
                int idToFetch = gen.getProfID(id_usr);
                var schedulings = tut.Schedulings
                    .Where(s => s.ID_Prof == idToFetch)
                    .Select(s => new
                    {
                        ProfessorName = tut.Profesors
                            .Where(p => p.ID_Prof == s.ID_Prof)
                            .Select(p => p.Nume + ' ' + p.Prenume)
                            .FirstOrDefault(),
                        StudentName = tut.Students
                            .Where(st => st.ID_Std == s.ID_Std)
                            .Select(st => st.Nume + ' ' + st.Prenume)
                            .FirstOrDefault(),
                        SubjectName = tut.Subjects
                            .Where(sub => sub.ID_Subj == s.ID_Subj)
                            .Select(sub => sub.nume)
                            .FirstOrDefault(),
                        ScheduledDate = s.Scheduled_Date
                    })
                    .ToList();

                myDataGrid.ItemsSource = schedulings;
            }
        }

    }
}
