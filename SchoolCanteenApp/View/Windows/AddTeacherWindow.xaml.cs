using SchoolCanteenApp.Model;
using System;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class AddTeacherWindow : Window
    {
        private readonly Teacher _newTeacher = new Teacher();
        public event EventHandler SaveClicked;

        public AddTeacherWindow()
        {
            InitializeComponent();
            DataContext = _newTeacher;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_newTeacher.FirstName))
            {
                MessageBox.Show("Введите имя учителя!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(_newTeacher.LastName))
            {
                MessageBox.Show("Введите фамилию учителя!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    context.Teacher.Add(_newTeacher);
                    context.SaveChanges();
                    SaveClicked?.Invoke(this, EventArgs.Empty);
                    this.DialogResult = true;
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