using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Veterinarios
{
    public class DetailsModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public DetailsModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public Veterinario Veterinario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios
                .Include(v => v.Citas)
                    .ThenInclude(c => c.Mascota)
                        .ThenInclude(m => m!.Propietario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (veterinario == null)
            {
                return NotFound();
            }

            Veterinario = veterinario;
            return Page();
        }
    }
}
