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
    /// Interaction logic for ChangeUser.xaml
    /// </summary>
    public partial class ChangeUser : Page
    {
        private string _user { get; set; }
        private string _newuser { get; set; }
        private string pwd { get; set; }
        private int id_usr { get; set; }
        public ChangeUser()
        {
            InitializeComponent();
        }
        public void SetID(int id)
        {
            this.id_usr = id;
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == email)
            {
                _user = email.Text;
            }
            else if (sender == newEmail)
            {
                _newuser = newEmail.Text;
            }
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
            if (_user != _newuser)
            {
                UserModel userModel = new UserModel(id_usr);
                userModel.UpdateUsername(id_usr, _user, _newuser, pwd);
            }
            else
            {
                Error er = new Error();
                er.ErrorMessage = "Username is the same.";
                er.Show();
            }
        }
    }
}
