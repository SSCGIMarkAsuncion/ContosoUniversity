using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Utils.Service
{
    public class SoftDelete
    {
        public static async Task<int> Delete(SchoolContext context, BaseModel model)
        {
            model.Deleted = true;
            context.Attach(model);
            context.Entry(model).Property(m => m.Deleted).IsModified = true;
            return await context.SaveChangesAsync();
        }

        public static async Task<int> Restore(SchoolContext context, BaseModel model)
        {
            model.Deleted = false;
            context.Attach(model);
            context.Entry(model).Property(m => m.Deleted).IsModified = true;
            return await context.SaveChangesAsync();
        }
    }
}
