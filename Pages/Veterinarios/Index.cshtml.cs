using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Veterinarios
{
    public class IndexModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public IndexModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public IList<Veterinario> Veterinarios { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Buscar { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? EstadoFiltro { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Veterinarios
                .Include(v => v.Citas)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Buscar))
            {
                var term = Buscar.Trim().ToLower();
                query = query.Where(v =>
                    v.Nombre.ToLower().Contains(term) ||
                    v.Apellidos.ToLower().Contains(term) ||
                    v.Especialidad.ToLower().Contains(term) ||
                    v.Telefono.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(EstadoFiltro))
            {
                if (EstadoFiltro == "activo")
                {
                    query = query.Where(v => v.Estado);
                }
                else if (EstadoFiltro == "inactivo")
                {
                    query = query.Where(v => !v.Estado);
                }
            }

            Veterinarios = await query.OrderBy(v => v.Apellidos).ThenBy(v => v.Nombre).ToListAsync();
        }
    }
}
