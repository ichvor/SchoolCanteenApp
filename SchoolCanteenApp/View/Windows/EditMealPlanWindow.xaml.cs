using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class EditMealPlanWindow : Window
    {
        private MealPlan _originalMealPlan; 
        private MealPlan _editableMealPlan;
        private readonly SchoolCanteenEntities _context;
        public event EventHandler SaveClicked;
        public EditMealPlanWindow(MealPlan selectedMealPlan)
        {
            InitializeComponent();
            _context = new SchoolCanteenEntities();
            InitializeDataAsync(selectedMealPlan);
        }

        private async void InitializeDataAsync(MealPlan selectedMealPlan)
        {
            try
            {
                var plan = await _context.MealPlan
                    .Include(mp => mp.Student)
                    .Include(mp => mp.Meal)
                    .Include(mp => mp.Day)
                    .Include(mp => mp.Paid)
                    .FirstOrDefaultAsync(mp => mp.IdMealPlan == selectedMealPlan.IdMealPlan);

                if (plan == null)
                {
                    MessageBox.Show("Запись не найдена");
                    Close();
                    return;
                }

                _originalMealPlan = plan;
                _editableMealPlan = new MealPlan
                {
                    IdMealPlan = _originalMealPlan.IdMealPlan,
                    IdStudent = _originalMealPlan.IdStudent,
                    IdMeal = _originalMealPlan.IdMeal,
                    IdDay = _originalMealPlan.IdDay,
                    IdPaid = _originalMealPlan.IdPaid
                };

                await LoadComboBoxDataAsync();
                DataContext = _editableMealPlan;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                Close();
            }
        }

        private async Task LoadComboBoxDataAsync()
        {
            StudentsCombo.ItemsSource = await _context.Student.ToListAsync();
            MealsCombo.ItemsSource = await _context.Meal.ToListAsync();
            DaysCombo.ItemsSource = await _context.Day.ToListAsync();
            PaidStatusCombo.ItemsSource = await _context.Paid.ToListAsync();

            StudentsCombo.SelectedValue = _editableMealPlan.IdStudent;
            MealsCombo.SelectedValue = _editableMealPlan.IdMeal;
            DaysCombo.SelectedValue = _editableMealPlan.IdDay;
            PaidStatusCombo.SelectedValue = _editableMealPlan.IdPaid;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            var errorMessage = new StringBuilder();
            if (StudentsCombo.SelectedValue == null)
                errorMessage.AppendLine("Не выбран ученик");
            if (MealsCombo.SelectedValue == null)
                errorMessage.AppendLine("Не выбран прием пищи");
            if (DaysCombo.SelectedValue == null)
                errorMessage.AppendLine("Не выбран день недели");
            if (PaidStatusCombo.SelectedValue == null)
                errorMessage.AppendLine("Не выбран статус оплаты");
            try
            {
                _originalMealPlan.IdStudent = _editableMealPlan.IdStudent;
                _originalMealPlan.IdMeal = _editableMealPlan.IdMeal;
                _originalMealPlan.IdDay = _editableMealPlan.IdDay;
                _originalMealPlan.IdPaid = _editableMealPlan.IdPaid;

                await _context.SaveChangesAsync();
                SaveClicked?.Invoke(this, EventArgs.Empty);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                DialogResult = false;
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