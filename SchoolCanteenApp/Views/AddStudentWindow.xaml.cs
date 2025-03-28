using SchoolCanteenApp.Model;
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
    /// Логика взаимодействия для AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        private readonly Student _newStudent = new Student();

        public AddStudentWindow()
        {
            InitializeComponent();
            DataContext = _newStudent;

            using (var context = new SchoolCanteenEntities())
            {
                ClassComboBox.ItemsSource = context.Class.ToList();
                ClassComboBox.DisplayMemberPath = "Class1";
                ClassComboBox.SelectedValuePath = "IdClass";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new SchoolCanteenEntities())
            {
                context.Student.Add(_newStudent);
                context.SaveChanges();
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
