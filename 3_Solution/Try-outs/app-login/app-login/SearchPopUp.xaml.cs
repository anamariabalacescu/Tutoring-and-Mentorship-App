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
    /// Interaction logic for SearchPopUp.xaml
    /// </summary>
    public partial class SearchPopUp : Window
    {
        private string profname { get; set; }
        public SearchPopUp(string profname)
        {
            InitializeComponent();
            this.profname = profname;
            LoadData();
        }
        private void LoadData()
        {
            TutoringDataContext tut = new TutoringDataContext();
            var prof = tut.Profesors.Where(p => p.Nume == profname || p.Prenume == profname || p.Nume + " " + p.Prenume == profname).FirstOrDefault();
            if(prof != null)
            {
                profName.Text = profname;
            }
            else
            {
                profName.Text = "No results for your search!";
            }
        }
        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
