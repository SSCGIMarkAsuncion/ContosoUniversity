using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength =2, ErrorMessage = "First name cannot be longer than 50 characters."), RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [DisplayName("First and Middle name")]
        // will use FirstName as column name isntead of FirstMidName
        // [Column("FirstName")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        [ValidateNever]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
