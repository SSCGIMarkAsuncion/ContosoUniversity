using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Utils.Service
{
    public class Filter
    {
        public string SortOrder { get; set; } = "";
        public string Q { get; set; } = "";
        public int Page { get; set; } = 1;

        public bool IsArchive { get; set; } = false;

        public struct Result<T>
        {
            public IQueryable<T> Queries { get; set; }
            public string CurrentSortParam { get; set; }
            public string NameSortParam { get; set; }
            public string DateSortParam { get; set; }
            public char NameSortSuffix { get; set; }
            public char DateSortSuffix { get; set; }
        }

        public Result<Student> ApplyToStudents(DbSet<Student> context)
        {
            Sort.Type stype = Sort.From(SortOrder);

            Sort.Type nameSortParam = Sort.Type.NAME_DESC;
            Sort.Type dateSortParam = Sort.Type.DATE_DESC;
            char nameSortSuffix = '▼'; //'▲';
            char dateSortSuffix = '▼';

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
                    nameSortSuffix = '▲';
                    break;
                case Sort.Type.NAME_DESC:
                    students = students.OrderByDescending(s => s.LastName);
                    nameSortParam = Sort.Type.NAME_ASC;
                    break;
                case Sort.Type.DATE_ASC:
                    students = students.OrderBy(s => s.EnrollmentDate);
                    dateSortSuffix = '▲';
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
