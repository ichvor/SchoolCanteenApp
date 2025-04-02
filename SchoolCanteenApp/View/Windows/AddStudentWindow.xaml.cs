using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class AddStudentWindow : Window
    {
        private readonly Student _newStudent = new Student();
        public event EventHandler SaveClicked;
        public AddStudentWindow()
        {
            InitializeComponent();
            DataContext = _newStudent;
            LoadClassesAsync();
        }

        private async void LoadClassesAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var classes = await context.Class.ToListAsync();
                    ClassComboBox.ItemsSource = classes;
                    ClassComboBox.DisplayMemberPath = "Class1";
                    ClassComboBox.SelectedValuePath = "IdClass";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки классов: {ex.Message}");
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
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

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    context.Student.Add(_newStudent);
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