﻿using System;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
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
                    //AdminWindow adminWindow = new AdminWindow();
                    //adminWindow.Show();
                }
                else if (selectedRole == "Student")
                {
                    // Navigate to the StudentWindow
                    //StudentWindow studentWindow = new StudentWindow();
                    //studentWindow.Show();
                }
                else if (selectedRole == "Professor")
                {
                    // Navigate to the ProfessorWindow
                    ProfRegister professorWindow = new ProfRegister();
                    professorWindow.Show();
                }
                else
                {
                    // Handle other cases or show an error message
                }
            }
        }
    }
}
