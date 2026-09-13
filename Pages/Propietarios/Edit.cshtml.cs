using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Propietarios
{
    public class EditModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public EditModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Propietario Propietario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propietario = await _context.Propietarios.FirstOrDefaultAsync(m => m.Id == id);
            if (propietario == null)
            {
                return NotFound();
            }
            Propietario = propietario;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Propietario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropietarioExists(Propietario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = "Datos del propietario actualizados correctamente.";
            return RedirectToPage("./Index");
        }

        private bool PropietarioExists(int id)
        {
            return _context.Propietarios.Any(e => e.Id == id);
        }
    }
}
