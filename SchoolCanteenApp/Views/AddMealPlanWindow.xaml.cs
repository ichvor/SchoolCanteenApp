using SchoolCanteenApp.Model;
using System.Linq;
using System.Windows;

namespace SchoolCanteenApp.Views
{
    public partial class AddMealPlanWindow : Window
    {
        private readonly MealPlan _newMealPlan = new MealPlan();

        public AddMealPlanWindow()
        {
            InitializeComponent();
            DataContext = _newMealPlan;

            using (var context = new SchoolCanteenEntities())
            {
                StudentsCombo.ItemsSource = context.Student.ToList();
                MealsCombo.ItemsSource = context.Meal.ToList();
                DaysCombo.ItemsSource = context.Day.ToList();
                PaidStatusCombo.ItemsSource = context.Paid.ToList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new SchoolCanteenEntities())
            {
                context.MealPlan.Add(_newMealPlan);
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