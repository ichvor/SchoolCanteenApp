using SchoolCanteenApp.ViewModel;
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

namespace SchoolCanteenApp.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

        }


        private void Button_Click_AddStudent(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_EditStudent(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_DeleteStudent(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_AddClass(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click_EditClass(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_DeleteClass(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_AddDish(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_EditDish(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_DeleteDish(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_AddMeal(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_EditMeal(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_DeleteMeal(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
