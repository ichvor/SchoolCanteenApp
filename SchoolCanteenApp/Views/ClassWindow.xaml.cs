using SchoolCanteenApp.ViewModels;
using System.Windows;

namespace SchoolCanteenApp.Views
{
    public partial class ClassView : Window
    {
        public ClassView(ClassViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}