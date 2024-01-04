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
    /// Interaction logic for YourLessons.xaml
    /// </summary>
    public partial class YourLessons : Window
    {
        private int id_user { get; set; }
        public void setId(int id) { this.id_user = id; }
        public YourLessons()
        {
            InitializeComponent();
        }
        public int GetID()
        {
            return id_user;
        }
        private void Profile(object sender, RoutedEventArgs e)
        {
            var prof = new YourProfile();
            prof.setId(this.id_user);
            prof.Show();
            Close();
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            var settings = new Settings(); 
            settings.setId(this.id_user);
            settings.Show();
            Close();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            var confirmationDialog = new LogoutConfirmationDialog();
            confirmationDialog.ShowDialog();

            if (confirmationDialog.UserConfirmed)
            {
                MainWindow main = new MainWindow();
                main.Show(); // Navigate to the main window
                Close();
            }
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void allLessons_Click(object sender, RoutedEventArgs e)
        {
            SchedulingsPage sp = new SchedulingsPage(id_user);
            Lessons_cmd.Content = sp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
