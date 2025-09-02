using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.DAL
{
    public class SchoolInitializer
    {
        public static void Seed(SchoolContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //context.Students.RemoveRange(context.Students);
            //context.Courses.RemoveRange(context.Courses);
            //context.Enrollments.RemoveRange(context.Enrollments);
            //context.Instructors.RemoveRange(context.Instructors);
            //context.OfficeAssignments.RemoveRange(context.OfficeAssignments);
            //context.Departments.RemoveRange(context.Departments);

            context.SaveChanges();

            var students = new List<Student>
            {
            new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
            new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
            new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
            };
            context.Set<Student>().AddRange(students);
            context.SaveChanges();

            var instructors = new List<Instructor>
            {
                new Instructor { FirstMidName = "Kim",     LastName = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstMidName = "Fadi",    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { FirstMidName = "Roger",   LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { FirstMidName = "Candace", LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { FirstMidName = "Roger",   LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12") }
            };
            context.Set<Instructor>().AddRange(instructors);
            context.SaveChanges();

            var departments = new List<Department>
            {
                new Department { Name = "English",     Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Abercrombie").Id },
                new Department { Name = "Mathematics", Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Fakhouri").Id },
                new Department { Name = "Engineering", Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Harui").Id },
                new Department { Name = "Economics",   Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Kapoor").Id }
            };
            context.Set<Department>().AddRange(departments);
            context.SaveChanges();

            var courses = new List<Course>
            {
                new Course {Id = 1050, Title = "Chemistry",      Credits = 3,
                  DepartmentID = departments.Single( s => s.Name == "Engineering").Id,
                  Instructors = new List<Instructor>()
                },
                new Course {Id = 4022, Title = "Microeconomics", Credits = 3,
                  DepartmentID = departments.Single( s => s.Name == "Economics").Id,
                  Instructors = new List<Instructor>()
                },
                new Course {Id = 4041, Title = "Macroeconomics", Credits = 3,
                  DepartmentID = departments.Single( s => s.Name == "Economics").Id,
                  Instructors = new List<Instructor>()
                },
                new Course {Id = 1045, Title = "Calculus",       Credits = 4,
                  DepartmentID = departments.Single( s => s.Name == "Mathematics").Id,
                  Instructors = new List<Instructor>()
                },
                new Course {Id = 3141, Title = "Trigonometry",   Credits = 4,
                  DepartmentID = departments.Single( s => s.Name == "Mathematics").Id,
                  Instructors = new List<Instructor>()
                },
                new Course {Id = 2021, Title = "Composition",    Credits = 3,
                  DepartmentID = departments.Single( s => s.Name == "English").Id,
                  Instructors = new List<Instructor>()
                },
                new Course {Id = 2042, Title = "Literature",     Credits = 4,
                  DepartmentID = departments.Single( s => s.Name == "English").Id,
                  Instructors = new List<Instructor>()
                },
            };
            context.Set<Course>().AddRange(courses);
            context.SaveChanges();

            // Add To Join Table
            AddInstructorToCourse(context, "Chemistry", "Kapoor");
            AddInstructorToCourse(context, "Chemistry", "Harui");
            AddInstructorToCourse(context, "Microeconomics", "Zheng");
            AddInstructorToCourse(context, "Macroeconomics", "Zheng");
            AddInstructorToCourse(context, "Calculus", "Fakhouri");
            AddInstructorToCourse(context, "Trigonometry", "Harui");
            AddInstructorToCourse(context, "Composition", "Abercrombie");
            AddInstructorToCourse(context, "Literature", "Abercrombie");

            var officeAssignments = new List<OfficeAssignment>
            {
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Fakhouri").Id,
                    Location = "Smith 17" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Harui").Id,
                    Location = "Gowan 27" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Kapoor").Id,
                    Location = "Thompson 304" },
            };
            context.Set<OfficeAssignment>().AddRange(officeAssignments);
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).Id,
                    Grade = Grade.A
                },
                 new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).Id,
                    Grade = Grade.C
                 },
                 new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                     StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Calculus" ).Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                     StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Composition" ).Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).Id
                 },
                 new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").Id,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").Id,
                    Grade = Grade.B
                 },
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Barzdukas").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry").Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Li").Id,
                    CourseID = courses.Single(c => c.Title == "Composition").Id,
                    Grade = Grade.B
                 },
                 new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Justice").Id,
                    CourseID = courses.Single(c => c.Title == "Literature").Id,
                    Grade = Grade.B
                 }
            };

            context.Set<Enrollment>().AddRange(enrollments);
            context.SaveChanges();
        }

        private static void AddInstructorToCourse(SchoolContext context, string courseTitle, string lastName)
        {
            // Assume courseTitle and lastName exist on both table
            Course crs = context.Courses.Single(c => c.Title == courseTitle);
            crs.Instructors.Add(context.Instructors.Single(i => i.LastName == lastName));
        }
    }
}
