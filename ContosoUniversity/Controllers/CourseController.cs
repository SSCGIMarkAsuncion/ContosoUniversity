using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.Utils.Service;
using X.PagedList.Extensions;
using ContosoUniversity.Utils;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private readonly SchoolContext _context;

        public CourseController(SchoolContext context)
        {
            _context = context;
        }

        private IQueryable<Course> ApplyFilter(Filter filter)
        {
            Filter.Result<Course> results = filter.ApplyToCourses(_context.Courses);
            ViewBag.NameSortParam = results.NameSortParam;
            ViewBag.IdCodeSortParam = results.IdCodeSortParam;
            ViewBag.CreditSortParam = results.CreditSortParam;
            ViewBag.DepartmentSortParam = results.DepartmentSortParam;

            ViewBag.NameSortSuffix = results.NameSortSuffix;
            ViewBag.IdCodeSortSuffix = results.IdCodeSortSuffix;
            ViewBag.CreditSortSuffix = results.CreditSortSuffix;
            ViewBag.DepartmentSortSuffix = results.DepartmentSortSuffix;

            ViewBag.QueryParam = filter.Q;
            ViewBag.IsArchive = filter.IsArchive;

            ViewBag.CurrentSortParam = results.CurrentSortParam;
            ViewBag.CurrentPage = filter.Page;

            return results.Queries;
        }

        // GET: Course
        public async Task<IActionResult> Index([FromQuery] Filter filter)
        {
            var query = ApplyFilter(filter);

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Course", LinkTo="/Course", IsCurrent = true },
            };
            ViewBag.LinkTo = "Index";
            return View(query.ToPagedList(filter.Page, 5));
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Course", LinkTo="/Course" },
                new BreadCrumb() { Name="Details", LinkTo=$"/Course/Details/{id}", IsCurrent = true },
            };

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = GetDepartmentSelectList();
            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Course", LinkTo="/Course" },
                new BreadCrumb() { Name="Create", LinkTo=$"/Course/Create", IsCurrent = true },
            };
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,DepartmentID")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", course.DepartmentID);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("error", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Course", LinkTo="/Course" },
                new BreadCrumb() { Name="Create", LinkTo=$"/Course/Create", IsCurrent = true },
            };
            ViewData["DepartmentID"] = GetDepartmentSelectList();
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = GetDepartmentSelectList();

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Course", LinkTo="/Course" },
                new BreadCrumb() { Name="Edit", LinkTo=$"/Course/Edit/{id}", IsCurrent = true },
            };
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Credits,DepartmentID")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Courses.Update(course);
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

            var cs = _context.Courses.Single(s => s.Id == id);
            if (cs != null)
            {
                ViewBag.BreadCrumbs = new List<BreadCrumb>
                {
                    new BreadCrumb() { Name="Home", LinkTo="/" },
                    new BreadCrumb() { Name="Course", LinkTo="/Course" },
                    new BreadCrumb() { Name="Edit", LinkTo=$"/Course/Edit/{id}", IsCurrent = true },
                };
                ViewData["DepartmentID"] = GetDepartmentSelectList();
                return View(course);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Course/Delete/5
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
            var course = await _context.Courses
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Course", LinkTo="/Course" },
                new BreadCrumb() { Name="Delete", LinkTo=$"/Course/Delete/{id}", IsCurrent = true },
            };

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var del = new Course() { Id = id, Deleted = true };
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
            var course = await _context.Courses
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null) return NotFound();

            ViewBag.BreadCrumbs = new List<BreadCrumb>
            {
                new BreadCrumb() { Name="Home", LinkTo="/" },
                new BreadCrumb() { Name="Course", LinkTo="/Course" },
                new BreadCrumb() { Name="Restore", LinkTo=$"/Course/Restore/{id}", IsCurrent = true },
            };
            return View(course);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        // POST: Course/Restore/5
        public async Task<IActionResult> ConfirmRestore(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var restored = new Course() { Id = (int)id, Deleted = false };
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

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        private SelectList GetDepartmentSelectList()
        {
            return new SelectList(_context.Departments, "Id", "Name");
        }
    }
}
