using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class PersonModel : BaseModel
    {
        [Required]
        [StringLength(50)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name cannot be longer than 50 characters."), RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [DisplayName("First and Middle name")]
        // will use FirstName as column name isntead of FirstMidName
        // [Column("FirstName")]
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

    }
}
