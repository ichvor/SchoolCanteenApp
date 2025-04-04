using SchoolCanteenApp.Model;
using SchoolCanteenApp.View.Windows;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolCanteenApp.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ObservableCollection<Student> _students = new ObservableCollection<Student>();
        private Student _selectedStudent;
        private ObservableCollection<Class> _classes = new ObservableCollection<Class>();
        private ObservableCollection<Dish> _dishes = new ObservableCollection<Dish>();
        private ObservableCollection<MealPlan> _mealPlans = new ObservableCollection<MealPlan>();
        private ObservableCollection<Teacher> _teachers = new ObservableCollection<Teacher>();

        private Class _selectedClass;
        private Dish _selectedDish;
        private MealPlan _selectedMealPlan;
        private bool _isLoading;
        private Teacher _selectedTeacher;


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
        public ICommand AddTeacherCommand { get; }
        public ICommand EditTeacherCommand { get; }
        public ICommand DeleteTeacherCommand { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetPropertyChanged(ref _isLoading, value, nameof(IsLoading));
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetPropertyChanged(ref _students, value, nameof(Students));
        }

        public ObservableCollection<Teacher> Teachers
        {
            get => _teachers;
            set => SetPropertyChanged(ref _teachers, value, nameof(Teachers));
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

        public Student SelectedStudent { get => _selectedStudent; set => SetPropertyChanged(ref _selectedStudent, value); }       
        public Teacher SelectedTeacher { get => _selectedTeacher; set => SetPropertyChanged(ref _selectedTeacher, value); }
        public Class SelectedClass { get => _selectedClass; set => SetPropertyChanged(ref _selectedClass, value); }
        public Dish SelectedDish { get => _selectedDish; set => SetPropertyChanged(ref _selectedDish, value); }
        public MealPlan SelectedMealPlan { get => _selectedMealPlan; set => SetPropertyChanged(ref _selectedMealPlan, value); }

        public MainWindowViewModel()
        {
            AddStudentCommand = new RelayCommand(OpenAddStudentWindow);
            EditStudentCommand = new RelayCommand(OpenEditStudentWindow, CanExecuteStudentCommand);
            DeleteStudentCommand = new RelayCommand(DeleteStudentAsync, CanExecuteStudentCommand);

            AddClassCommand = new RelayCommand(OpenAddClassWindow);
            EditClassCommand = new RelayCommand(OpenEditClassWindow, CanExecuteClassCommand);
            DeleteClassCommand = new RelayCommand(DeleteClassAsync, CanExecuteClassCommand);

            AddDishCommand = new RelayCommand(OpenAddDishWindow);
            EditDishCommand = new RelayCommand(OpenEditDishWindow, CanExecuteDishCommand);
            DeleteDishCommand = new RelayCommand(DeleteDishAsync, CanExecuteDishCommand);

            AddMealPlanCommand = new RelayCommand(OpenAddMealPlanWindow);
            EditMealPlanCommand = new RelayCommand(OpenEditMealPlanWindow, CanExecuteMealPlanCommand);
            DeleteMealPlanCommand = new RelayCommand(DeleteMealPlanAsync, CanExecuteMealPlanCommand);
            AddTeacherCommand = new RelayCommand(OpenAddTeacherWindow);
            EditTeacherCommand = new RelayCommand(OpenEditTeacherWindow, CanExecuteTeacherCommand);
            DeleteTeacherCommand = new RelayCommand(DeleteTeacherAsync, CanExecuteTeacherCommand);

            LoadAllDataAsync().ConfigureAwait(false);
        }

        #region Data Loading
        private async Task LoadAllDataAsync()
        {
            IsLoading = true;
            try
            {
                await Task.WhenAll(
                    LoadStudentsAsync(),
                    LoadTeachersAsync(),
                    LoadClassesAsync(),
                    LoadDishesAsync(),
                    LoadMealPlansAsync()
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var students = await context.Student.Include(s => s.Class).ToListAsync();
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Students.Clear();
                        foreach (var student in students) Students.Add(student);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки учеников: {ex.Message}");
            }
        }
        private async Task LoadTeachersAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var teachers = await context.Teacher.ToListAsync();
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Teachers.Clear();
                        foreach (var teacher in teachers) Teachers.Add(teacher);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки учителей: {ex.Message}");
            }
        }
        private async Task LoadClassesAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var classes = await context.Class.Include(c => c.Teacher).ToListAsync();
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Classes.Clear();
                        foreach (var cls in classes) Classes.Add(cls);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки классов: {ex.Message}");
            }
        }

        private async Task LoadDishesAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var dishes = await context.Dish.Include(d => d.Ingredient).ToListAsync();
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Dishes.Clear();
                        foreach (var dish in dishes) Dishes.Add(dish);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки блюд: {ex.Message}");
            }
        }

        private async Task LoadMealPlansAsync()
        {
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var plans = await context.MealPlan
                        .Include(mp => mp.Student)
                        .Include(mp => mp.Meal)
                        .Include(mp => mp.Day)
                        .Include(mp => mp.Paid)
                        .ToListAsync();

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        MealPlans.Clear();
                        foreach (var plan in plans) MealPlans.Add(plan);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки планов питания: {ex.Message}");
            }
        }
        #endregion

        #region Student Commands
        private bool CanExecuteStudentCommand(object obj) => SelectedStudent != null;

        private void OpenAddStudentWindow(object obj)
        {
            var window = new AddStudentWindow();
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadStudentsAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private void OpenEditStudentWindow(object obj)
        {
            if (SelectedStudent == null) return;

            var window = new EditStudentWindow(SelectedStudent);
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadStudentsAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private async void DeleteStudentAsync(object obj)
        {
            if (SelectedStudent == null) return;

            var result = MessageBox.Show("Удалить этого ученика?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            IsLoading = true;
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var student = await context.Student.FindAsync(SelectedStudent.IdStudent).ConfigureAwait(false);
                    if (student != null)
                    {
                        context.Student.Remove(student);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                        await LoadStudentsAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion

        #region Class Commands
        private bool CanExecuteClassCommand(object obj) => SelectedClass != null;

        private void OpenAddClassWindow(object obj)
        {
            var window = new AddClassWindow();
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadClassesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private void OpenEditClassWindow(object obj)
        {
            if (SelectedClass == null) return;

            var window = new EditClassWindow(SelectedClass);
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadClassesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private async void DeleteClassAsync(object obj)
        {
            if (SelectedClass == null) return;

            var result = MessageBox.Show("Удалить этот класс?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            IsLoading = true;
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var cls = await context.Class.FindAsync(SelectedClass.IdClass).ConfigureAwait(false);
                    if (cls != null)
                    {
                        context.Class.Remove(cls);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                        await LoadClassesAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion

        #region Dish Commands
        private bool CanExecuteDishCommand(object obj) => SelectedDish != null;

        private void OpenAddDishWindow(object obj)
        {
            var window = new AddDishWindow();
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadDishesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private void OpenEditDishWindow(object obj)
        {
            if (SelectedDish == null) return;

            var window = new EditDishWindow(SelectedDish);
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadDishesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private async void DeleteDishAsync(object obj)
        {
            if (SelectedDish == null) return;

            var result = MessageBox.Show("Удалить это блюдо?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            IsLoading = true;
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var dish = await context.Dish.FindAsync(SelectedDish.IdDish).ConfigureAwait(false);
                    if (dish != null)
                    {
                        context.Dish.Remove(dish);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                        await LoadDishesAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion

        #region MealPlan Commands
        private bool CanExecuteMealPlanCommand(object obj) => SelectedMealPlan != null;

        private void OpenAddMealPlanWindow(object obj)
        {
            var window = new AddMealPlanWindow();
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadMealPlansAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private void OpenEditMealPlanWindow(object obj)
        {
            if (SelectedMealPlan == null) return;

            var window = new EditMealPlanWindow(SelectedMealPlan);
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadMealPlansAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private async void DeleteMealPlanAsync(object obj)
        {
            if (SelectedMealPlan == null) return;

            var result = MessageBox.Show("Удалить запись плана питания?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            IsLoading = true;
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var plan = await context.MealPlan.FindAsync(SelectedMealPlan.IdMealPlan).ConfigureAwait(false);
                    if (plan != null)
                    {
                        context.MealPlan.Remove(plan);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                        await LoadMealPlansAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion

        #region Teacher Commands
        private bool CanExecuteTeacherCommand(object obj) => SelectedTeacher != null;

        private void OpenAddTeacherWindow(object obj)
        {
            var window = new AddTeacherWindow();
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadTeachersAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private void OpenEditTeacherWindow(object obj)
        {
            if (SelectedTeacher == null) return;

            var window = new EditTeacherWindow(SelectedTeacher);
            window.SaveClicked += async (sender, e) =>
            {
                IsLoading = true;
                try
                {
                    await LoadTeachersAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() => IsLoading = false);
                }
            };
            window.ShowDialog();
        }

        private async void DeleteTeacherAsync(object obj)
        {
            if (SelectedTeacher == null) return;

            var result = MessageBox.Show("Удалить этого учителя?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            IsLoading = true;
            try
            {
                using (var context = new SchoolCanteenEntities())
                {
                    var teacher = await context.Teacher.FindAsync(SelectedTeacher.IdTeacher).ConfigureAwait(false);
                    if (teacher != null)
                    {
                        // Проверяем, не используется ли учитель как классный руководитель
                        var isClassTeacher = await context.Class.AnyAsync(c => c.IdTeacher == teacher.IdTeacher);
                        if (isClassTeacher)
                        {
                            MessageBox.Show("Нельзя удалить учителя, так как он является классным руководителем!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        context.Teacher.Remove(teacher);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                        await LoadTeachersAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion
    }
}