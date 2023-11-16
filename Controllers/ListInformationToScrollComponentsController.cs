using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormsAPI.Data;
using FormsAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace FormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListInformationToScrollComponentsController : ControllerBase
    {
        private readonly FormsAPIContext _context;

        public ListInformationToScrollComponentsController(FormsAPIContext context)
        {
            _context = context;
        }

        // GET: api/ListInformationToScrollComponents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListInformationToScrollComponent>>> GetListInformationToScrollComponent()
        {
            if (_context.ListInformationToScrollComponent == null)
            {
                return NotFound();
            }
            return await _context.ListInformationToScrollComponent.ToListAsync();
        }

        // GET: api/ListInformationToScrollComponents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ListInformationToScrollComponent>> GetListInformationToScrollComponent(int id)
        {
            if (_context.ListInformationToScrollComponent == null)
            {
                return NotFound();
            }
            var listInformationToScrollComponent = await _context.ListInformationToScrollComponent.FindAsync(id);

            if (listInformationToScrollComponent == null)
            {
                return NotFound();
            }

            return listInformationToScrollComponent;
        }

        // PUT: api/ListInformationToScrollComponents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListInformationToScrollComponent(int id, ListInformationToScrollComponent listInformationToScrollComponent)
        {
            if (id != listInformationToScrollComponent.id)
            {
                return BadRequest();
            }

            _context.Entry(listInformationToScrollComponent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListInformationToScrollComponentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ListInformationToScrollComponents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ListInformationToScrollComponent>> PostListInformationToScrollComponent(ListInformationToScrollComponent listInformationToScrollComponent)
        {
            if (_context.ListInformationToScrollComponent == null)
            {
                return Problem("Entity set 'FormsAPIContext.ListInformationToScrollComponent'  is null.");
            }
            _context.ListInformationToScrollComponent.Add(listInformationToScrollComponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListInformationToScrollComponent", new { listInformationToScrollComponent.id }, listInformationToScrollComponent);
        }

        // DELETE: api/ListInformationToScrollComponents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListInformationToScrollComponent(int id)
        {
            if (_context.ListInformationToScrollComponent == null)
            {
                return NotFound();
            }
            var listInformationToScrollComponent = await _context.ListInformationToScrollComponent.FindAsync(id);
            if (listInformationToScrollComponent == null)
            {
                return NotFound();
            }

            _context.ListInformationToScrollComponent.Remove(listInformationToScrollComponent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ListInformationToScrollComponentExists(int id)
        {
            return (_context.ListInformationToScrollComponent?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
