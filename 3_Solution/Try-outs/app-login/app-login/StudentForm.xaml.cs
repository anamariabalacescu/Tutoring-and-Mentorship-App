using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using System.Data.Common;
using System.Data;

namespace app_login
{
    /// <summary>
    /// Interaction logic for StudentForm.xaml
    /// </summary>
    public partial class StudentForm : Window
    {
        private string userName = "";
        private string userPass = "";
        private string sname = "";
        private string fname = "";
        private string mail = "";
        private string uni = "";

        SqlConnection conn;
        public StudentForm()
        {
            InitializeComponent();
            InitConn();
        }

        void InitConn()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Tutoring"].ToString();
            conn = new SqlConnection(connectionString);
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
                else if (textBox == university)
                {
                    textBox.Text = "Insert university";
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
                else if (textBox == password)
                {
                    userPass = textBox.Text;
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
                else if (textBox == university)
                {
                    uni = textBox.Text;
                }
        }

        private void onclick(object sender, RoutedEventArgs e)
        {
            //try
            //{
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Students WHERE Username = @Username";
                cmd.Parameters.AddWithValue("@Username", username.Text);

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                Window2 eroare = new Window2();
                eroare.Show();
                //Console.WriteLine("Username already exists");
                }
                else
                {
                    SqlCommand insertCmd = conn.CreateCommand();
                    insertCmd.CommandText = "INSERT INTO Students (Username) VALUES (@Username)";
                    insertCmd.Parameters.AddWithValue("@Username", username.Text);
                    //insertCmd.Parameters.AddWithValue("@Password", password.Text);
                    //insertCmd.Parameters.AddWithValue("@Nume",.Text);
                    //insertCmd.Parameters.AddWithValue("@Prenume", username.Text);
                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Successfully inserted the new user
                        Console.WriteLine("User inserted successfully");
                    }
                    else
                    {
                        // Failed to insert user
                        Console.WriteLine("Failed to insert user");
                    }
                }
            /*}
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }*/
            conn.Close();
        }

    }

}
