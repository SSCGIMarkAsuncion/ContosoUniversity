using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ContosoUniversity.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        [ValidateNever]
        public bool Deleted { get; set; } = false;
    }
}
