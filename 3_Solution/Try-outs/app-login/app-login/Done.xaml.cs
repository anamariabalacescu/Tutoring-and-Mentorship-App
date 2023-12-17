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
    /// Interaction logic for Done.xaml
    /// </summary>
    public partial class Done : Window
    {
        public Done()
        {
            InitializeComponent();
            DataContext = this; //current instance
        }
        public static readonly DependencyProperty SuccessMessageProperty = DependencyProperty.Register("SuccessMessage", typeof(string), typeof(Error));

        public string SuccessMessage
        {
            get { return (string)GetValue(SuccessMessageProperty); }
            set { SetValue(SuccessMessageProperty, value); }
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            if (sender == home)
            {
                Close();
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            else if (sender == exit)
            {
                Close();
            }
        }
    }
}
