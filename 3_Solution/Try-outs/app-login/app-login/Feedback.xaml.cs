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

            // Verifică dacă steaua curentă este și steaua selectată
            if (star != selectedStar)
            {
                star.Fill = Brushes.White;

                // Obține index-ul stelei curente
                int currentIndex = Convert.ToInt32(star.Name.Substring(4));

                // Parcurge toate stelele și colorează-le dacă sunt mai la stânga decât steaua curentă
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
            MessageBox.Show("Feedback not sent.", "Feedback Not Sent");
            ResetStars();
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Feedback sent.", "Feedback Sent");
            ResetStars();
            Close();
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
