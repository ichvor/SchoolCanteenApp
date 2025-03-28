using SchoolCanteenApp.Model;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace SchoolCanteenApp.Views
{
    public partial class EditDishWindow : Window
    {
        private readonly Dish _originalDish;
        private readonly Dish _editableDish;

        public EditDishWindow(Dish selectedDish)
        {
            InitializeComponent();

            // Создаем копию объекта для редактирования
            _originalDish = selectedDish;
            _editableDish = new Dish
            {
                IdDish = _originalDish.IdDish,
                DishName = _originalDish.DishName,
                Price = _originalDish.Price,
                Ingredient = new ObservableCollection<Ingredient>(_originalDish.Ingredient)
            };

            using (var context = new SchoolCanteenEntities())
            {
                context.Entry(_editableDish).State = EntityState.Unchanged;
                IngredientsList.ItemsSource = context.Ingredient.ToList();

                foreach (var ingredient in _editableDish.Ingredient)
                {
                    IngredientsList.SelectedItems.Add(ingredient);
                }
            }

            DataContext = _editableDish;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var dishToUpdate = context.Dish
                        .Include(d => d.Ingredient)
                        .FirstOrDefault(d => d.IdDish == _originalDish.IdDish);

                    if (dishToUpdate != null)
                    {
                        // Обновляем свойства
                        dishToUpdate.DishName = _editableDish.DishName;
                        dishToUpdate.Price = _editableDish.Price;

                        // Обновляем ингредиенты
                        dishToUpdate.Ingredient.Clear();
                        foreach (Ingredient ingredient in IngredientsList.SelectedItems)
                        {
                            var dbIngredient = context.Ingredient.Find(ingredient.IdIngredient);
                            dishToUpdate.Ingredient.Add(dbIngredient);
                        }

                        context.SaveChanges();
                    }
                }
                DialogResult = true;
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
    }
}