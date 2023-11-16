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
    public class typeComponentsController : ControllerBase
    {
        private readonly FormsAPIContext _context;

        public typeComponentsController(FormsAPIContext context)
        {
            _context = context;
        }

        // GET: api/typeComponents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeComponent>>> GettypeComponent()
        {
            if (_context.typeComponent == null)
            {
                return NotFound();
            }
            return await _context.typeComponent.ToListAsync();
        }

        // GET: api/typeComponents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeComponent>> GettypeComponent(int id)
        {
            if (_context.typeComponent == null)
            {
                return NotFound();
            }
            var typeComponent = await _context.typeComponent.FindAsync(id);

            if (typeComponent == null)
            {
                return NotFound();
            }

            return typeComponent;
        }

        // PUT: api/typeComponents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttypeComponent(int id, TypeComponent typeComponent)
        {
            if (id != typeComponent.id)
            {
                return BadRequest();
            }

            _context.Entry(typeComponent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!typeComponentExists(id))
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

        // POST: api/typeComponents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TypeComponent>> PosttypeComponent(TypeComponent typeComponent)
        {
            if (_context.typeComponent == null)
            {
                return Problem("Entity set 'FormsAPIContext.typeComponent'  is null.");
            }
            _context.typeComponent.Add(typeComponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettypeComponent", new { typeComponent.id }, typeComponent);
        }

        // DELETE: api/typeComponents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletetypeComponent(int id)
        {
            if (_context.typeComponent == null)
            {
                return NotFound();
            }
            var typeComponent = await _context.typeComponent.FindAsync(id);
            if (typeComponent == null)
            {
                return NotFound();
            }

            _context.typeComponent.Remove(typeComponent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool typeComponentExists(int id)
        {
            return (_context.typeComponent?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
