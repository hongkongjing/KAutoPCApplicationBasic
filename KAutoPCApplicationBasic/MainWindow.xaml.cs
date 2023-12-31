using KAutoPCApplicationBasic.ViewModel;
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

namespace KAutoPCApplicationBasic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel MainViewModels;
        public MainWindow()
        {
            InitializeComponent();
            MainViewModels = new MainViewModel();
            this.DataContext = MainViewModels;
        }

        private void BtnLoadDevice_Click(object sender, RoutedEventArgs e)
        {
            MainViewModels.GetDevice3();
            
        }

        private void btnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            MainViewModels.ScreenShotOne();
        }
    }
}
