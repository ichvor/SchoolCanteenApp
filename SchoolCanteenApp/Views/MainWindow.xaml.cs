using SchoolCanteenApp.ViewModels;
using System.Windows;

namespace SchoolCanteenApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}