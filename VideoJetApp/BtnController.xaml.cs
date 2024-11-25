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
using VideoJetApp.ViewModels;

namespace VideoJetApp
{
    /// <summary>
    /// Interaction logic for BtnController.xaml
    /// </summary>
    public partial class BtnController : Window
    {
        public BtnController()
        {
            InitializeComponent();
          //  this.DataContext = new VideoJetApp.ViewModels.DashboardViewModel(`);
          this .DataContext = new BtnViewModel();
        }
    }
}
