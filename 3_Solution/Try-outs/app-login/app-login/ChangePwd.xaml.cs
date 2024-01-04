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
    /// Interaction logic for ChangePwd.xaml
    /// </summary>
    public partial class ChangePwd : Page
    {
        private string oldpwd { get; set; }
        private string newpwd { get; set; }
        private string newpwd2 { get; set; }
        private int id_usr { get; set; }

        public ChangePwd()
        {
            InitializeComponent();
        }
        public void SetID(int id)
        {
            this.id_usr = id;
        }

        private void OnPasswordBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == oldpass)
            {
                placeholderOld.Visibility = Visibility.Collapsed;
            }
            if (sender == newpass)
            {
                placeholderNew.Visibility = Visibility.Collapsed;
            }
            if (sender == newpassAgain)
            {
                placeholderNew2.Visibility = Visibility.Collapsed;
            }
        }

        private void OnPasswordBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender == oldpass)
            {
                if (string.IsNullOrEmpty(oldpass.Password))
                {
                    placeholderOld.Visibility = Visibility.Visible;
                }
            }
            if (sender == newpass)
            {
                if (string.IsNullOrEmpty(newpass.Password))
                {
                    placeholderNew.Visibility = Visibility.Visible;
                }
            }
            if (sender == newpassAgain)
            {
                if (string.IsNullOrEmpty(newpassAgain.Password))
                {
                    placeholderNew2.Visibility = Visibility.Visible;
                }
            }
        }

        private void input(object sender, RoutedEventArgs e)
        {
            if (sender == oldpass)
            {
                oldpwd = oldpass.Password;
            }
            if (sender == newpass)
            {
                newpwd = newpass.Password;
            }
            if (sender == newpassAgain)
            {
                newpwd2 = newpassAgain.Password;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (newpwd == newpwd2 && newpwd != oldpwd)
            {
                UserModel userModel = new UserModel(id_usr);
                userModel.UpdateUserPassword(id_usr, oldpwd, newpwd);
            }
        }

        private void OnPlaceholderGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == oldpass)
            {
                if (string.IsNullOrEmpty(oldpass.Password))
                {
                    placeholderOld.Visibility = Visibility.Collapsed;
                    oldpass.Focus();
                }
            }
            if (sender == newpass)
            {
                if (string.IsNullOrEmpty(newpass.Password))
                {
                    placeholderNew.Visibility = Visibility.Collapsed;
                    newpass.Focus();
                }
            }
            if (sender == newpassAgain)
            {
                if (string.IsNullOrEmpty(newpassAgain.Password))
                {
                    placeholderNew2.Visibility = Visibility.Collapsed;
                    newpassAgain.Focus();
                }
            }
        }
    }
}
