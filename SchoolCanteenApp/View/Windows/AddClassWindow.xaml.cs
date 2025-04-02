using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class AddClassWindow : Window
    {
        private readonly Class _newClass = new Class();
        public event EventHandler SaveClicked;
        public AddClassWindow()
        {
            InitializeComponent();
            DataContext = _newClass;
            LoadTeachersAsync();
        }

        private async void LoadTeachersAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var teachers = await context.Teacher.ToListAsync();
                    TeacherComboBox.ItemsSource = teachers;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки учителей: {ex.Message}");
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
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

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    context.Class.Add(_newClass);
                    await context.SaveChangesAsync();
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