using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class EditStudentWindow : Window
    {
        private Student _originalStudent; // Убрали readonly
        private Student _editableStudent;
        private readonly SchoolCanteenEntities _context;
        public event EventHandler SaveClicked;
        public EditStudentWindow(Student student)
        {
            InitializeComponent();
            _context = new SchoolCanteenEntities();
            InitializeDataAsync(student);
        }

        private async void InitializeDataAsync(Student student)
        {
            try
            {
                var dbStudent = await _context.Student
                    .Include(s => s.Class)
                    .FirstOrDefaultAsync(s => s.IdStudent == student.IdStudent);

                if (dbStudent == null)
                {
                    MessageBox.Show("Ученик не найден");
                    Close();
                    return;
                }

                _originalStudent = dbStudent;
                _editableStudent = new Student
                {
                    IdStudent = _originalStudent.IdStudent,
                    FirstName = _originalStudent.FirstName,
                    LastName = _originalStudent.LastName,
                    IdClass = _originalStudent.IdClass
                };

                var classes = await _context.Class.ToListAsync();
                ClassComboBox.ItemsSource = classes;
                ClassComboBox.SelectedValue = _editableStudent.IdClass;

                DataContext = _editableStudent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                Close();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
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
            try
            {
                _originalStudent.FirstName = _editableStudent.FirstName;
                _originalStudent.LastName = _editableStudent.LastName;
                _originalStudent.IdClass = _editableStudent.IdClass;

                await _context.SaveChangesAsync();
                SaveClicked?.Invoke(this, EventArgs.Empty);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                DialogResult = false;
            }
            finally
            {
                Close();
            }
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