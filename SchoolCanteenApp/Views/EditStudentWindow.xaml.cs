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
using System.Data.Entity;
using System.Windows.Shapes;

namespace SchoolCanteenApp.Views
{
    /// <summary>
    /// Логика взаимодействия для EditStudentWindow.xaml
    /// </summary>
    public partial class EditStudentWindow : Window
    {
        private readonly Student _originalStudent;
        private Student _editableStudent;
        private readonly SchoolCanteenEntities _context;

        public EditStudentWindow(Student student)
        {
            InitializeComponent();

            _context = new SchoolCanteenEntities();
            _originalStudent = _context.Student
                .Include(s => s.Class) 
                .FirstOrDefault(s => s.IdStudent == student.IdStudent);

            if (_originalStudent == null)
            {
                MessageBox.Show("Ученик не найден");
                Close();
                return;
            }

            _editableStudent = new Student
            {
                IdStudent = _originalStudent.IdStudent,
                FirstName = _originalStudent.FirstName,
                LastName = _originalStudent.LastName,
                IdClass = _originalStudent.IdClass
            };

            ClassComboBox.ItemsSource = _context.Class.ToList();
            ClassComboBox.SelectedValue = _editableStudent.IdClass;

            DataContext = _editableStudent;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_editableStudent.FirstName))
            {
                MessageBox.Show("Имя ученика не может быть пустым!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                FirstNameTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_editableStudent.LastName))
            {
                MessageBox.Show("Фамилия ученика не может быть пустой!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                LastNameTextBox.Focus();
                return;
            }

            if (ClassComboBox.SelectedValue == null)
            {
                MessageBox.Show("Необходимо выбрать класс!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ClassComboBox.Focus();
                return;
            }

            var result = MessageBox.Show("Сохранить изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {                   
                    _originalStudent.FirstName = _editableStudent.FirstName;
                    _originalStudent.LastName = _editableStudent.LastName;
                    _originalStudent.IdClass = _editableStudent.IdClass;

                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                    DialogResult = false;
                }
            }
            else
            {
                DialogResult = false;
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _context.Dispose();
        }
    }
}
