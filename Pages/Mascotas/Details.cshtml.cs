using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Mascotas
{
    public class DetailsModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public DetailsModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public Mascota Mascota { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(m => m.Propietario)
                .Include(m => m.Citas)
                    .ThenInclude(c => c.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mascota == null)
            {
                return NotFound();
            }

            Mascota = mascota;
            return Page();
        }
    }
}
