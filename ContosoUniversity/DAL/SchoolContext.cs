using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure.Interception;

namespace ContosoUniversity.DAL
{
    public class SchoolContext : DbContext
    {

        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.AddInterceptors(new SchoolInterceptorLogging());
            //optionsBuilder.AddInterceptors(new SchoolInterceptorTransientErrors());
            optionsBuilder.UseSeeding((context, _) =>
            {
                SchoolInitializer.Seed((SchoolContext)context);
            });
        }

        #region SeedOnlyOnCreate
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Student>().HasData(
        //    new Student { ID=1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2005-09-01") },
        //    new Student { ID=2, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2002-09-01") },
        //    new Student { ID=3, FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2003-09-01") },
        //    new Student { ID=4, FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2002-09-01") },
        //    new Student { ID=5, FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2002-09-01") },
        //    new Student { ID=6, FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2001-09-01") },
        //    new Student { ID=7, FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2003-09-01") },
        //    new Student { ID=8, FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2005-09-01") }
        //    );

        //    modelBuilder.Entity<Instructor>().HasData(
        //    );

        //    modelBuilder.Entity<Course>().HasData(
        //    new Course { CourseID = 1050, Title = "Chemistry", Credits = 3, },
        //    new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3, },
        //    new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3, },
        //    new Course { CourseID = 1045, Title = "Calculus", Credits = 4, },
        //    new Course { CourseID = 3141, Title = "Trigonometry", Credits = 4, },
        //    new Course { CourseID = 2021, Title = "Composition", Credits = 3, },
        //    new Course { CourseID = 2042, Title = "Literature", Credits = 4, }
        //    );

        //    modelBuilder.Entity<Enrollment>().HasData(
        //    new Enrollment { EnrollmentID=1, StudentID = 1, CourseID = 1050, Grade = Grade.A },
        //    new Enrollment { EnrollmentID=2, StudentID = 1, CourseID = 4022, Grade = Grade.C },
        //    new Enrollment { EnrollmentID=3, StudentID = 1, CourseID = 4041, Grade = Grade.B },
        //    new Enrollment { EnrollmentID=4, StudentID = 2, CourseID = 1045, Grade = Grade.B },
        //    new Enrollment { EnrollmentID=5, StudentID = 2, CourseID = 3141, Grade = Grade.F },
        //    new Enrollment { EnrollmentID=6, StudentID = 2, CourseID = 2021, Grade = Grade.F },
        //    new Enrollment { EnrollmentID=7, StudentID = 3, CourseID = 1050 },
        //    new Enrollment { EnrollmentID=8, StudentID = 4, CourseID = 1050, },
        //    new Enrollment { EnrollmentID=9, StudentID = 4, CourseID = 4022, Grade = Grade.F },
        //    new Enrollment { EnrollmentID=10, StudentID = 5, CourseID = 4041, Grade = Grade.C },
        //    new Enrollment { EnrollmentID=11, StudentID = 6, CourseID = 1045 },
        //    new Enrollment { EnrollmentID=12, StudentID = 7, CourseID = 3141, Grade = Grade.A }
        //    );

        //    // When deleting department Entity that has a foreign key to
        //    // Department will be set to null instead of cascade delete
        //    modelBuilder.Entity<Department>()
        //        .HasOne(d => d.Administrator)
        //        .WithMany()
        //        .OnDelete(DeleteBehavior.Restrict);
        //}
        #endregion
    }
}
