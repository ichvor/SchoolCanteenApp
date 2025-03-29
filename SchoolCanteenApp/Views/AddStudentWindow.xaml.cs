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
            // Проверки
            if (string.IsNullOrWhiteSpace(_newStudent.FirstName))
            {
                MessageBox.Show("Введите имя ученика!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(_newStudent.LastName))
            {
                MessageBox.Show("Введите фамилию ученика!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ClassComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите класс!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Сохранение
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    context.Student.Add(_newStudent);
                    context.SaveChanges();
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
