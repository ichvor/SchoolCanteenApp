using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class EditClassWindow : Window
    {
        private Class _originalClass; 
        private Class _editableClass;
        private readonly SchoolCanteenEntities _context;
        public event EventHandler SaveClicked;
        public EditClassWindow(Class selectedClass)
        {
            InitializeComponent();
            _context = new SchoolCanteenEntities();
            InitializeDataAsync(selectedClass);
        }

        private async void InitializeDataAsync(Class selectedClass)
        {
            try
            {
                var original = await _context.Class
                    .Include(c => c.Teacher)
                    .FirstOrDefaultAsync(c => c.IdClass == selectedClass.IdClass);

                if (original == null)
                {
                    MessageBox.Show("Класс не найден");
                    Close();
                    return;
                }

                _originalClass = original; 
                _editableClass = new Class
                {
                    IdClass = _originalClass.IdClass,
                    Class1 = _originalClass.Class1,
                    IdTeacher = _originalClass.IdTeacher
                };

                var teachers = await _context.Teacher.ToListAsync();
                TeacherComboBox.ItemsSource = teachers;
                TeacherComboBox.SelectedValue = _editableClass.IdTeacher;

                DataContext = _editableClass;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                Close();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(_editableClass.Class1))
            {
                MessageBox.Show("Название класса не может быть пустым!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ClassTextBox.Focus();
                return;
            }

            if (TeacherComboBox.SelectedValue == null)
            {
                MessageBox.Show("Необходимо выбрать классного руководителя!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                TeacherComboBox.Focus();
                return;
            }
            if (_originalClass == null) 
            {
                MessageBox.Show("Ошибка: данные класса не загружены");
                return;
            }

            if (string.IsNullOrWhiteSpace(_editableClass.Class1))
            {
                MessageBox.Show("Введите название класса!", "Ошибка");
                return;
            }

            try
            {
               
                _originalClass.Class1 = _editableClass.Class1;
                _originalClass.IdTeacher = _editableClass.IdTeacher;

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