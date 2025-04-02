using SchoolCanteenApp.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class EditDishWindow : Window
    {
        private Dish _originalDish; 
        private Dish _editableDish;
        private readonly SchoolCanteenEntities _context;
        public event EventHandler SaveClicked;
        public EditDishWindow(Dish selectedDish)
        {
            InitializeComponent();
            _context = new SchoolCanteenEntities();
            InitializeDataAsync(selectedDish);
        }

        private async void InitializeDataAsync(Dish selectedDish)
        {
            try
            {
                var dish = await _context.Dish
                    .Include(d => d.Ingredient)
                    .FirstOrDefaultAsync(d => d.IdDish == selectedDish.IdDish);

                if (dish == null)
                {
                    MessageBox.Show("Блюдо не найдено");
                    Close();
                    return;
                }

                _originalDish = dish;
                _editableDish = new Dish
                {
                    IdDish = _originalDish.IdDish,
                    DishName = _originalDish.DishName,
                    Price = _originalDish.Price,
                    Ingredient = new ObservableCollection<Ingredient>(_originalDish.Ingredient)
                };

                var allIngredients = await _context.Ingredient.ToListAsync();
                IngredientsList.ItemsSource = allIngredients;

                foreach (var ingredient in _editableDish.Ingredient)
                {
                    var item = allIngredients.FirstOrDefault(i => i.IdIngredient == ingredient.IdIngredient);
                    if (item != null) IngredientsList.SelectedItems.Add(item);
                }

                DataContext = _editableDish;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                Close();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (_originalDish == null) return;
            if (string.IsNullOrWhiteSpace(_editableDish.DishName))
            {
                MessageBox.Show("Название блюда не может быть пустым!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                DishNameTextBox.Focus();
                return;
            }

            if (_editableDish.Price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                PriceTextBox.Focus();
                return;
            }

            if (IngredientsList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Необходимо выбрать хотя бы один ингредиент!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }         
            try
            {
                _originalDish.DishName = _editableDish.DishName;
                _originalDish.Price = _editableDish.Price;

                _originalDish.Ingredient.Clear();
                foreach (Ingredient ingredient in IngredientsList.SelectedItems)
                {
                    var dbIngredient = await _context.Ingredient.FindAsync(ingredient.IdIngredient);
                    _originalDish.Ingredient.Add(dbIngredient);
                }

                await _context.SaveChangesAsync();
                SaveClicked?.Invoke(this, EventArgs.Empty);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
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