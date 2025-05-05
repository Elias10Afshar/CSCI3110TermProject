using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSCI3110TermProject.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace CSCI3110TermProject.Web.Controllers
{
    /// <summary>
    /// MVC controller for managing JobApplication views (List, Details, Create, Edit, Delete).
    /// Requires authenticated users.
    /// </summary>
    [Authorize]
    public class JobApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Injects the EF Core context for accessing JobApplications and Tags.
        public JobApplicationsController(ApplicationDbContext context)
            => _context = context;

        /// <summary>
        /// GET: /JobApplications
        /// Display a list of job applications, optionally filtered by search term.
        /// </summary>
        public async Task<IActionResult> Index([FromQuery] string? searchTerm)
        {
            // Start building the query from all applications
            var query = _context.JobApplications.AsQueryable();

            // If the user supplied a term, filter it
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(j =>
                    j.CompanyName.Contains(searchTerm!) ||
                    j.Position.Contains(searchTerm!));
            }

            // Keep the term around so the view can redisplay it
            ViewData["CurrentFilter"] = searchTerm;

            var list = await query.ToListAsync();
            return View(list);
        }

        /// <summary>
        /// GET: /JobApplications/Details/5
        /// Show details for one application, including its Tags.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var app = await _context.JobApplications
                // bring in the join entities
                .Include(j => j.JobApplicationTags)
                // then include the Tag itself
                    .ThenInclude(jt => jt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (app == null) return NotFound();
            return View(app);
        }

        /// <summary>
        /// GET: /JobApplications/Create
        /// Render the blank create form with all available Tags.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            ViewData["AllTags"] = await _context.Tags.ToListAsync();
            ViewData["SelectedTagIds"] = new int[0];
            return View();
        }

        /// <summary>
        /// POST: /JobApplications/Create
        /// Handle form submission to create a new application and link selected Tags.
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CompanyName,Position,DateApplied")] JobApplication jobApplication,
            int[] SelectedTagIds)
        {
            if (!ModelState.IsValid)
            {
                ViewData["AllTags"] = await _context.Tags.ToListAsync();
                ViewData["SelectedTagIds"] = SelectedTagIds;
                return View(jobApplication);
            }

            foreach (var tid in SelectedTagIds)
                jobApplication.JobApplicationTags.Add(
                    new JobApplicationTag { TagId = tid });
            // Save new application with its tags
            _context.Add(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /JobApplications/Edit/5
        /// Render edit form, pre-checking existing Tags.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var existing = await _context.JobApplications
                .Include(j => j.JobApplicationTags)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (existing == null) return NotFound();
            // Provide all tags and mark the ones already linked
            ViewData["AllTags"] = await _context.Tags.ToListAsync();
            ViewData["SelectedTagIds"] =
                existing.JobApplicationTags.Select(jt => jt.TagId).ToArray();

            return View(existing);
        }

        /// <summary>
        /// POST: /JobApplications/Edit/5
        /// Handle form submission to update the application and its Tags.
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,CompanyName,Position,DateApplied")] JobApplication jobApplication,
            int[] SelectedTagIds)
        {
            if (id != jobApplication.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["AllTags"] = await _context.Tags.ToListAsync();
                ViewData["SelectedTagIds"] = SelectedTagIds;
                return View(jobApplication);
            }

            var existing = await _context.JobApplications
                .Include(j => j.JobApplicationTags)
                .FirstAsync(j => j.Id == id);

            // remove old links
            _context.RemoveRange(existing.JobApplicationTags);
            // add new
            foreach (var tid in SelectedTagIds)
                existing.JobApplicationTags.Add(
                    new JobApplicationTag { TagId = tid });

            // update fields
            existing.CompanyName = jobApplication.CompanyName;
            existing.Position = jobApplication.Position;
            existing.DateApplied = jobApplication.DateApplied;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /JobApplications/Delete/5
        /// Confirm delete of a single application.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var app = await _context.JobApplications
                .FirstOrDefaultAsync(j => j.Id == id);
            if (app == null) return NotFound();
            return View(app);
        }

        /// <summary>
        /// POST: /JobApplications/Delete/5
        /// Perform the deletion after confirmation.
        /// </summary>
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var app = await _context.JobApplications.FindAsync(id);
            _context.JobApplications.Remove(app);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
