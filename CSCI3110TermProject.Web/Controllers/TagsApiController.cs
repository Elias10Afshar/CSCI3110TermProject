using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSCI3110TermProject.Data;

namespace CSCI3110TermProject.Web.Controllers
{
    /// <summary>
    /// API controller for managing Tag entities (CRUD operations).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TagsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor: injects the EF Core DbContext.
        public TagsApiController(ApplicationDbContext context)
            => _context = context;

        /// <summary>
        /// GET: /api/TagsApi
        /// Returns all tags in the system.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> Get()
            // Fetches and returns the full list of Tags
            => await _context.Tags.ToListAsync();

        /// <summary>
        /// GET: /api/TagsApi/{id}
        /// Retrieves a specific tag by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> Get(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();
            return tag;
        }

        /// <summary>
        /// POST: /api/TagsApi
        /// Creates a new tag.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Tag>> Post(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = tag.Id }, tag);
        }

        /// <summary>
        /// PUT: /api/TagsApi/{id}
        /// Updates an existing tag. 
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Tag tag)
        {
            if (id != tag.Id) return BadRequest();
            _context.Entry(tag).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// DELETE: /api/TagsApi/{id}
        /// Deletes a tag by its ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the tag; return 404 if missing
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();
            // Remove and persist
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            // 204 No Content on successful deletion
            return NoContent();
        }
    }
}

