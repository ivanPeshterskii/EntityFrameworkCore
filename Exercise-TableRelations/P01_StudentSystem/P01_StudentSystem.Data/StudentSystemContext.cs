using System;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
	public class StudentSystemContext : DbContext
	{
		public StudentSystemContext()
		{
		}

		public StudentSystemContext(DbContextOptions dbContextOptions)
			: base (dbContextOptions)
		{

		}

		DbSet<Student> Students { get; set; } = null!;

		DbSet<Course> Courses { get; set; } = null!;

		DbSet<Homework> Homeworks { get; set; } = null!;

		DbSet<Resource> Resources { get; set; } = null!;

		DbSet<StudentCourse> StudentCourses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<StudentCourse>()
				.HasKey(k => new { k.StudentId, k.CourseId });

			modelBuilder.Entity<StudentCourse>()
				.HasOne(s => s.Student)
				.WithMany(s => s.StudentsCourses)
				.HasForeignKey(fk => fk.StudentId);

			modelBuilder.Entity<StudentCourse>()
				.HasOne(c => c.Course)
				.WithMany(c => c.StudentsCourses)
				.HasForeignKey(fk => fk.CourseId);

			modelBuilder.Entity<Homework>()
				.HasOne(s => s.Student)
				.WithMany(s => s.Homeworks)
				.HasForeignKey(fk => fk.StudentId);

			modelBuilder.Entity<Homework>()
				.HasOne(c => c.Course)
				.WithMany(s => s.Homeworks)
				.HasForeignKey(fk => fk.CourseId);

			modelBuilder.Entity<Resource>()
				.HasOne(c => c.Course)
				.WithMany(r => r.Resources)
				.HasForeignKey(fk => fk.ResourceId);

            modelBuilder.Entity<Student>()
				.Property(s => s.PhoneNumber)
				.IsUnicode(false)
				.IsRequired(false);

            modelBuilder.Entity<Resource>()
                .Property(r => r.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Homework>()
                .Property(h => h.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}

