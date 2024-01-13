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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace app_login
{
    /// <summary>
    /// Interaction logic for PendingLessonsPage.xaml
    /// </summary>
    public partial class PendingLessonsPage : Page
    {
        TutoringDataContext tut = new TutoringDataContext();
        private int id_usr {  get; set; }
        public PendingLessonsPage(int id_usr)
        {
            this.id_usr = id_usr;
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            GeneralCmds gen = new GeneralCmds();

            int prog_id = gen.getProfID(id_usr);

            var sched = tut.Schedulings.Where(s => s.ID_Prof == prog_id && s.StatusProgramare == "Pending").ToList();

            pending.ItemsSource = sched;
        }

        private void Change(object sender, MouseButtonEventArgs e)
        {
            if (pending.SelectedItem != null && pending.SelectedItem is Scheduling scheduling)
            {
                if (scheduling.StatusProgramare == "Denied")
                {
                    scheduling.StatusProgramare = "Accepted";
                }
                else if (scheduling.StatusProgramare == "Accepted")
                {
                    scheduling.StatusProgramare = "Denied";
                }

                pending.Items.Refresh();
            }
        }
    }
}
