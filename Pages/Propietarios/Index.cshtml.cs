using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Propietarios
{
    public class IndexModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public IndexModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public IList<Propietario> Propietarios { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Buscar { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? EstadoFiltro { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Propietarios
                .Include(p => p.Mascotas)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Buscar))
            {
                var term = Buscar.Trim().ToLower();
                query = query.Where(p =>
                    p.Nombre.ToLower().Contains(term) ||
                    p.Apellidos.ToLower().Contains(term) ||
                    p.Email.ToLower().Contains(term) ||
                    p.Telefono.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(EstadoFiltro))
            {
                if (EstadoFiltro == "activo")
                {
                    query = query.Where(p => p.Estado);
                }
                else if (EstadoFiltro == "inactivo")
                {
                    query = query.Where(p => !p.Estado);
                }
            }

            Propietarios = await query.OrderBy(p => p.Apellidos).ThenBy(p => p.Nombre).ToListAsync();
        }
    }
}
