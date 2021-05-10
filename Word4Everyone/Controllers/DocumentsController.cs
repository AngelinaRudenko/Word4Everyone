using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Word4Everyone.Data;
using Word4Everyone.Model;

namespace Word4Everyone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public DocumentsController(AppDbContext context, 
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _context.Documents
                .Where(x => x.UserId == userId).ToListAsync();
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            Document document = await _context.Documents.FindAsync(id);
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (document == null || !document.UserId.Equals(userId)) 
                return NotFound(_sharedLocalizer["NoDocumentFound"].Value);

            return document;
        }

        // PUT: api/Documents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(int id, Document document)
        {
            if (document.UserId == null)
                return BadRequest(_sharedLocalizer["BadRequest"].Value);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (id != document.Id || !DocumentExists(id) || !document.UserId.Equals(userId)) 
                return BadRequest(_sharedLocalizer["NoDocumentFound"].Value);

            document.ChangeDateModified();
            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(_sharedLocalizer["BadRequest"].Value);
            }

            return Ok(_sharedLocalizer["Saved"].Value);
        }

        // POST: api/Documents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Document>> PostDocument(Document document)
        {
            document.ChangeDateCreated();
            document.ChangeDateModified();
            document.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return Ok(_sharedLocalizer["Saved"].Value);
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);

            if (document.UserId == null)
                return BadRequest(_sharedLocalizer["BadRequest"].Value);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!document.UserId.Equals(userId)) 
                return NotFound(_sharedLocalizer["NoDocumentFound"].Value);

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return Ok(_sharedLocalizer["Deleted"].Value);
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}