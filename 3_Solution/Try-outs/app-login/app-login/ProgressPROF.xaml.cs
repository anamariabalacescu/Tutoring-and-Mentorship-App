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
    /// Interaction logic for ProgressPROF.xaml
    /// </summary>
    public partial class ProgressPROF : Window
    {
        TutoringDataContext tut = new TutoringDataContext();
        GeneralCmds gen = new GeneralCmds();
        private int id_usr { get; set; }
        private int id_sub {  get; set; }
        private int id_prof { get; set; }
        private string type {  get; set; }
        public ProgressPROF(int id_usr)
        {
            InitializeComponent();


            type = gen.getUserType(id_usr);
            if (type == "student")
            {
                lbl.Content = "Teacher";
            }

            AddSubjs();
        }
        private void AddSubjs()
        {
            var subjects = tut.Schedulings.ToList();
            List<string> ss = new List<string>();
            foreach (var subject in subjects)
            {
                var subj = tut.Subjects.Where(pr => pr.ID_Subj == subject.ID_Subj).FirstOrDefault();
                ss.Add(subj.nume);
            }
            cboSubject.ItemsSource = ss;
        }
        private void AddProfs_Studs()
        {

            List<string> list = new List<string>();
            if (type == "student") 
            {
                int id_std = gen.getStdID(id_usr);

                var profs = tut.Schedulings.Where(s => s.ID_Std == id_std);
                foreach (var idp in profs)
                {
                    var studs = tut.Students.Where(pr => pr.ID_Std == idp.ID_Std).FirstOrDefault();
                    list.Add(studs.Nume + ' ' + studs.Prenume);
                }

            }
            else if(type == "profesor")
            {
                int id_prof = gen.getProfID(id_usr);


                var studs = tut.Schedulings.Where(s => s.ID_Prof == id_prof);
                foreach (var idp in studs)
                {
                    var profs = tut.Profesors.Where(pr => pr.ID_Prof == idp.ID_Prof).FirstOrDefault();
                    list.Add(profs.Nume + ' ' + profs.Prenume);
                }
            }

            cboTeacher.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var subject = cboSubject.SelectedItem.ToString();
            var colab = cboTeacher.SelectedItem.ToString();


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
            id_prof = tut.Profesors.Where(p => p.Nume + " " + p.Prenume == profName).FirstOrDefault().ID_Prof;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
