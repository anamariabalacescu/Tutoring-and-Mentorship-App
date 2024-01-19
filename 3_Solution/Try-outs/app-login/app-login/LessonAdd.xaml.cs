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
    /// Interaction logic for LessonAdd.xaml
    /// </summary>
    public partial class LessonAdd : Window
    {
        private int id_user { get; set; }
        public void setId(int id) { this.id_user = id; }
        public LessonAdd()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            TutoringEntities tut = new TutoringEntities();
            GeneralCmds gen = new GeneralCmds();
            var type = gen.getUserType(id_user);

            if (type == "student")
            {
                // Load teachers
                var teachers = tut.Profesors.Select(t => t.Nume).ToList();
                cboPROF.ItemsSource = teachers;

                // Load subjects
                var subjects = tut.Subjects.Select(s => s.nume).ToList();
                cboSUBJ.ItemsSource = subjects;
            }
            else if (type == "profesor")
            {
                label_prof.Content = "Students";

                // Load students
                var students = tut.Students.Select(s => s.Nume).ToList();
                cboPROF.ItemsSource = students;

                // Load subjects
                var subjects = tut.Subjects.Select(s => s.nume).ToList();
                cboSUBJ.ItemsSource = subjects;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (TutoringEntities tut = new TutoringEntities())
            {
                GeneralCmds gen = new GeneralCmds();

                var type = gen.getUserType(id_user);
                if (cboPROF.SelectedItem != null && cboSUBJ.SelectedItem != null && calendar.SelectedDate != null)
                {
                    if (type == "profesor")
                    {
                        int profId = gen.getProfID(id_user);

                        Scheduling newScheduling = new Scheduling
                        {
                            ID_Prof = profId,
                            ID_Std = ((Student)cboPROF.SelectedItem).ID_Std,
                            ID_Subj = ((Subject)cboSUBJ.SelectedItem).ID_Subj,
                            Programare = calendar.SelectedDate.Value,
                            StatusProgramare = "Pending",
                            ProgresSTD = 0
                        };

                        // Add the new scheduling to the Schedulings table
                        tut.Schedulings.Add(newScheduling);
                        tut.SaveChanges();
                    }
                    else if (type == "student")
                    {
                        int stdId = gen.getStdID(id_user);

                        Scheduling newScheduling = new Scheduling
                        {
                            ID_Std = stdId,
                            ID_Prof = ((Student)cboPROF.SelectedItem).ID_Std,
                            ID_Subj = ((Subject)cboSUBJ.SelectedItem).ID_Subj,
                            Programare = calendar.SelectedDate.Value,
                            StatusProgramare = "Pending",
                            EVALProf = 0
                        };

                        // Add the new scheduling to the Schedulings table
                        tut.Schedulings.Add(newScheduling);
                        tut.SaveChanges();
                    }
                }
            }
        }
    }
}
