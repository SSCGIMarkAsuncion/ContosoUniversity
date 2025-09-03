using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Utils.Service
{
    public class Filter
    {
        private const char DESC = '▼';
        private const char ASC = '▲';

        public string SortOrder { get; set; } = "";
        public string Q { get; set; } = "";
        public int Page { get; set; } = 1;

        public bool IsArchive { get; set; } = false;

        public struct Result<T>
        {
            public IQueryable<T> Queries { get; set; }
            public string CurrentSortParam { get; set; }
            public string NameSortParam { get; set; }
            public string? DateSortParam { get; set; }
            public string? IdCodeSortParam { get; set; }
            public string? CreditSortParam { get; set; }
            public string? DepartmentSortParam { get; set; }

            public char NameSortSuffix { get; set; }
            public char? DateSortSuffix { get; set; }
            public char? IdCodeSortSuffix { get; set; }
            public char? CreditSortSuffix { get; set; }
            public char? DepartmentSortSuffix { get; set; }
        }

        public Result<Course> ApplyToCourses(DbSet<Course> context)
        {
            Sort.Type stype = Sort.From(SortOrder);

            Sort.Type nameSortParam = Sort.Type.NAME_DESC;
            Sort.Type idSortParam = Sort.Type.ID_DESC;
            Sort.Type creditSortParam = Sort.Type.CREDIT_DESC;
            Sort.Type departmentSortParam = Sort.Type.DEPARTMENT_DESC;

            char nameSortSuffix = DESC;
            char idSortSuffix = DESC;
            char creditSortSuffix = DESC;
            char departmentSortSuffix = DESC;

            var courses = (from s in context
                            where s.Deleted == IsArchive
                            select s);

            if (!string.IsNullOrEmpty(Q))
            {
                courses = courses.Where(
                    s => s.Title.ToLower().Contains(Q.ToLower()) || s.Department.Name.ToLower().Contains(Q.ToLower())
                );
            }

            switch (stype)
            {
                case Sort.Type.NAME_ASC:
                    courses = courses.OrderBy(s => s.Title);
                    nameSortSuffix = ASC;
                    break;
                case Sort.Type.NAME_DESC:
                    courses = courses.OrderByDescending(s => s.Title);
                    nameSortParam = Sort.Type.NAME_ASC;
                    break;
                case Sort.Type.ID_ASC:
                    courses = courses.OrderBy(s => s.Id);
                    idSortParam = Sort.Type.ID_DESC;
                    idSortSuffix = ASC;
                    break;
                case Sort.Type.ID_DESC:
                    courses = courses.OrderByDescending(s => s.Id);
                    idSortParam = Sort.Type.ID_ASC;
                    break;
                case Sort.Type.CREDIT_ASC:
                    courses = courses.OrderBy(s => s.Credits);
                    creditSortParam = Sort.Type.CREDIT_DESC;
                    creditSortSuffix = ASC;
                    break;
                case Sort.Type.CREDIT_DESC:
                    courses = courses.OrderByDescending(s => s.Credits);
                    creditSortParam = Sort.Type.CREDIT_ASC;
                    break;
                case Sort.Type.DEPARTMENT_ASC:
                    courses = courses.OrderBy(s => s.Credits);
                    departmentSortParam = Sort.Type.DEPARTMENT_DESC;
                    departmentSortSuffix = ASC;
                    break;
                case Sort.Type.DEPARTMENT_DESC:
                    courses = courses.OrderByDescending(s => s.DepartmentID);
                    departmentSortParam = Sort.Type.DEPARTMENT_ASC;
                    break;
                default:
                    break;
            }
            courses = courses.Include(c => c.Department);
            return new Result<Course>()
            {
                Queries = courses,
                CurrentSortParam = stype.ToString().ToLower(),
                NameSortParam = nameSortParam.ToString().ToLower(),
                NameSortSuffix = nameSortSuffix,
                IdCodeSortParam = idSortParam.ToString().ToLower(),
                IdCodeSortSuffix = idSortSuffix,
                CreditSortParam = creditSortParam.ToString().ToLower(),
                CreditSortSuffix = creditSortSuffix,
                DepartmentSortParam = departmentSortParam.ToString().ToLower(),
                DepartmentSortSuffix = departmentSortSuffix
            };
        }

        public Result<Student> ApplyToStudents(DbSet<Student> context)
        {
            Sort.Type stype = Sort.From(SortOrder);

            Sort.Type nameSortParam = Sort.Type.NAME_DESC;
            Sort.Type dateSortParam = Sort.Type.DATE_DESC;
            char nameSortSuffix = DESC;
            char dateSortSuffix = DESC;

            var students = (from s in context
                            where s.Deleted == IsArchive
                            select s);

            if (!string.IsNullOrEmpty(Q))
            {
                students = students.Where(s => s.LastName.ToLower().Contains(Q) || s.FirstMidName.ToLower().Contains(Q));
            }

            switch (stype)
            {
                case Sort.Type.NAME_ASC:
                    students = students.OrderBy(s => s.LastName);
                    nameSortSuffix = ASC;
                    break;
                case Sort.Type.NAME_DESC:
                    students = students.OrderByDescending(s => s.LastName);
                    nameSortParam = Sort.Type.NAME_ASC;
                    break;
                case Sort.Type.DATE_ASC:
                    students = students.OrderBy(s => s.EnrollmentDate);
                    dateSortSuffix = ASC;
                    break;
                case Sort.Type.DATE_DESC:
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    dateSortParam = Sort.Type.DATE_ASC;
                    break;
                default:
                    break;
            }
            return new Result<Student>()
            {
                Queries = students,
                CurrentSortParam = stype.ToString().ToLower(),
                NameSortParam = nameSortParam.ToString().ToLower(),
                NameSortSuffix = nameSortSuffix,
                DateSortParam = dateSortParam.ToString().ToLower(),
                DateSortSuffix = dateSortSuffix,
            };
        }
    }
}
