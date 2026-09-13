using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Citas
{
    public class IndexModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public IndexModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public IList<Cita> Citas { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? EstadoFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FechaFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Buscar { get; set; }

        public int TotalPendientes { get; set; }
        public int TotalHoy { get; set; }

        public async Task OnGetAsync()
        {
            var hoyInicio = DateTime.Today;
            var hoyFin = DateTime.Today.AddDays(1).AddTicks(-1);

            TotalPendientes = await _context.Citas.CountAsync(c => c.Estado == EstadosCita.Pendiente);
            TotalHoy = await _context.Citas.CountAsync(c => c.FechaHora >= hoyInicio && c.FechaHora <= hoyFin);

            var query = _context.Citas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m!.Propietario)
                .Include(c => c.Veterinario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Buscar))
            {
                var term = Buscar.Trim().ToLower();
                query = query.Where(c =>
                    c.Motivo.ToLower().Contains(term) ||
                    (c.Diagnostico != null && c.Diagnostico.ToLower().Contains(term)) ||
                    (c.Mascota != null && c.Mascota.Nombre.ToLower().Contains(term)) ||
                    (c.Mascota != null && c.Mascota.Propietario != null && (c.Mascota.Propietario.Nombre.ToLower().Contains(term) || c.Mascota.Propietario.Apellidos.ToLower().Contains(term))) ||
                    (c.Veterinario != null && (c.Veterinario.Nombre.ToLower().Contains(term) || c.Veterinario.Apellidos.ToLower().Contains(term))));
            }

            if (!string.IsNullOrWhiteSpace(EstadoFiltro))
            {
                query = query.Where(c => c.Estado == EstadoFiltro);
            }

            if (!string.IsNullOrWhiteSpace(FechaFiltro))
            {
                if (FechaFiltro == "hoy")
                {
                    query = query.Where(c => c.FechaHora >= hoyInicio && c.FechaHora <= hoyFin);
                }
                else if (FechaFiltro == "proximas")
                {
                    query = query.Where(c => c.FechaHora >= DateTime.Now);
                }
                else if (FechaFiltro == "pasadas")
                {
                    query = query.Where(c => c.FechaHora < DateTime.Now);
                }
            }

            Citas = await query.OrderByDescending(c => c.FechaHora).ToListAsync();
        }
    }
}
