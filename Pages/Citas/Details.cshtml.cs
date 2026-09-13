using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Citas
{
    public class DetailsModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public DetailsModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public Cita Cita { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m!.Propietario)
                .Include(c => c.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            Cita = cita;
            return Page();
        }
    }
}
