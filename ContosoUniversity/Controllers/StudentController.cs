using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System.Diagnostics;
using ContosoUniversity.Utils;
using X.PagedList;
using X.PagedList.Extensions;
using ContosoUniversity.Utils.Service;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolContext _context;

        public StudentController(SchoolContext context)
        {
            _context = context;
        }

        public IQueryable<Student> ApplyFilter(Filter filter)
        {
            Filter.Result<Student> results = filter.ApplyToStudents(_context.Students);
            ViewBag.NameSortParam = results.NameSortParam;
            ViewBag.DateSortParam = results.DateSortParam;
            ViewBag.NameSortSuffix = results.NameSortSuffix;
            ViewBag.DateSortSuffix = results.DateSortSuffix;
            ViewBag.QueryParam = filter.Q;
            ViewBag.IsArchive = filter.IsArchive;

            ViewBag.CurrentSortParam = results.CurrentSortParam;
            ViewBag.CurrentPage = filter.Page;

            return results.Queries;
        }

        // GET: Student
        public async Task<IActionResult> Index([FromQuery] Filter filter)
        {
            var query = ApplyFilter(filter);
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Student", LinkTo="/Student", IsCurrent = true },
            };

            ViewBag.LinkTo = "Index";

            return View(query.ToPagedList(filter.Page, 5));
        }

        // GET: Student/Archive
        public async Task<IActionResult> Archive([FromQuery] Filter filter)
        {
            filter.IsArchive = true;
            var query = ApplyFilter(filter);
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Student", LinkTo="/Student" },
                new BreadCrumb() { Name="Archive", LinkTo="/Archive", IsCurrent = true }
            };
            ViewBag.LinkTo = "Archive";
            return View("Index", query.ToPagedList(filter.Page, 5));
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Student", LinkTo="/Student" },
                new BreadCrumb() { Name="Details", LinkTo=$"/Student/Details/{id}", IsCurrent = true },
            };
            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Student", LinkTo="/Student" },
                new BreadCrumb() { Name="Details", LinkTo=$"/Student/Create", IsCurrent = true },
            };
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidDataException _)
            {
                ModelState.AddModelError("error", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Student", LinkTo="/Student" },
                new BreadCrumb() { Name="Edit", LinkTo=$"/Student/Edit/{id}", IsCurrent = true },
            };
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Students.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("error", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception)
            {
                return Forbid();
            }

            var stud = _context.Students.Single(s => s.Id == id);
            if (stud != null)
                return View(student);
            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ModelState.AddModelError("error", "Delete failed. Try again, and if the problem persists see your system administrator.");
            }
            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Student", LinkTo="/Student" },
                new BreadCrumb() { Name="Delete", LinkTo=$"/Student/Delete/{id}", IsCurrent = true },
            };
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                //var student = await _context.Students.FindAsync(id);
                //if (student == null) return RedirectToAction(nameof(Index));
                //_context.Students.Remove(student);

                // Faster deletion
                // Skips query
                var del = new Student() { Id = id, Deleted = true };
                _context.Attach(del);
                _context.Entry(del).Property(s => s.Deleted).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException _)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        // GET: Student/Restore/5
        public async Task<IActionResult> Restore(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ModelState.AddModelError("error", "Restore failed. Try again, and if the problem persists see your system administrator.");
            }
            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) return NotFound();

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Student", LinkTo="/Student" },
                new BreadCrumb() { Name="Restore", LinkTo=$"/Student/Restore/{id}", IsCurrent = true },
            };
            return View(student);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        // POST: Student/Restore/5
        public async Task<IActionResult> ConfirmRestore(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var restored = new Student() { Id = (int)id, Deleted = false };
                _context.Attach(restored);
                _context.Entry(restored).Property(s => s.Deleted).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Restore), new { id, saveChangesError = true });
            }

            return RedirectToAction(nameof(Index), new { archive = true });
        }
    }
}
