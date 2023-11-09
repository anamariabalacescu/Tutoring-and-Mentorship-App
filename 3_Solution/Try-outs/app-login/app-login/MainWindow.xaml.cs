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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 register = new Window1();  // Create an instance of Window1
            register.Show();  // Show Window1
            Close();  // Close the current window
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window3 home = new Window3();  // Send to home page - Window3
            home.Show();  // Show Window3
            Close();  // Close the current window3
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void userInput(object sender, TextChangedEventArgs e)
        {
            if (sender == user)
            {
                string userInputUsername = user.Text;
            }
            else if (sender == password)
            {
                string userInputPassword = password.Text;
            }
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Insert username" || textBox.Text == "Insert password")
            {
                textBox.Text = "";
            }
        }

        private void RestoreText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "user")
                {
                    textBox.Text = "Insert username";
                }
                else if (textBox.Name == "password")
                {
                    textBox.Text = "Insert password";
                }
            }
        }

    }
}
