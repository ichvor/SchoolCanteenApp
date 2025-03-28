using SchoolCanteenApp.ViewWindow;
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
            AddStudentWindow addStudentWindow = new AddStudentWindow();
            addStudentWindow.Owner = this;
            addStudentWindow.ShowDialog();
        }

        private void Button_Click_LoadStudent(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).LoadStudent();
        }

        private void Button_Click_DeleteStudent(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).DeleteStudent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
