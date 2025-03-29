using SchoolCanteenApp.Model;
using System;
using System.Linq;
using System.Windows;

namespace SchoolCanteenApp.Views
{
    public partial class AddClassWindow : Window
    {
        private readonly Class _newClass = new Class();

        public AddClassWindow()
        {
            InitializeComponent();
            DataContext = _newClass;

            using (var context = new SchoolCanteenEntities())
            {
                TeacherComboBox.ItemsSource = context.Teacher.ToList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверки
            if (string.IsNullOrWhiteSpace(_newClass.Class1))
            {
                MessageBox.Show("Введите название класса!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TeacherComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите классного руководителя!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Сохранение
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    context.Class.Add(_newClass);
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