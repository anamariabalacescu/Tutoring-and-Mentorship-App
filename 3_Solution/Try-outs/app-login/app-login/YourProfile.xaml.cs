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
    /// Interaction logic for YourProfile.xaml
    /// </summary>
    public partial class YourProfile : Window
    {
        private int id_user { get; set; }
        public void setId(int id) { this.id_user = id; }
        public YourProfile()
        {
            InitializeComponent();
        }

        private void Video_Lesson(object sender, RoutedEventArgs e)
        {
            var vid_les = new VideoLesson();
            vid_les.setId(this.id_user);
            vid_les.Show();
            Close();
        }

        private void Lessons(object sender, RoutedEventArgs e)
        {
            var les = new YourLessons();
            les.setId(this.id_user);
            les.Show();
            Close();
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            var set = new Settings();
            set.setId(this.id_user);
            set.Show();
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

        private void Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            GeneralCmds cmds = new GeneralCmds();

            string usertype = cmds.getUserType(id_user);
            string username = cmds.getUsername(id_user);
            string email = cmds.getEmail(id_user);
            string name = cmds.getSurname(id_user);
            string firstname = cmds.getFirstname(id_user);


            //TextBlock type = new TextBlock() { Text = usertype };
            TreeViewItem root = new TreeViewItem() { Header = usertype };

            //StackPanel stack = new StackPanel();
            //stack.Children.Add(new TextBlock() { Text = "Username: " + username });
            //stack.Children.Add(new TextBlock() { Text = "Email: " + email });

            root.Items.Add(new TreeViewItem() { Header = "Nume: " + name });
            root.Items.Add(new TreeViewItem() { Header = "Prenume: " + firstname });
            root.Items.Add(new TreeViewItem() { Header = "Username: " + username });
            root.Items.Add(new TreeViewItem() { Header = "Email: " + email });
            if (usertype == "profesor")
            {
                string profesie = cmds.getProfJob(id_user);
                root.Items.Add(new TreeViewItem() { Header = "Profesie de baza: " + profesie });
                //stack.Children.Add(new TextBlock() { Text = "Profesie de baza: " + profesie });
            } else if(usertype == "student") {
                string university = cmds.getUniversity(id_user);
                root.Items.Add(new TreeViewItem() { Header = "Universitate: " + university });
                //stack.Children.Add(new TextBlock() { Text = "Universitate: " + university });
            }

            UserData.Items.Add(root);

            foreach (TreeViewItem treeViewItem in UserData.Items)
            {
                if (treeViewItem.Header is TextBlock textBlock)
                {
                    textBlock.FontSize = 32; // Adjust FontSize as needed
                }
            }
            //StackPanel root = new StackPanel();
            //root.Children.Add(type);
            //root.Children.Add(stack);
            //grid.Children.Add(root);
        }
    }
}
