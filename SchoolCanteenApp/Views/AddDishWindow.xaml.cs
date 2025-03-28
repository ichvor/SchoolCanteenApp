using SchoolCanteenApp.Model;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    // Получаем выбранные ингредиенты
                    _newDish.Ingredient = IngredientsList.SelectedItems.Cast<Ingredient>().ToList();

                    // Добавляем блюдо в контекст
                    context.Dish.Add(_newDish);

                    // Сохраняем изменения
                    context.SaveChanges();
                }

                DialogResult = true;
                Close();
            }
            catch (DbEntityValidationException ex)
            {
                // Собираем сообщения об ошибках
                var errorMessages = new List<string>();
                foreach (var entityError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityError.ValidationErrors)
                    {
                        errorMessages.Add($"Свойство: {validationError.PropertyName}, Ошибка: {validationError.ErrorMessage}");
                    }
                }
                MessageBox.Show(string.Join("\n", errorMessages), "Ошибка валидации");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}