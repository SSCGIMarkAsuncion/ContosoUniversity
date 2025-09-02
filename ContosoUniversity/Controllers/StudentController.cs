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

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolContext _context;

        public StudentController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index(string sortOrder, string q, int? page=1, bool? archive = false)
        {
            Sort.Type stype = Sort.From(string.IsNullOrEmpty(sortOrder)? "":sortOrder);

            Sort.Type nameSortParam = Sort.Type.NAME_DESC;
            Sort.Type dateSortParam = Sort.Type.DATE_DESC;
            char nameSortSuffix = '▼'; //'▲';
            char dateSortSuffix = '▼';

            var students = (from s in _context.Students
                            where s.Deleted == archive.GetValueOrDefault()
                            select s);

            int curPage = page.GetValueOrDefault();

            if (!string.IsNullOrEmpty(q))
            {
                students = students.Where(s => s.LastName.ToLower().Contains(q) || s.FirstMidName.ToLower().Contains(q));
            }

            switch (stype)
            {
                case Sort.Type.NAME_ASC:
                    students = students.OrderBy(s => s.LastName);
                    nameSortSuffix = '▲';
                    break;
                case Sort.Type.NAME_DESC:
                    students = students.OrderByDescending(s => s.LastName);
                    nameSortParam = Sort.Type.NAME_ASC;
                    break;
                case Sort.Type.DATE_ASC:
                    students = students.OrderBy(s => s.EnrollmentDate);
                    dateSortSuffix = '▲';
                    break;
                case Sort.Type.DATE_DESC:
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    dateSortParam = Sort.Type.DATE_ASC;
                    break;
                default:
                    break;
            }

            ViewBag.NameSortParam = nameSortParam.ToString().ToLower();
            ViewBag.DateSortParam = dateSortParam.ToString().ToLower();
            ViewBag.QueryParam = string.IsNullOrEmpty(q) ? "" : q;
            ViewBag.NameSortSuffix = nameSortSuffix;
            ViewBag.DateSortSuffix = dateSortSuffix;
            ViewBag.IsArchive = archive.GetValueOrDefault();

            ViewBag.CurrentSortParam = stype.ToString().ToLower();
            ViewBag.CurrentPage = curPage;

            return View(students.ToPagedList(curPage, 3));
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
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
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            Student? student = null;
            try
            {
                student = _context.Students.Find(id);
                if (student == null) return NotFound();
                bool updateRes = await TryUpdateModelAsync<Student>(
                    student,
                    "",
                    s => s.LastName, s => s.FirstMidName, s => s.EnrollmentDate
                );
                if (!updateRes) return Forbid();

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException _)
            {
                return NotFound();
            }
            catch (DbUpdateException _)
            {
                ModelState.AddModelError("error", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception _)
            {
                return Forbid();
            }

            if (student != null)
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
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

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
                var del = new Student() { ID = id, Deleted = true };
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
            return _context.Students.Any(e => e.ID == id);
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
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null) return NotFound();

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
                var restored = new Student() { ID = (int)id, Deleted = false };
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
