using SchoolCanteenApp.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.View.Windows
{
    public partial class AddDishWindow : Window
    {
        private readonly Dish _newDish = new Dish();
        public event EventHandler SaveClicked;
        public AddDishWindow()
        {
            InitializeComponent();
            DataContext = _newDish;
            LoadIngredientsAsync();
        }

        private async void LoadIngredientsAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var ingredients = await context.Ingredient.ToListAsync();
                    IngredientsList.ItemsSource = ingredients;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ингредиентов: {ex.Message}");
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_newDish.DishName))
            {
                MessageBox.Show("Введите название блюда!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_newDish.Price <= 0)
            {
                MessageBox.Show("Цена должна быть больше нуля!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IngredientsList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один ингредиент!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    _newDish.Ingredient = IngredientsList.SelectedItems
                        .Cast<Ingredient>()
                        .ToList();

                    context.Dish.Add(_newDish);
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