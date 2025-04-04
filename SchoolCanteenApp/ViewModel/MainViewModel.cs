using SchoolCanteenApp.Data;
using SchoolCanteenApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SchoolCanteenApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly SchoolCanteenContext _context = new SchoolCanteenContext();

        private ObservableCollection<Class> _classes;
        private ObservableCollection<Student> _students;
        private ObservableCollection<Dish> _dishes;
        private ObservableCollection<MealPlan> _mealPlans;

        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set => SetProperty(ref _classes, value);
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }

        public ObservableCollection<MealPlan> MealPlans
        {
            get => _mealPlans;
            set => SetProperty(ref _mealPlans, value);
        }


        // Команды для Class
        public ICommand AddClassCommand { get; }
        public ICommand EditClassCommand { get; }
        public ICommand DeleteClassCommand { get; }

        // Команды для Student
        public ICommand AddStudentCommand { get; }
        public ICommand EditStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }

        // Команды для Dish
        public ICommand AddDishCommand { get; }
        public ICommand EditDishCommand { get; }
        public ICommand DeleteDishCommand { get; }

        // Команды для MealPlan
        public ICommand AddMealPlanCommand { get; }
        public ICommand EditMealPlanCommand { get; }
        public ICommand DeleteMealPlanCommand { get; }

        public MainViewModel()
        {
            LoadData();
           
        }

        private void LoadData()
        {
            try
            {
                // Загрузка данных с включением связанных сущностей
                _context.Classes
                    .Include(c => c.Teacher)
                    .Include(c => c.Students)
                    .Load();
                Classes = _context.Classes.Local;

                _context.Students
                    .Include(s => s.Class)
                    .Include(s => s.MealPlans)
                    .Load();
                Students = _context.Students.Local;

                _context.Dishes
                    .Include(d => d.Ingredients)
                    .Load();
                Dishes = _context.Dishes.Local;

                _context.MealPlans
                    .Include(mp => mp.Student)
                    .Include(mp => mp.Meal)
                    .Include(mp => mp.Paid)
                    .Include(mp => mp.Day)
                    .Load();
                MealPlans = _context.MealPlans.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
            }
            // Методы для Class
            private void AddClass(object parameter)
        {
            var newClass = new Class { Class1 = "Новый класс" };
            _context.Classes.Add(newClass);
            _context.SaveChanges();
            Classes.Add(newClass);
        }

        private void EditClass(object parameter)
        {
            if (parameter is Class selectedClass)
            {
                // Логика редактирования
            }
        }

        private void DeleteClass(object parameter)
        {
            if (parameter is Class selectedClass)
            {
                _context.Classes.Remove(selectedClass);
                _context.SaveChanges();
                Classes.Remove(selectedClass);
            }
        }

        private void AddStudent(object parameter)
        {
            var newStudent = new Student
            {
                FirstName = "Новый",
                LastName = "Студент",
                IdClass = 1 // Пример класса по умолчанию
            };

            _context.Students.Add(newStudent);
            _context.SaveChanges();
            Students.Add(newStudent);
        }

        private void EditStudent(object parameter)
        {
            if (parameter is Student selectedStudent)
            {
                // Пример редактирования
                selectedStudent.FirstName = "Измененное имя";
                _context.Entry(selectedStudent).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        private void DeleteStudent(object parameter)
        {
            if (parameter is Student selectedStudent)
            {
                _context.Students.Remove(selectedStudent);
                _context.SaveChanges();
                Students.Remove(selectedStudent);
            }
        }
        private void AddDish(object parameter)
        {
            var newDish = new Dish
            {
                DishName = "Новое блюдо",
                NutritionalValue = "Калории: 300",
                Price = 150.00m
            };

            _context.Dishes.Add(newDish);
            _context.SaveChanges();
            Dishes.Add(newDish);
        }

        private void EditDish(object parameter)
        {
            if (parameter is Dish selectedDish)
            {
                // Пример редактирования
                selectedDish.Price = 200.00m;
                _context.Entry(selectedDish).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        private void DeleteDish(object parameter)
        {
            if (parameter is Dish selectedDish)
            {
                _context.Dishes.Remove(selectedDish);
                _context.SaveChanges();
                Dishes.Remove(selectedDish);
            }
        }
        private void AddMealPlan(object parameter)
        {
            var newMealPlan = new MealPlan
            {
                IdStudent = 1,    // Пример студента
                IdMeal = 1,       // Пример приема пищи
                IdPaid = 1,       // Пример статуса оплаты
                IdDay = 1         // Пример дня
            };

            _context.MealPlans.Add(newMealPlan);
            _context.SaveChanges();
            MealPlans.Add(newMealPlan);
        }

        private void EditMealPlan(object parameter)
        {
            if (parameter is MealPlan selectedMealPlan)
            {
                // Пример редактирования
                selectedMealPlan.IdDay = 2;
                _context.Entry(selectedMealPlan).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        private void DeleteMealPlan(object parameter)
        {
            if (parameter is MealPlan selectedMealPlan)
            {
                _context.MealPlans.Remove(selectedMealPlan);
                _context.SaveChanges();
                MealPlans.Remove(selectedMealPlan);
            }
        }


    }
}