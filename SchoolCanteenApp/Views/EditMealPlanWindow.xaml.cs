using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace SchoolCanteenApp.Views
{
    public partial class EditMealPlanWindow : Window
    {
        private readonly MealPlan _originalMealPlan;
        private MealPlan _editableMealPlan;
        private readonly SchoolCanteenEntities _context;

        public EditMealPlanWindow(MealPlan selectedMealPlan)
        {
            InitializeComponent();

            _context = new SchoolCanteenEntities();
            _originalMealPlan = _context.MealPlan
                .Include(mp => mp.Student)
                .Include(mp => mp.Meal)
                .Include(mp => mp.Day)
                .Include(mp => mp.Paid)
                .FirstOrDefault(mp => mp.IdMealPlan == selectedMealPlan.IdMealPlan);

            if (_originalMealPlan == null)
            {
                MessageBox.Show("Запись не найдена");
                Close();
                return;
            }

            _editableMealPlan = new MealPlan
            {
                IdMealPlan = _originalMealPlan.IdMealPlan,
                IdStudent = _originalMealPlan.IdStudent,
                IdMeal = _originalMealPlan.IdMeal,
                IdDay = _originalMealPlan.IdDay,
                IdPaid = _originalMealPlan.IdPaid
            };

            StudentsCombo.ItemsSource = _context.Student.ToList();
            MealsCombo.ItemsSource = _context.Meal.ToList();
            DaysCombo.ItemsSource = _context.Day.ToList();
            PaidStatusCombo.ItemsSource = _context.Paid.ToList();

            StudentsCombo.SelectedValue = _editableMealPlan.IdStudent;
            MealsCombo.SelectedValue = _editableMealPlan.IdMeal;
            DaysCombo.SelectedValue = _editableMealPlan.IdDay;
            PaidStatusCombo.SelectedValue = _editableMealPlan.IdPaid;

            DataContext = _editableMealPlan;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                
                    _originalMealPlan.IdStudent = _editableMealPlan.IdStudent;
                    _originalMealPlan.IdMeal = _editableMealPlan.IdMeal;
                    _originalMealPlan.IdDay = _editableMealPlan.IdDay;
                    _originalMealPlan.IdPaid = _editableMealPlan.IdPaid;

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