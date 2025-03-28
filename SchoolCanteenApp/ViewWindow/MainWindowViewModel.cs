using System;
using SchoolCanteenApp.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Data.Entity;
using System.Windows.Input;
using SchoolCanteenApp.Views;

namespace SchoolCanteenApp.ViewWindow
{
    public class MainWindowViewModel : BaseViewModel
    {
        // Поля и свойства
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;
        private ObservableCollection<Class> _classes;
        private ObservableCollection<Dish> _dishes;
        private ObservableCollection<MealPlan> _mealPlans;
        private Class _selectedClass;
        private Dish _selectedDish;
        private MealPlan _selectedMealPlan;
      

        // Команды
        public ICommand AddStudentCommand { get; }
        public ICommand EditStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }
        public ICommand AddClassCommand { get; }
        public ICommand EditClassCommand { get; }
        public ICommand DeleteClassCommand { get; }
        public ICommand AddDishCommand { get; }
        public ICommand EditDishCommand { get; }
        public ICommand DeleteDishCommand { get; }
        public ICommand AddMealPlanCommand { get; }
        public ICommand EditMealPlanCommand { get; }
        public ICommand DeleteMealPlanCommand { get; }

        // Коллекции
        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetPropertyChanged(ref _students, value, nameof(Students));
        }

        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set => SetPropertyChanged(ref _classes, value, nameof(Classes));
        }

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetPropertyChanged(ref _dishes, value, nameof(Dishes));
        }

        public ObservableCollection<MealPlan> MealPlans
        {
            get => _mealPlans;
            set => SetPropertyChanged(ref _mealPlans, value, nameof(MealPlans));
        }

        // Выбранные элементы
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set => SetPropertyChanged(ref _selectedStudent, value);
        }

        public Class SelectedClass
        {
            get => _selectedClass;
            set => SetPropertyChanged(ref _selectedClass, value);
        }

        public Dish SelectedDish
        {
            get => _selectedDish;
            set => SetPropertyChanged(ref _selectedDish, value);
        }

        public MealPlan SelectedMealPlan
        {
            get => _selectedMealPlan;
            set => SetPropertyChanged(ref _selectedMealPlan, value);
        }

        public MainWindowViewModel()
        {
            // Инициализация коллекций
            Students = new ObservableCollection<Student>();
            Classes = new ObservableCollection<Class>();
            Dishes = new ObservableCollection<Dish>();
            MealPlans = new ObservableCollection<MealPlan>();

            // Инициализация команд

            AddStudentCommand = new RelayCommand(OpenAddStudentWindow);
            EditStudentCommand = new RelayCommand(OpenEditStudentWindow, CanExecuteStudentCommand);
            DeleteStudentCommand = new RelayCommand(DeleteStudent, CanExecuteStudentCommand);

            AddClassCommand = new RelayCommand(OpenAddClassWindow);
            EditClassCommand = new RelayCommand(OpenEditClassWindow, CanExecuteClassCommand);
            DeleteClassCommand = new RelayCommand(DeleteClass, CanExecuteClassCommand);

            AddDishCommand = new RelayCommand(OpenAddDishWindow);
            EditDishCommand = new RelayCommand(OpenEditDishWindow, CanExecuteDishCommand); // Добавлено
            DeleteDishCommand = new RelayCommand(DeleteDish, CanExecuteDishCommand);

            AddMealPlanCommand = new RelayCommand(OpenAddMealPlanWindow);
            EditMealPlanCommand = new RelayCommand(OpenEditMealPlanWindow, CanExecuteMealPlanCommand);
            DeleteMealPlanCommand = new RelayCommand(DeleteMealPlan, CanExecuteMealPlanCommand);

            LoadAllData();
        }

        #region Общие методы
        private void LoadAllData()
        {
            LoadStudents();
            LoadClasses();
            LoadDishes();
            LoadMealPlans();
        }
        #endregion

        #region Студенты
        private bool CanExecuteStudentCommand(object obj) => SelectedStudent != null;

        private void OpenAddStudentWindow(object obj)
        {
            var window = new AddStudentWindow();
            if (window.ShowDialog() == true) LoadStudents();
        }

        private void OpenEditStudentWindow(object obj)
        {
            if (SelectedStudent == null) return;
            var window = new EditStudentWindow(SelectedStudent);
            if (window.ShowDialog() == true) LoadStudents();
        }

        private void DeleteStudent(object obj)
        {
            if (SelectedStudent == null) return;

            var result = MessageBox.Show("Удалить этого ученика?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var student = context.Student.Find(SelectedStudent.IdStudent);
                    if (student != null)
                    {
                        context.Student.Remove(student);
                        context.SaveChanges();
                        LoadStudents();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }
        #endregion

        #region Классы
        private bool CanExecuteClassCommand(object obj) => SelectedClass != null;

        private void OpenAddClassWindow(object obj)
        {
            var window = new AddClassWindow();
            if (window.ShowDialog() == true) LoadClasses();
        }

        private void OpenEditClassWindow(object obj)
        {
            if (SelectedClass == null) return;
            var window = new EditClassWindow(SelectedClass);
            if (window.ShowDialog() == true) LoadClasses();
        }
        private void OpenEditDishWindow(object obj)
        {
            if (SelectedDish == null) return;

            var window = new EditDishWindow(SelectedDish);
            if (window.ShowDialog() == true) LoadDishes();
        }
        private void DeleteClass(object obj)
        {
            if (SelectedClass == null) return;

            var result = MessageBox.Show("Удалить этот класс?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var cls = context.Class.Find(SelectedClass.IdClass);
                    if (cls != null)
                    {
                        context.Class.Remove(cls);
                        context.SaveChanges();
                        LoadClasses();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        public void LoadClasses()
        {
            Classes.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                foreach (var cls in context.Class.Include(c => c.Teacher).ToList())
                {
                    Classes.Add(cls);
                }
            }
        }
        #endregion

        #region Блюда
        private bool CanExecuteDishCommand(object obj) => SelectedDish != null;

        private void OpenAddDishWindow(object obj)
        {
            var window = new AddDishWindow();
            if (window.ShowDialog() == true) LoadDishes();
        }

        

        private void DeleteDish(object obj)
        {
            if (SelectedDish == null) return;

            var result = MessageBox.Show("Удалить это блюдо?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var dish = context.Dish.Find(SelectedDish.IdDish);
                    if (dish != null)
                    {
                        context.Dish.Remove(dish);
                        context.SaveChanges();
                        LoadDishes();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        public void LoadDishes()
        {
            Dishes.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                foreach (var dish in context.Dish.Include(d => d.Ingredient).ToList())
                {
                    Dishes.Add(dish);
                }
            }
        }
        #endregion

        #region План питания
        private bool CanExecuteMealPlanCommand(object obj) => SelectedMealPlan != null;

        private void OpenAddMealPlanWindow(object obj)
        {
            var window = new AddMealPlanWindow();
            if (window.ShowDialog() == true) LoadMealPlans();
        }

        private void OpenEditMealPlanWindow(object obj)
        {
            if (SelectedMealPlan == null) return;

            try
            {
                // Перезагружаем полные данные
                using (var context = new SchoolCanteenEntities())
                {
                    var fullMealPlan = context.MealPlan
                        .Include(mp => mp.Student)
                        .Include(mp => mp.Meal)
                        .Include(mp => mp.Day)
                        .Include(mp => mp.Paid)
                        .FirstOrDefault(mp => mp.IdMealPlan == SelectedMealPlan.IdMealPlan);

                    if (fullMealPlan == null) return;

                    var window = new EditMealPlanWindow(fullMealPlan);
                    if (window.ShowDialog() == true) LoadMealPlans();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия: {ex.Message}");
            }
        }

        private void DeleteMealPlan(object obj)
        {
            if (SelectedMealPlan == null) return;

            var result = MessageBox.Show("Удалить эту запись плана питания?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var plan = context.MealPlan.Find(SelectedMealPlan.IdMealPlan);
                    if (plan != null)
                    {
                        context.MealPlan.Remove(plan);
                        context.SaveChanges();
                        LoadMealPlans();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        public void LoadMealPlans()
        {
            MealPlans.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                var plans = context.MealPlan
                    .Include(mp => mp.Student)
                    .Include(mp => mp.Meal)
                    .Include(mp => mp.Day)
                    .Include(mp => mp.Paid)
                    .ToList();

                foreach (var plan in plans)
                {
                    MealPlans.Add(plan);
                }
            }
        }
        #endregion

        // Остальные методы загрузки данных
        public void LoadStudents()
        {
            Students.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                foreach (var student in context.Student.Include(s => s.Class).ToList())
                {
                    Students.Add(student);
                }
            }
        }
    }
}