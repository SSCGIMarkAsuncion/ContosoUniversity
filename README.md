# EFCore Setup
```powershell
dotnet tool install --create-manifest-if-needed dotnet-ef
dotnet ef migrations add InitialCreate --project ContosoUniversity
dotnet ef database update --project ContosoUniversity
```

# ViewBag and ViewData
## ViewData - is a Dict type object to pass data from controller to view
## ViewBag - is a wrapper to ViewData that uses dynamic feature of C#


# Join Table

```csharp
    public class Course
    {
		...
        public virtual ICollection<Instructor> Instructors { get; set; }
    }

    public class Instructor
    {
		...
        public virtual ICollection<Course> Courses { get; set; }
    }
```
When 2 or more models have a navigation property `dotnet-ef` will create a JoinTable this table will represent the connection between the two in the database
