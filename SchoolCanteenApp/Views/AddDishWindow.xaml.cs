using SchoolCanteenApp.Model;
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
            using (var context = new SchoolCanteenEntities())
            {
                _newDish.Ingredient = IngredientsList.SelectedItems.Cast<Ingredient>().ToList();
                context.Dish.Add(_newDish);
                context.SaveChanges();
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}