using SchoolCanteenApp.Data;
using SchoolCanteenApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolCanteenApp.ViewModels
{
    public class ClassViewModel : BaseViewModel
    {
        private Class _currentClass;
        private List<Teacher> _teachers;

        public Class CurrentClass
        {
            get => _currentClass;
            set => SetProperty(ref _currentClass, value);
        }

        public List<Teacher> Teachers
        {
            get => _teachers;
            set => SetProperty(ref _teachers, value);
        }

        public ClassViewModel(List<Teacher> teachers)
        {
            CurrentClass = new Class();
            Teachers = teachers;
        }

        public ClassViewModel(Class editClass, List<Teacher> teachers) : this(teachers)
        {
            CurrentClass = editClass ?? throw new ArgumentNullException(nameof(editClass));
        }

        public async Task SaveAsync(object parameter)
        {
            try
            {
                using (var context = new SchoolCanteenContext())
                {
                    if (CurrentClass.IdClass == 0)
                    {
                        context.Classes.Add(CurrentClass);
                    }
                    else
                    {
                        context.Entry(CurrentClass).State = EntityState.Modified;
                    }

                    await context.SaveChangesAsync();
                }

                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}