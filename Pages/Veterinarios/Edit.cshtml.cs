using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Veterinarios
{
    public class EditModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public EditModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Veterinario Veterinario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios.FirstOrDefaultAsync(m => m.Id == id);
            if (veterinario == null)
            {
                return NotFound();
            }

            Veterinario = veterinario;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Veterinario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeterinarioExists(Veterinario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = "Datos del veterinario actualizados correctamente.";
            return RedirectToPage("./Index");
        }

        private bool VeterinarioExists(int id)
        {
            return _context.Veterinarios.Any(e => e.Id == id);
        }
    }
}
