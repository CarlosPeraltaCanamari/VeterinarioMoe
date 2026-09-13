using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Mascotas
{
    public class IndexModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public IndexModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public IList<Mascota> Mascotas { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Buscar { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? EspecieFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? EstadoFiltro { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Mascotas
                .Include(m => m.Propietario)
                .Include(m => m.Citas)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Buscar))
            {
                var term = Buscar.Trim().ToLower();
                query = query.Where(m =>
                    m.Nombre.ToLower().Contains(term) ||
                    m.Raza.ToLower().Contains(term) ||
                    (m.Propietario != null && (m.Propietario.Nombre.ToLower().Contains(term) || m.Propietario.Apellidos.ToLower().Contains(term))));
            }

            if (!string.IsNullOrWhiteSpace(EspecieFiltro))
            {
                query = query.Where(m => m.Especie.ToLower() == EspecieFiltro.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(EstadoFiltro))
            {
                if (EstadoFiltro == "activo")
                {
                    query = query.Where(m => m.Estado);
                }
                else if (EstadoFiltro == "inactivo")
                {
                    query = query.Where(m => !m.Estado);
                }
            }

            Mascotas = await query.OrderBy(m => m.Nombre).ToListAsync();
        }
    }
}
