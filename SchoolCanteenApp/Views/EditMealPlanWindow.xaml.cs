using SchoolCanteenApp.Model;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace SchoolCanteenApp.Views
{
    public partial class EditMealPlanWindow : Window
    {
        private readonly MealPlan _mealPlan;

        public EditMealPlanWindow(MealPlan selectedMealPlan)
        {
            InitializeComponent();

            using (var context = new SchoolCanteenEntities())
            {
                // Перезагружаем объект в текущем контексте
                _mealPlan = context.MealPlan
                    .Include(mp => mp.Student)
                    .Include(mp => mp.Meal)
                    .Include(mp => mp.Day)
                    .Include(mp => mp.Paid)
                    .FirstOrDefault(mp => mp.IdMealPlan == selectedMealPlan.IdMealPlan);

                if (_mealPlan == null)
                {
                    MessageBox.Show("Запись не найдена");
                    Close();
                    return;
                }

                DataContext = _mealPlan;

                // Загрузка данных для комбобоксов
                StudentsCombo.ItemsSource = context.Student.ToList();
                MealsCombo.ItemsSource = context.Meal.ToList();
                DaysCombo.ItemsSource = context.Day.ToList();
                PaidStatusCombo.ItemsSource = context.Paid.ToList();

                // Установка выбранных значений
                StudentsCombo.SelectedValue = _mealPlan.IdStudent;
                MealsCombo.SelectedValue = _mealPlan.IdMeal;
                DaysCombo.SelectedValue = _mealPlan.IdDay;
                PaidStatusCombo.SelectedValue = _mealPlan.IdPaid;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new SchoolCanteenEntities())
            {
                var mealPlanToUpdate = context.MealPlan.Find(_mealPlan.IdMealPlan);
                if (mealPlanToUpdate != null)
                {
                    mealPlanToUpdate.IdStudent = _mealPlan.IdStudent;
                    mealPlanToUpdate.IdMeal = _mealPlan.IdMeal;
                    mealPlanToUpdate.IdDay = _mealPlan.IdDay;
                    mealPlanToUpdate.IdPaid = _mealPlan.IdPaid;
                    context.SaveChanges();
                }
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