using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("First and Middle name")]
        public string FirstMidName { get; set; }

        [DisplayName("Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [ValidateNever]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
