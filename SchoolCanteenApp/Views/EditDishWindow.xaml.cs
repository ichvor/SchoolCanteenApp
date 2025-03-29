using SchoolCanteenApp.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchoolCanteenApp.Views
{
    public partial class EditDishWindow : Window
    {
        private readonly Dish _originalDish;
        private Dish _editableDish;
        private readonly SchoolCanteenEntities _context;

        public EditDishWindow(Dish selectedDish)
        {
            InitializeComponent();

            _context = new SchoolCanteenEntities();

           
            _originalDish = _context.Dish
                .Include(d => d.Ingredient)
                .FirstOrDefault(d => d.IdDish == selectedDish.IdDish);

            if (_originalDish == null)
            {
                MessageBox.Show("Блюдо не найдено");
                Close();
                return;
            }

          
            _editableDish = new Dish
            {
                IdDish = _originalDish.IdDish,
                DishName = _originalDish.DishName,
                Price = _originalDish.Price,
                Ingredient = new ObservableCollection<Ingredient>(_originalDish.Ingredient)
            };

           
            var allIngredients = _context.Ingredient.ToList();
            IngredientsList.ItemsSource = allIngredients;

          
            foreach (var ingredient in _editableDish.Ingredient)
            {
                var item = allIngredients.FirstOrDefault(i => i.IdIngredient == ingredient.IdIngredient);
                if (item != null) IngredientsList.SelectedItems.Add(item);
            }

            DataContext = _editableDish;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
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

            var result = MessageBox.Show("Сохранить изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                  
                    _originalDish.DishName = _editableDish.DishName;
                    _originalDish.Price = _editableDish.Price;

                 
                    _originalDish.Ingredient.Clear();
                    foreach (Ingredient ingredient in IngredientsList.SelectedItems)
                    {
                        var dbIngredient = _context.Ingredient.Find(ingredient.IdIngredient);
                        _originalDish.Ingredient.Add(dbIngredient);
                    }

                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (System.Exception ex)
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