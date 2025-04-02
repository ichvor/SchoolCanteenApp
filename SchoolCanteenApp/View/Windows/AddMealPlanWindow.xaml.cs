using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class AddMealPlanWindow : Window
    {
        private readonly MealPlan _newMealPlan = new MealPlan();
        public event EventHandler SaveClicked;
        public AddMealPlanWindow()
        {
            InitializeComponent();
            DataContext = _newMealPlan;
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var students = await context.Student.ToListAsync();
                    var meals = await context.Meal.ToListAsync();
                    var days = await context.Day.ToListAsync();
                    var statuses = await context.Paid.ToListAsync();

                    StudentsCombo.ItemsSource = students;
                    MealsCombo.ItemsSource = meals;
                    DaysCombo.ItemsSource = days;
                    PaidStatusCombo.ItemsSource = statuses;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
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