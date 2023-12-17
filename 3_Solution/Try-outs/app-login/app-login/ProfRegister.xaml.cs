using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO.Packaging;
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
    /// Interaction logic for ProfRegister.xaml
    /// </summary>
    public partial class ProfRegister : Window
    {
        private string userName = ""; 
        private string userPass = "";
        private string sname = "";
        private string fname = "";
        private string mail = "";
        private string prof = "";
        SqlConnection conn;
        public ProfRegister()
        {
            InitializeComponent();
            //InitConn();
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {

            Application.Current.Shutdown();
        }

        private void roleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)roles.SelectedItem;

            if (selectedItem != null)
            {
                string selectedRole = selectedItem.Content.ToString();

                // Determine which window to navigate to based on the selected role
                if (selectedRole == "Admin")
                {
                    // Navigate to the AdminWindow
                    AdminForm adminWindow = new AdminForm();
                    adminWindow.Show();
                    Close();
                }
                else if (selectedRole == "Student")
                {
                    // Navigate to the StudentWindow
                    StudentForm studentWindow = new StudentForm();
                    studentWindow.Show();
                    Close();
                }
                else if (selectedRole == "Professor")
                {
                    // Already here
                }
                else
                {
                    //Window1 window1 = new Window1();
                    //window1.Show();
                }
            }
        }

        private void EraseText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
        }

        private void InitialText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox == username)
                {
                    textBox.Text = "Insert username";
                }
                else if (textBox == email)
                {
                    textBox.Text = "Insert email address";
                }
                else if (textBox == surname)
                {
                    textBox.Text = "Insert Surname";
                }
                else if (textBox == firstname)
                {
                    textBox.Text = "Insert Firstname";
                }
                else if (textBox == profession)
                {
                    textBox.Text = "Insert profession";
                }
            }
        }
        private void userInput(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox == username)
            {
                userName = textBox.Text;
            }
            else if (textBox == email)
            {
                mail = textBox.Text;
            }
            else if (textBox == surname)
            {
                sname = textBox.Text;
            }
            else if (textBox == firstname)
            {
                fname = textBox.Text;
            }
            else if (textBox == profession)
            {
                prof = textBox.Text;
            }
        }

        private void onclick(object sender, RoutedEventArgs e)
        {

            bool count = FormValidationRules.IsValidUsername(userName);

            if (count)
            {
                Window2 eroare = new Window2();
                eroare.Show();
                //Console.WriteLine("Username already exists");
            }
            else
            {
                bool passValid = FormValidationRules.IsValidPassword(userPass);

                if (passValid)
                {
                    bool emailValid = FormValidationRules.IsValidEmail(mail);
                    if (emailValid)
                    {
                        UserModel u = new UserModel(userName, userPass, mail, "profesor");
                        int id_p = u.UserInsert(userName, userPass, mail);

                        ProfessorModel p = new ProfessorModel(id_p,sname, fname, prof);
                        int result = p.InsertProfesor(id_p, sname, fname, prof);

                        if(result < 0)
                        {
                            Error error = new Error();
                            error.ErrorMessage = "Couldn't register Professor";
                            error.Show();
                        } else{
                            Done done = new Done();
                            done.SuccessMessage = "Student registered!";
                            done.Show();
                        }
                    }
                    else
                    {
                        Error errorWindow = new Error();
                        errorWindow.ErrorMessage = "Email is not valid. Please provide a valid email.";
                        errorWindow.Show();
                    }
                }
                else
                {
                    Error errorWindow = new Error();
                    errorWindow.ErrorMessage = "Weak Password! Minimum 8 characters - 1 upper, 1 lower, 1 digit. Please try again!";
                    errorWindow.Show();
                }
            }
        }
        private void OnPasswordBoxGotFocus(object sender, RoutedEventArgs e)
        {
            placeholderText.Visibility = Visibility.Collapsed;
        }

        private void OnPasswordBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(password.Password))
            {
                placeholderText.Visibility = Visibility.Visible;
            }
        }

        private void OnPlaceholderGotFocus(object sender, RoutedEventArgs e)
        {
            placeholderText.Visibility = Visibility.Collapsed;
            password.Focus();
        }

        private void userInput(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;
            userPass = pb.Password;
        }
        private void goHome(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
    }
}
