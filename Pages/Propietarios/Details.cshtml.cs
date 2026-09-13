using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Propietarios
{
    public class DetailsModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public DetailsModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public Propietario Propietario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propietario = await _context.Propietarios
                .Include(p => p.Mascotas)
                    .ThenInclude(m => m.Citas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (propietario == null)
            {
                return NotFound();
            }

            Propietario = propietario;
            return Page();
        }
    }
}
