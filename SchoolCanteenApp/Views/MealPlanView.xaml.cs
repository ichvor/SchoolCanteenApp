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

namespace SchoolCanteenApp.Views
{
    /// <summary>
    /// Логика взаимодействия для MealPlanView.xaml
    /// </summary>
    public partial class MealPlanView : Window
    {
        public MealPlanView(ViewModel.MealPlanViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
