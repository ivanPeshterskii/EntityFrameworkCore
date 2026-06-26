using System;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
	public class StudentSystemContext : DbContext
	{
		public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
			: base(options)
		{
		}
        
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<StudentCourse> StudentsCourses { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<StudentCourse>()
				.HasKey(sc => new { sc.StudentId, sc.CourseId });

			modelBuilder.Entity<StudentCourse>()
				.HasOne(sc => sc.Student)
				.WithMany(s => s.StudentsCourses)
				.HasForeignKey(sc => sc.StudentId);

			modelBuilder.Entity<StudentCourse>()
				.HasOne(sc => sc.Course)
				.WithMany(s => s.StudentsCourses)
				.HasForeignKey(c => c.CourseId);
        }
    }
}

