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
    /// Interaction logic for Feedback.xaml
    /// </summary>
    public partial class Feedback : Window
    {
        private int id_user { get; set; }
        private int selected_star_index = -1;
        public void setId(int id) { this.id_user = id; }

        public Feedback()
        {
            InitializeComponent();
        }
        private Polygon selectedStar = null;
        private void Star_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var star = sender as Polygon;
            ResetStars();
            star.Fill = Brushes.Yellow;
            // Obține index-ul stelei curente
            int currentIndex = Convert.ToInt32(star.Name.Substring(4));

            // Parcurge toate stelele și colorează-le dacă sunt mai la stânga decât steaua curentă
            for (int i = 1; i < currentIndex; i++)
            {
                var previousStar = FindName($"star{i}") as Polygon;
                if (previousStar != null)
                {
                    previousStar.Fill = Brushes.Yellow;
                }
            }
        }

        private void Star_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var star = sender as Polygon;

            // Verifica daca steaua curenta este si steaua selectata
            if (star != selectedStar)
            {
                star.Fill = Brushes.White;

                // Obtine index-ul stelei curente
                int currentIndex = Convert.ToInt32(star.Name.Substring(4));

                // Parcurge toate stelele si coloreaza-le daca sunt mai la stanga decat steaua curenta
                for (int i = 1; i < currentIndex; i++)
                {
                    var previousStar = FindName($"star{i}") as Polygon;
                    if (previousStar != null)
                    {
                        previousStar.Fill = Brushes.White;
                    }
                }
            }
        }
        private void DontSendButton_Click(object sender, RoutedEventArgs e)
        {
            Done d = new Done();
            d.SuccessMessage = "Feedback not sent!\n";
            d.Show();
            ResetStars();
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            Done d = new Done();
            d.SuccessMessage = "Feedback sent!\n";
            d.Show();

            TutoringDataContext tut = new TutoringDataContext();

            GeneralCmds gen = new GeneralCmds();

            string type = gen.getUserType(id_user);
            int id;
            if(type == "profesor")
            {
                id = gen.getProfID(id_user);
                var subj = tut.Schedulings.Where(s => s.ID_Prof == id && s.ProgresSTD == null).FirstOrDefault();
                subj.ProgresSTD = selected_star_index;
                tut.SubmitChanges();
            }
            else
            {
                id = gen.getStdID(id_user);
                var subj = tut.Schedulings.Where(s => s.ID_Std == id && s.EVALProf == null).FirstOrDefault();
                subj.EVALProf = selected_star_index;
                tut.SubmitChanges();
            }



            ResetStars();

            var profile = new YourProfile();
            profile.setId(this.id_user);
            profile.Show();
        }

        private void Star_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var star = sender as Polygon;

            if (star != null)
            {
                selectedStar = star;

                // Setează culoarea stelei selectate la galben
                star.Fill = Brushes.Yellow;

                int currentIndex = Convert.ToInt32(star.Name.Substring(4));

                // Parcurge toate stelele și colorează-le dacă sunt mai la stânga sau sunt chiar steaua curentă
                for (int i = 1; i <= currentIndex; i++)
                {
                    var currentStar = FindName($"star{i}") as Polygon;
                    if (currentStar != null)
                    {
                        currentStar.Fill = Brushes.Yellow;
                    }
                }
                selected_star_index = currentIndex;
            }

        }
        private void ResetStars()
        {
            for (int i = 1; i <= 5; i++)
            {
                var star = FindName($"star{i}") as Polygon;
                if (star != null)
                {
                    star.Fill = Brushes.White;
                }
            }

            selectedStar = null;
        }
    }
}
