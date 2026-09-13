using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Citas
{
    public class EditModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public EditModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cita Cita { get; set; } = default!;

        public SelectList MascotasList { get; set; } = default!;
        public SelectList VeterinariosList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas.FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            Cita = cita;
            await CargarListasAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarListasAsync();
                return Page();
            }

            _context.Attach(Cita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CitaExists(Cita.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = "Cita actualizada correctamente.";
            return RedirectToPage("./Index");
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.Id == id);
        }

        private async Task CargarListasAsync()
        {
            var mascotas = await _context.Mascotas
                .Include(m => m.Propietario)
                .OrderBy(m => m.Nombre)
                .Select(m => new
                {
                    m.Id,
                    Descripcion = $"{m.Nombre} ({m.Especie} - {m.Raza}) - Tutor: {(m.Propietario != null ? m.Propietario.NombreCompleto : "Sin tutor")}"
                })
                .ToListAsync();

            var veterinarios = await _context.Veterinarios
                .OrderBy(v => v.Apellidos)
                .Select(v => new
                {
                    v.Id,
                    Descripcion = $"Dr(a). {v.Nombre} {v.Apellidos} - {v.Especialidad}"
                })
                .ToListAsync();

            MascotasList = new SelectList(mascotas, "Id", "Descripcion");
            VeterinariosList = new SelectList(veterinarios, "Id", "Descripcion");
        }
    }
}
