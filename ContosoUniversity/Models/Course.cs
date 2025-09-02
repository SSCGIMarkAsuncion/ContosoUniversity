using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Course : BaseModel
    {
        // do not auto increment
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [BindProperty(Name = "Id")]
        [DisplayName("Course Code")]
        new public int Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(1, 5)]
        public int Credits { get; set; }
        public int DepartmentID { get; set; }


        [ValidateNever]
        public virtual Department Department { get; set; }
        [ValidateNever]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        [ValidateNever]
        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}
