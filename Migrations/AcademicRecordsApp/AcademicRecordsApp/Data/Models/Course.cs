using System;
using System.ComponentModel.DataAnnotations;
using AcademicRecordsApp.Models;

namespace AcademicRecordsApp.Data.Models
{
	public class Course
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		public virtual ICollection<Exam> Exams { get; set; }
			= new HashSet<Exam>();

		public ICollection<Student> Students { get; set; } = new List<Student>();

        public ICollection<StudentCourse> StudentsCourses { get; set; }
            = new HashSet<StudentCourse>();
    }
}

