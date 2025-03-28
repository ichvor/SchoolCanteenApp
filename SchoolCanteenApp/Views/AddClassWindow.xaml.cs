using SchoolCanteenApp.Model;
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
            using (var context = new SchoolCanteenEntities())
            {
                context.Class.Add(_newClass);
                context.SaveChanges();
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}