using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.ViewModels
{
    public class EnrollmentDateGroup
    {


        [DisplayName("Enrollment Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EnrollmentDate { get; set; }

        [DisplayName("Number of Students")]
        public int StudentCount { get; set; }
    }
}
