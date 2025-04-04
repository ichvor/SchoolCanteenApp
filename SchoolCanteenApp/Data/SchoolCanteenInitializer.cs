using SchoolCanteenApp.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace SchoolCanteenApp.Data
{
    public class SchoolCanteenInitializer : DropCreateDatabaseIfModelChanges<SchoolCanteenContext>
    {
        protected override void Seed(SchoolCanteenContext context)
        {
            var teachers = new List<Teacher>
            {
                new Teacher { FirstName = "Асиет", LastName = "Шаова" },
                new Teacher { FirstName = "Виктория", LastName = "Скрипняк" }
            };
            teachers.ForEach(t => context.Teachers.Add(t));
            context.SaveChanges();

            var classes = new List<Class>
            {
                new Class { Class1 = "1А", IdTeacher = 1 },
                new Class { Class1 = "5А", IdTeacher = 2 }
            };
            classes.ForEach(c => context.Classes.Add(c));
            context.SaveChanges();

            var students = new List<Student>
            {
                new Student { FirstName = "Богдан", LastName = "Никитенко", IdClass = 2 },
                new Student { FirstName = "Ясмина", LastName = "Натхо", IdClass = 2 }
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();
        }
    }
}