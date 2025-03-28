using SchoolCanteenApp.Model;
using System;
using System.Linq;
using System.Windows;
using System.Data.Entity;

namespace SchoolCanteenApp.Views
{
    /// <summary>
    /// Логика взаимодействия для EditClassWindow.xaml
    /// </summary>
    public partial class EditClassWindow : Window
    {
        private readonly Class _originalClass;
        private Class _editableClass;
        private readonly SchoolCanteenEntities _context;

        public EditClassWindow(Class selectedClass)
        {
            InitializeComponent();

            _context = new SchoolCanteenEntities();
            _originalClass = _context.Class
                .Include(c => c.Teacher)
                .FirstOrDefault(c => c.IdClass == selectedClass.IdClass);

            if (_originalClass == null)
            {
                MessageBox.Show("Класс не найден");
                Close();
                return;
            }

           
            _editableClass = new Class
            {
                IdClass = _originalClass.IdClass,
                Class1 = _originalClass.Class1,
                IdTeacher = _originalClass.IdTeacher
            };

            
            TeacherComboBox.ItemsSource = _context.Teacher.ToList();
            TeacherComboBox.SelectedValue = _editableClass.IdTeacher;

            DataContext = _editableClass;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    
                    _originalClass.Class1 = _editableClass.Class1;
                    _originalClass.IdTeacher = _editableClass.IdTeacher;

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
