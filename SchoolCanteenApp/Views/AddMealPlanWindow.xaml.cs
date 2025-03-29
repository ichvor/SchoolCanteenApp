using SchoolCanteenApp.Model;
using System;
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
            if (StudentsCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите ученика!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MealsCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите прием пищи!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DaysCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите день!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PaidStatusCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус оплаты!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    context.MealPlan.Add(_newMealPlan);
                    context.SaveChanges();
                    DialogResult = true;
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