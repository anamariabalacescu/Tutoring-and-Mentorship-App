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
    /// Interaction logic for DataStd.xaml
    /// </summary>
    public partial class DataStd : Page
    {
        private string pwd { get; set; }
        public DataStd()
        {
            InitializeComponent();
        }

        private void OnPasswordBoxGotFocus(object sender, RoutedEventArgs e)
        {
            placeholderNew2.Visibility = Visibility.Collapsed;
        }

        private void OnPasswordBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pass.Password))
            {
                placeholderNew2.Visibility = Visibility.Visible;
            }
        }
        private void OnPlaceholderGotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pass.Password))
            {
                placeholderNew2.Visibility = Visibility.Collapsed;
                pass.Focus();
            }
        }

        private void input(object sender, RoutedEventArgs e)
        {
            pwd = pass.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
