using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Propietarios
{
    public class DeleteModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public DeleteModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Propietario Propietario { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propietario = await _context.Propietarios
                .Include(p => p.Mascotas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (propietario == null)
            {
                return NotFound();
            }

            Propietario = propietario;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propietario = await _context.Propietarios
                .Include(p => p.Mascotas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (propietario != null)
            {
                if (propietario.Mascotas.Any())
                {
                    ErrorMessage = "No se puede eliminar el propietario porque tiene mascotas registradas. Se recomienda cambiar su estado a 'Inactivo' o reasignar las mascotas.";
                    Propietario = propietario;
                    return Page();
                }

                _context.Propietarios.Remove(propietario);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Propietario eliminado correctamente.";
            return RedirectToPage("./Index");
        }
    }
}
