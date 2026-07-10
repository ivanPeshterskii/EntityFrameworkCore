using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AcademicRecordsApp.Data.Models;

namespace AcademicRecordsApp.Models
{
    public partial class Student
    {
        public Student()
        {
            Grades = new HashSet<Grade>();
            Courses = new List<Course>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        public virtual ICollection<Grade> Grades { get; set; }

        public ICollection<Course> Courses { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; }
            = new HashSet<StudentCourse>();
    }
}
