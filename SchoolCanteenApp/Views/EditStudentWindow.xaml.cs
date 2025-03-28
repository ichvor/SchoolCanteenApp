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
    /// Логика взаимодействия для EditStudentWindow.xaml
    /// </summary>
    public partial class EditStudentWindow : Window
    {
        private readonly Student _student;

        public EditStudentWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            DataContext = _student;

            // Загрузка классов для ComboBox
            using (var context = new SchoolCanteenEntities())
            {
                ClassComboBox.ItemsSource = context.Class.ToList();
                ClassComboBox.DisplayMemberPath = "Class1";
                ClassComboBox.SelectedValuePath = "IdClass";
                ClassComboBox.SelectedValue = _student.IdClass;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new SchoolCanteenEntities())
            {
                var studentToUpdate = context.Student.Find(_student.IdStudent);
                if (studentToUpdate != null)
                {
                    studentToUpdate.FirstName = _student.FirstName;
                    studentToUpdate.LastName = _student.LastName;
                    studentToUpdate.IdClass = _student.IdClass;
                    context.SaveChanges();
                }
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
