using SchoolCanteenApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolCanteenApp.ViewModel
{
    public class MealPlanViewModel
    {
        public MealPlan CurrentMealPlan { get; set; }
        public List<Student> Students { get; set; }
        public List<Meal> Meals { get; set; }
        public List<Paid> Paids { get; set; }
        public List<Day> Days { get; set; }

        public MealPlanViewModel(
            List<Student> students,
            List<Meal> meals,
            List<Paid> paids,
            List<Day> days)
        {
            CurrentMealPlan = new MealPlan();
            Students = students;
            Meals = meals;
            Paids = paids;
            Days = days;
        }

        public MealPlanViewModel(
            MealPlan mealPlan,
            List<Student> students,
            List<Meal> meals,
            List<Paid> paids,
            List<Day> days) : this(students, meals, paids, days)
        {
            CurrentMealPlan = mealPlan;
        }
    }
}
