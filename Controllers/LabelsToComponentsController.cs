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
    public class LabelsToComponentsController : ControllerBase
    {
        private readonly FormsAPIContext _context;

        public LabelsToComponentsController(FormsAPIContext context)
        {
            _context = context;
        }

        // GET: api/LabelsToComponents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LabelsToComponent>>> GetLabelsToComponent()
        {
            if (_context.LabelsToComponent == null)
            {
                return NotFound();
            }
            return await _context.LabelsToComponent.ToListAsync();
        }

        // GET: api/LabelsToComponents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LabelsToComponent>> GetLabelsToComponent(int id)
        {
            if (_context.LabelsToComponent == null)
            {
                return NotFound();
            }
            var labelsToComponent = await _context.LabelsToComponent.FindAsync(id);

            if (labelsToComponent == null)
            {
                return NotFound();
            }

            return labelsToComponent;
        }

        // PUT: api/LabelsToComponents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLabelsToComponent(int id, LabelsToComponent labelsToComponent)
        {
            if (id != labelsToComponent.id)
            {
                return BadRequest();
            }

            _context.Entry(labelsToComponent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabelsToComponentExists(id))
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

        // POST: api/LabelsToComponents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LabelsToComponent>> PostLabelsToComponent(LabelsToComponent labelsToComponent)
        {
            if (_context.LabelsToComponent == null)
            {
                return Problem("Entity set 'FormsAPIContext.LabelsToComponent'  is null.");
            }
            _context.LabelsToComponent.Add(labelsToComponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLabelsToComponent", new { labelsToComponent.id }, labelsToComponent);
        }

        // DELETE: api/LabelsToComponents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabelsToComponent(int id)
        {
            if (_context.LabelsToComponent == null)
            {
                return NotFound();
            }
            var labelsToComponent = await _context.LabelsToComponent.FindAsync(id);
            if (labelsToComponent == null)
            {
                return NotFound();
            }

            _context.LabelsToComponent.Remove(labelsToComponent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LabelsToComponentExists(int id)
        {
            return (_context.LabelsToComponent?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
