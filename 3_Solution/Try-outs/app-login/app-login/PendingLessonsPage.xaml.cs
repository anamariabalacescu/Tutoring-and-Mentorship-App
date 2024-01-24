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
        TutoringEntities tut = new TutoringEntities();
        private int id_usr {  get; set; }
        public PendingLessonsPage(int id_usr)
        {
            this.id_usr = id_usr;
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            GeneralCmds gen = new GeneralCmds();

            string userType = gen.getUserType(id_usr);
            if (userType == "profesor")
            {

                int prog_id = gen.getProfID(id_usr);

                var sched = tut.Schedulings.Where(s => s.ID_Prof == prog_id & s.StatusProgramare == "pending").Take(10).ToList();

                myDataGrid.ItemsSource = sched;
            }
            else
            {

                int prog_id = gen.getStdID(id_usr);

                var sched = tut.Schedulings
                    .Where(s => s.ID_Std == prog_id & s.StatusProgramare == "pending")
                    .Take(10)
                    .ToList();

                myDataGrid.ItemsSource = sched;
            }
        }


        private void doubleclick(object sender, MouseButtonEventArgs e)
        {
            if (myDataGrid.SelectedItem != null)
            {
                GeneralCmds gen = new GeneralCmds();
                string userType = gen.getUserType(id_usr);
                if (userType == "profesor")
                {
                    Scheduling selectedScheduling = (Scheduling)myDataGrid.SelectedItem;
                    selectedScheduling.StatusProgramare = (selectedScheduling.StatusProgramare != "active") ? "active" : "pending";

                    using (var tut = new TutoringEntities())
                    {

                        var userInDb = tut.Schedulings.FirstOrDefault(u => u.ID_Meeting == selectedScheduling.ID_Meeting);

                        if (userInDb != null)
                        {
                            userInDb.StatusProgramare = selectedScheduling.StatusProgramare;
                        }

                        try
                        {
                            // Submit changes to the database
                            tut.SaveChanges();

                            // If no exception occurred, changes were successfully submitted
                            Done d = new Done();
                            d.SuccessMessage = "Status changed successfully!";
                            d.Show();
                        }
                        catch (Exception ex)
                        {
                            // Handle exceptions if any occur during the submission
                            Error er = new Error();
                            er.ErrorMessage = "Error submitting status changes";
                        }
                    }
                }
            }
        }
    }
}
