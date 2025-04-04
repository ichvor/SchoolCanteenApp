using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class EditTeacherWindow : Window
    {
        private Teacher _originalTeacher;
        private Teacher _editableTeacher;
        private readonly SchoolCanteenEntities _context;
        public event EventHandler SaveClicked;

        public EditTeacherWindow(Teacher selectedTeacher)
        {
            InitializeComponent();
            _context = new SchoolCanteenEntities();
            InitializeDataAsync(selectedTeacher);
        }

        private async void InitializeDataAsync(Teacher selectedTeacher)
        {
            try
            {
                var teacher = await _context.Teacher.FindAsync(selectedTeacher.IdTeacher);

                if (teacher == null)
                {
                    MessageBox.Show("Учитель не найден");
                    Close();
                    return;
                }

                _originalTeacher = teacher;
                _editableTeacher = new Teacher
                {
                    IdTeacher = _originalTeacher.IdTeacher,
                    FirstName = _originalTeacher.FirstName,
                    LastName = _originalTeacher.LastName
                };

                DataContext = _editableTeacher;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                Close();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_editableTeacher.FirstName))
            {
                MessageBox.Show("Имя учителя не может быть пустым!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                FirstNameTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_editableTeacher.LastName))
            {
                MessageBox.Show("Фамилия учителя не может быть пустой!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                LastNameTextBox.Focus();
                return;
            }

            try
            {
                _originalTeacher.FirstName = _editableTeacher.FirstName;
                _originalTeacher.LastName = _editableTeacher.LastName;

                await _context.SaveChangesAsync();
                SaveClicked?.Invoke(this, EventArgs.Empty);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
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