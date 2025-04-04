using SchoolCanteenApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolCanteenApp.ViewModel
{
    public class StudentViewModel
    {
        public Student CurrentStudent { get; set; }
        public List<Class> Classes { get; set; }

        public StudentViewModel(List<Class> classes)
        {
            CurrentStudent = new Student();
            Classes = classes;
        }

        public StudentViewModel(Student student, List<Class> classes) : this(classes)
        {
            CurrentStudent = student;
        }
    }

}
