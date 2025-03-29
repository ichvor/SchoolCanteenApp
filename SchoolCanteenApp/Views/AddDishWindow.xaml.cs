using SchoolCanteenApp.Model;
using System;
using System.Linq;
using System.Windows;

namespace SchoolCanteenApp.Views
{
    public partial class AddDishWindow : Window
    {
        private readonly Dish _newDish = new Dish();

        public AddDishWindow()
        {
            InitializeComponent();
            DataContext = _newDish;

            using (var context = new SchoolCanteenEntities())
            {
                IngredientsList.ItemsSource = context.Ingredient.ToList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверки
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

            // Сохранение
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    _newDish.Ingredient = IngredientsList.SelectedItems
                        .Cast<Ingredient>()
                        .ToList();

                    context.Dish.Add(_newDish);
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