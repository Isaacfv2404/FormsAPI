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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace FormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComponentsController : ControllerBase
    {
        private readonly FormsAPIContext _context;

        public ComponentsController(FormsAPIContext context)
        {
            _context = context;
        }

        // GET: api/Components
        [HttpGet]
        public async Task<ActionResult<string>> GetComponent()
        {
            var components = await _context.Component.Include(c => c.typeComponent).ToListAsync();

            if (components == null)
            {
                return NotFound();
            }

            // Crea una lista para almacenar los fragmentos HTML de componentes
            var componentHtmlFragments = new List<string>();

            foreach (var component in components)
            {
                var typeComponent = component.typeComponent != null ? component.typeComponent.nameComponent : "Sin typeComponent";

                // Genera el fragmento HTML para cada componente y escapa los valores para seguridad
                var htmlFragment = $"<label for=\"{HtmlEncode(component.nameDescription)}\">\"{HtmlEncode(component.nameDescription)}\":</label>\r\n";
                 htmlFragment += $"<input type=\"{HtmlEncode(typeComponent)}\" id=\"{HtmlEncode(component.componentNameId)}\" name=\"{HtmlEncode(component.nameDescription)}\" key=\"{HtmlEncode(component.componentNameId)}\">";

                componentHtmlFragments.Add(htmlFragment);
            }

            // Convierte la lista de fragmentos HTML en una cadena completa
            var html = string.Join("", componentHtmlFragments);

            return Content(html, "text/html");
        }

        // MÃ©todo para escapar caracteres HTML especiales
        private string HtmlEncode(string value)
        {
            return System.Web.HttpUtility.HtmlEncode(value);
        }

        // GET: api/Components/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Component>> GetComponent(int id)
        {
            var components = await _context.Component
           .Include(c => c.typeComponent)
           .Where(c => c.formId == id)
           .ToListAsync();

            if (components == null || components.Count == 0)
            {
                return NotFound();
            }

            var componentHtmlFragments = new List<string>();

            foreach (var component in components)
            {
                var typeComponent = component.typeComponent != null ? component.typeComponent.nameComponent : "Sin typeComponent";
                var htmlFragment = "";

                //el typeComponentId es 7 (label)
                if (component.typeComponentId == 7)
                {
                    htmlFragment += $"<label for=\"{HtmlEncode(component.nameDescription)}\">\"{HtmlEncode(component.nameDescription)}\":</label>\r\n";
                }

                switch (component.typeComponentId)
                {
                    case 1: // text
                        htmlFragment += $"<input type=\"text\" id=\"{HtmlEncode(component.componentNameId)}\" name=\"{HtmlEncode(component.nameDescription)}\" key=\"{HtmlEncode(component.componentNameId)}\">";
                        break;
                    case 2: // button
                        htmlFragment += $"<button type=\"button\" id=\"{HtmlEncode(component.componentNameId)}\" name=\"{HtmlEncode(component.nameDescription)}\" key=\"{HtmlEncode(component.componentNameId)}\">{HtmlEncode(component.nameDescription)}</button>";
                        break;
                    case 3: // radio
                        htmlFragment += $"<input type=\"radio\" id=\"{HtmlEncode(component.componentNameId)}\" name=\"{HtmlEncode(component.nameDescription)}\" key=\"{HtmlEncode(component.componentNameId)}\">";
                        break;
                    case 4: // checkbox
                        htmlFragment += $"<input type=\"checkbox\" id=\"{HtmlEncode(component.componentNameId)}\" name=\"{HtmlEncode(component.nameDescription)}\" key=\"{HtmlEncode(component.componentNameId)}\">";
                        break;
                    case 5: // select
                        htmlFragment += $"<select id=\"{HtmlEncode(component.componentNameId)}\" name=\"{HtmlEncode(component.nameDescription)}\" key=\"{HtmlEncode(component.componentNameId)}\">";

                        // Agregar opciones al select (puedes modificar esta lógica según tus necesidades)
                        htmlFragment += "<option value=\"opcion1\">Opción 1</option>";
                        htmlFragment += "<option value=\"opcion2\">Opción 2</option>";
                        htmlFragment += "</select>";
                        break;
                    case 6: // textarea
                        htmlFragment += $"<textarea id=\"{HtmlEncode(component.componentNameId)}\" name=\"{HtmlEncode(component.nameDescription)}\" key=\"{HtmlEncode(component.componentNameId)}\"></textarea>";
                        break;
                    case 7: // label
                            // No es necesario agregar nada adicional para el caso de label, ya que ya se incluye en el fragmento HTML.
                        break;
                    default:
                        htmlFragment += $"Tipo de componente no manejado para typeComponentId {component.typeComponentId}";
                        break;
                }

                componentHtmlFragments.Add(htmlFragment);
            }

            var html = string.Join("", componentHtmlFragments);

            return Content(html, "text/html");

        }

        // PUT: api/Components/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComponent(int id, Component component)
        {
            if (id != component.id)
            {
                return BadRequest();
            }

            _context.Entry(component).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentExists(id))
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

        // POST: api/Components
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Component>> PostComponent(Component component)
        {
            if (_context.Component == null)
            {
                return Problem("Entity set 'FormsAPIContext.Component'  is null.");
            }
            _context.Component.Add(component);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComponent", new { component.id }, component);
        }

        // DELETE: api/Components/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComponent(int id)
        {
            if (_context.Component == null)
            {
                return NotFound();
            }
            var component = await _context.Component.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }

            _context.Component.Remove(component);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComponentExists(int id)
        {
            return (_context.Component?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
