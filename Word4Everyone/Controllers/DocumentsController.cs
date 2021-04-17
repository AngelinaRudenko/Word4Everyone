using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public DocumentsController(AppDbContext context)
        {
            _context = context;
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
            var document = await _context.Documents.FindAsync(id);
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (document == null || !document.UserId.Equals(userId)) 
                return NotFound("Документа не существует");

            return document;
        }

        // PUT: api/Documents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(int id, Document document)
        {
            if (document.UserId == null)
                return BadRequest("Неверный запрос");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (id != document.Id || !DocumentExists(id) || !document.UserId.Equals(userId)) 
                return BadRequest("Документа не существует");

            document.ChangeDateModified();
            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Возникла ошибка. Попробуйте еще раз");
            }

            return Ok("Изменения сохранены");
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

            return Ok("Сохранено");
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);

            if (document.UserId == null)
                return BadRequest("Неверный запрос");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (document == null || !document.UserId.Equals(userId)) 
                return NotFound("Документа не существует");

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return Ok("Удалено");
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}