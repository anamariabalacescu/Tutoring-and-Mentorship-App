using System;
using System.Collections.Generic;
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
        public ProfRegister()
        {
            InitializeComponent();
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
                }
                else if (selectedRole == "Student")
                {
                    // Navigate to the StudentWindow
                    StudentForm studentWindow = new StudentForm();
                    studentWindow.Show();
                }
                else if (selectedRole == "Professor")
                {
                    // Navigate to the ProfessorWindow
                    //ProfRegister professorWindow = new ProfRegister();
                    //professorWindow.Show();
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
                else if (textBox == password)
                {
                    textBox.Text = "Insert password";
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
    }
}
