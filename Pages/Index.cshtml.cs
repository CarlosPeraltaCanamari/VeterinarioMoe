using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public IndexModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        public int TotalCitasHoy { get; set; }
        public int TotalCitasPendientes { get; set; }
        public int TotalMascotasActivas { get; set; }
        public int TotalVeterinariosActivos { get; set; }
        public int TotalPropietariosActivos { get; set; }

        public IList<Cita> ProximasCitas { get; set; } = new List<Cita>();
        public IList<Cita> CitasHoy { get; set; } = new List<Cita>();

        public async Task OnGetAsync()
        {
            var hoyInicio = DateTime.Today;
            var hoyFin = DateTime.Today.AddDays(1).AddTicks(-1);

            TotalCitasHoy = await _context.Citas
                .CountAsync(c => c.FechaHora >= hoyInicio && c.FechaHora <= hoyFin);

            TotalCitasPendientes = await _context.Citas
                .CountAsync(c => c.Estado == EstadosCita.Pendiente);

            TotalMascotasActivas = await _context.Mascotas
                .CountAsync(m => m.Estado);

            TotalVeterinariosActivos = await _context.Veterinarios
                .CountAsync(v => v.Estado);

            TotalPropietariosActivos = await _context.Propietarios
                .CountAsync(p => p.Estado);

            CitasHoy = await _context.Citas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m!.Propietario)
                .Include(c => c.Veterinario)
                .Where(c => c.FechaHora >= hoyInicio && c.FechaHora <= hoyFin)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();

            ProximasCitas = await _context.Citas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m!.Propietario)
                .Include(c => c.Veterinario)
                .Where(c => c.FechaHora >= DateTime.Now && c.Estado == EstadosCita.Pendiente)
                .OrderBy(c => c.FechaHora)
                .Take(6)
                .ToListAsync();
        }
    }
}
