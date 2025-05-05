using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSCI3110TermProject.Data;
using Microsoft.AspNetCore.Authorization;

namespace CSCI3110TermProject.Web.Controllers
{
    /// <summary>
    /// API controller to manage JobApplication records.
    /// Supports querying with filtering, sorting, paging, and basic CRUD.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class JobApplicationsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        // Injects the EF Core DbContext for data operations.
        public JobApplicationsApiController(ApplicationDbContext context)
            => _context = context;

        /// <summary>
        /// GET: /api/JobApplicationsApi
        /// Returns a paged, sorted, and optionally filtered list of applications.
        /// Query string supports: searchTerm, page, pageSize, sortBy, sortDir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string? searchTerm,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "DateApplied",
            [FromQuery] string sortDir = "desc")
        {
            // 1) Start with full set, including tags for filtering only
            var query = _context.JobApplications
                                .Include(j => j.JobApplicationTags)
                                  .ThenInclude(jt => jt.Tag)
                                .AsQueryable();

            // 2) Apply filter on company, position or tag name
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(j =>
                    j.CompanyName.Contains(term) ||
                    j.Position.Contains(term) ||
                    j.JobApplicationTags.Any(jt => jt.Tag.Name.Contains(term))
                );
            }

            // 3) Apply sort and apply ordering
            sortDir = sortDir.ToLower() == "asc" ? "asc" : "desc";
            query = (sortBy, sortDir) switch
            {
                ("CompanyName", "asc") => query.OrderBy(j => j.CompanyName),
                ("CompanyName", "desc") => query.OrderByDescending(j => j.CompanyName),
                ("Position", "asc") => query.OrderBy(j => j.Position),
                ("Position", "desc") => query.OrderByDescending(j => j.Position),
                ("DateApplied", "asc") => query.OrderBy(j => j.DateApplied),
                                     _ => query.OrderByDescending(j => j.DateApplied),
            };

            // 4) Count _before_ paging
            var totalCount = await query.CountAsync();

            // 5) Page _and_ project into a simple DTO
            var list = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(j => new {
                id = j.Id,
                companyName = j.CompanyName,
                position = j.Position,
                dateApplied = j.DateApplied,
                tags = j.JobApplicationTags
                      .Select(jt => jt.Tag.Name)
                      .ToList()
            })
            .ToListAsync();


            // 6) Return exactly the shape your JS is expecting
            return Ok(new
            {
                totalCount = totalCount,   // <-- must match the C# var name
                page = page,
                pageSize = pageSize,
                items = list
            });
        }

        /// <summary>
        /// GET: /api/JobApplicationsApi/{id}
        /// Retrieves a single JobApplication by its primary key.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplication>> Get(int id)
        {
            var app = await _context.JobApplications.FindAsync(id);
            if (app == null) return NotFound();
            return app;
        }

        /// <summary>
        /// POST: /api/JobApplicationsApi
        /// Creates a new JobApplication record.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<JobApplication>> Post(JobApplication jobApplication)
        {
            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();
            // Returns 201 with location header pointing to the new resource
            return CreatedAtAction(nameof(Get), new { id = jobApplication.Id }, jobApplication);
        }

        /// <summary>
        /// PUT: /api/JobApplicationsApi/{id}
        /// Updates an existing JobApplication. Must match URL id and body id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, JobApplication jobApplication)
        {
            if (id != jobApplication.Id) return BadRequest();
            _context.Entry(jobApplication).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.JobApplications.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        /// <summary>
        /// DELETE: /api/JobApplicationsApi/{id}
        /// Deletes a JobApplication by its id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var app = await _context.JobApplications.FindAsync(id);
            if (app == null) return NotFound();
            _context.JobApplications.Remove(app);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
