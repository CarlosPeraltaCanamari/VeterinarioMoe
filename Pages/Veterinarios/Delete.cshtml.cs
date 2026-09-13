using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Veterinarios
{
    public class DeleteModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public DeleteModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Veterinario Veterinario { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios
                .Include(v => v.Citas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (veterinario == null)
            {
                return NotFound();
            }

            Veterinario = veterinario;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios
                .Include(v => v.Citas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (veterinario != null)
            {
                if (veterinario.Citas.Any())
                {
                    ErrorMessage = "No es posible eliminar al veterinario porque tiene citas o consultas clínicas registradas a su nombre. Cambie su estado a 'Inactivo' para retirar su disponibilidad.";
                    Veterinario = veterinario;
                    return Page();
                }

                _context.Veterinarios.Remove(veterinario);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Veterinario eliminado del equipo médico.";
            return RedirectToPage("./Index");
        }
    }
}
