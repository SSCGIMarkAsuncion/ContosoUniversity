using System.Diagnostics;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.Utils;
using ContosoUniversity.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchoolContext _context;
        private List<BreadCrumb> _breadcrums = new List<BreadCrumb>
        {
            new BreadCrumb() { Name="Home", LinkTo="/" },
            new BreadCrumb() { Name="About", LinkTo="/About" },
        };

        public AboutController(ILogger<HomeController> logger, SchoolContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var data = from student in _context.Students
               group student by student.EnrollmentDate into dateGroup
               select new EnrollmentDateGroup()
               {
                   EnrollmentDate = dateGroup.Key,
                   StudentCount = dateGroup.Count()
               };

            _breadcrums[1].IsCurrent = true;
            ViewBag.BreadCrumbs = _breadcrums;

            return View(data.ToList());
        }
    }
}
