using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Mascotas
{
    public class EditModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public EditModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mascota Mascota { get; set; } = default!;

        public SelectList PropietariosList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
            if (mascota == null)
            {
                return NotFound();
            }

            Mascota = mascota;
            await CargarPropietariosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarPropietariosAsync();
                return Page();
            }

            _context.Attach(Mascota).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MascotaExists(Mascota.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = "Datos de la mascota actualizados correctamente.";
            return RedirectToPage("./Index");
        }

        private bool MascotaExists(int id)
        {
            return _context.Mascotas.Any(e => e.Id == id);
        }

        private async Task CargarPropietariosAsync()
        {
            var propietarios = await _context.Propietarios
                .Where(p => p.Estado)
                .OrderBy(p => p.Apellidos)
                .ThenBy(p => p.Nombre)
                .Select(p => new
                {
                    p.Id,
                    Descripcion = $"{p.Apellidos}, {p.Nombre} (Tel: {p.Telefono})"
                })
                .ToListAsync();

            PropietariosList = new SelectList(propietarios, "Id", "Descripcion");
        }
    }
}
