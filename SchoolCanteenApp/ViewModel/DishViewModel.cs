using SchoolCanteenApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolCanteenApp.ViewModel
{
    public class DishViewModel
    {
        public Dish CurrentDish { get; set; }
        public List<Ingredient> AllIngredients { get; set; }
        public List<int> SelectedIngredients { get; set; }

        public DishViewModel(List<Ingredient> ingredients)
        {
            CurrentDish = new Dish();
            AllIngredients = ingredients;
            SelectedIngredients = new List<int>();
        }

        public DishViewModel(Dish dish, List<Ingredient> ingredients) : this(ingredients)
        {
            CurrentDish = dish;
            SelectedIngredients = dish.Ingredients.Select(i => i.IdIngredient).ToList();
        }
    }
}
