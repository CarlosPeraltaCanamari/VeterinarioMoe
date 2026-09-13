using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Mascotas
{
    public class DeleteModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public DeleteModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mascota Mascota { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(m => m.Propietario)
                .Include(m => m.Citas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mascota == null)
            {
                return NotFound();
            }

            Mascota = mascota;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(m => m.Citas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mascota != null)
            {
                if (mascota.Citas.Any())
                {
                    ErrorMessage = "No se puede eliminar la ficha del paciente porque tiene historial de citas o diagnósticos registrados. Se sugiere marcar su estado como 'Inactivo' para preservar el historial clínico legal.";
                    Mascota = mascota;
                    return Page();
                }

                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Mascota eliminada del sistema.";
            return RedirectToPage("./Index");
        }
    }
}
