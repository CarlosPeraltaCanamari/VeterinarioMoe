using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Citas
{
    public class CreateModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public CreateModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cita Cita { get; set; } = new();

        public SelectList MascotasList { get; set; } = default!;
        public SelectList VeterinariosList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? mascotaId)
        {
            await CargarListasAsync();

            Cita.FechaHora = DateTime.Today.AddDays(1).AddHours(10); // Sugerir mañana a las 10:00

            if (mascotaId.HasValue)
            {
                Cita.MascotaId = mascotaId.Value;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarListasAsync();
                return Page();
            }

            // Validar que la mascota y el veterinario existan
            var mascotaExiste = await _context.Mascotas.AnyAsync(m => m.Id == Cita.MascotaId);
            var vetExiste = await _context.Veterinarios.AnyAsync(v => v.Id == Cita.VeterinarioId);

            if (!mascotaExiste || !vetExiste)
            {
                ModelState.AddModelError(string.Empty, "La mascota o el veterinario seleccionado no son válidos.");
                await CargarListasAsync();
                return Page();
            }

            Cita.FechaCreacion = DateTime.UtcNow;
            _context.Citas.Add(Cita);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cita agendada exitosamente.";
            return RedirectToPage("./Index");
        }

        private async Task CargarListasAsync()
        {
            var mascotas = await _context.Mascotas
                .Include(m => m.Propietario)
                .Where(m => m.Estado)
                .OrderBy(m => m.Nombre)
                .Select(m => new
                {
                    m.Id,
                    Descripcion = $"{m.Nombre} ({m.Especie} - {m.Raza}) - Tutor: {(m.Propietario != null ? m.Propietario.NombreCompleto : "Sin tutor")}"
                })
                .ToListAsync();

            var veterinarios = await _context.Veterinarios
                .Where(v => v.Estado)
                .OrderBy(v => v.Apellidos)
                .Select(v => new
                {
                    v.Id,
                    Descripcion = $"Dr(a). {v.Nombre} {v.Apellidos} - {v.Especialidad}"
                })
                .ToListAsync();

            MascotasList = new SelectList(mascotas, "Id", "Descripcion");
            VeterinariosList = new SelectList(veterinarios, "Id", "Descripcion");
        }
    }
}
