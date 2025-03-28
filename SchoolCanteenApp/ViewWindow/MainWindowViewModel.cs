using System;
using SchoolCanteenApp.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Data.Entity;

namespace SchoolCanteenApp.ViewWindow
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _firstName;
        private string _lastName;
        private int? _idClass;
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;
        private Student _newStudent;
        private ObservableCollection<Class> _classes;
        private ObservableCollection<Dish> _dishes;
        private ObservableCollection<MealPlan> _mealPlans;

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

        public MainWindowViewModel()
        {
            // Инициализация всех коллекций
            Students = new ObservableCollection<Student>();
            Classes = new ObservableCollection<Class>();
            Dishes = new ObservableCollection<Dish>();
            MealPlans = new ObservableCollection<MealPlan>();

            NewStudent = new Student();
            LoadAllData(); // Загрузка всех данных при инициализации
        }

        public void LoadAllData()
        {
            LoadStudents();
            LoadClasses();
            LoadDishes();
            LoadMealPlans();
        }

        public void LoadStudents()
        {
            Students.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                var students = context.Student
                    .Include(s => s.Class) // Добавляем подгрузку классов
                    .ToList();

                foreach (var student in students)
                {
                    Students.Add(student);
                }
            }
        }

        public void LoadClasses()
        {
            Classes.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                var classes = context.Class
                    .Include(c => c.Teacher) // Подгружаем учителей
                    .ToList();

                foreach (var cls in classes)
                {
                    Classes.Add(cls);
                }
            }
        }

        public void LoadDishes()
        {
            Dishes.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                var dishes = context.Dish
                    .Include(d => d.Ingredient) // Подгружаем ингредиенты
                    .ToList();

                foreach (var dish in dishes)
                {
                    Dishes.Add(dish);
                }
            }
        }

        public void LoadMealPlans()
        {
            MealPlans.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                var mealPlans = context.MealPlan
                    .Include(mp => mp.Student)
                    .Include(mp => mp.Meal)
                    .Include(mp => mp.Day)
                    .Include(mp => mp.Paid)
                    .ToList();

                foreach (var plan in mealPlans)
                {
                    MealPlans.Add(plan);
                }
            }
        }
        // Изменённые свойства согласно модели Student
        public string FirstName
        {
            get => _firstName;
            set => SetPropertyChanged(ref _firstName, value, nameof(FirstName));
        }

        public string LastName
        {
            get => _lastName;
            set => SetPropertyChanged(ref _lastName, value, nameof(LastName));
        }

        public int? IdClass
        {
            get => _idClass;
            set => SetPropertyChanged(ref _idClass, value, nameof(IdClass));
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetPropertyChanged(ref _students, value, nameof(Students));
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set => SetPropertyChanged(ref _selectedStudent, value, nameof(SelectedStudent));
        }

        public Student NewStudent
        {
            get => _newStudent;
            set => SetPropertyChanged(ref _newStudent, value, nameof(NewStudent));
        }


        public void LoadStudent()
        {
            Students.Clear();
            using (var context = new SchoolCanteenEntities())
            {
                var students = context.Student.ToList();
                foreach (var student in students)
                {
                    Students.Add(student);
                }
            }
        }

        public void DeleteStudent()
        {
            if (SelectedStudent == null) return;

            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var studentToDelete = context.Student.Find(SelectedStudent.IdStudent);
                    if (studentToDelete != null)
                    {
                        context.Student.Remove(studentToDelete);
                        context.SaveChanges();
                        LoadStudents(); // Используем обновленный метод с подгрузкой
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool AddNewStudent()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    context.Student.Add(new Student
                    {
                        FirstName = NewStudent.FirstName,
                        LastName = NewStudent.LastName,
                        IdClass = NewStudent.IdClass
                    });
                    context.SaveChanges();
                    LoadStudents(); // Используем обновленный метод с подгрузкой
                    NewStudent = new Student();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}