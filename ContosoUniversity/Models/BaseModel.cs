using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ContosoUniversity.Models
{
    public class BaseModel
    {
        [ValidateNever]
        public bool Deleted { get; set; } = false;
    }
}
