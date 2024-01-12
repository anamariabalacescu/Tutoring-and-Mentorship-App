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
    /// Interaction logic for PageSchedule.xaml
    /// </summary>
    public partial class PageSchedule : Page
    {
        private int id_sch { get; set; } //either student or prof - doesn't matter - will be uploaded in the Yourlessons
        public PageSchedule()
        {
            InitializeComponent();
        }
        void LoadData()
        {
            
        }
    }
}
