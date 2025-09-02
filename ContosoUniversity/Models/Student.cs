using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Student : PersonModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [ValidateNever]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
