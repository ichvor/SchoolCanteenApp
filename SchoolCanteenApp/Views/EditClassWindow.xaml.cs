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
    /// Логика взаимодействия для EditClassWindow.xaml
    /// </summary>
    public partial class EditClassWindow : Window
    {
        private readonly Class _class;

        public EditClassWindow(Class selectedClass)
        {
            InitializeComponent();
            _class = selectedClass;
            DataContext = _class;

            using (var context = new SchoolCanteenEntities())
            {
                TeacherComboBox.ItemsSource = context.Teacher.ToList();
                TeacherComboBox.SelectedValue = _class.IdTeacher;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new SchoolCanteenEntities())
            {
                var classToUpdate = context.Class.Find(_class.IdClass);
                if (classToUpdate != null)
                {
                    classToUpdate.Class1 = _class.Class1;
                    classToUpdate.IdTeacher = _class.IdTeacher;
                    context.SaveChanges();
                }
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
