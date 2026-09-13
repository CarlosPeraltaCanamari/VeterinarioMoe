using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Citas
{
    public class AtenderModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public AtenderModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public Cita Cita { get; set; } = default!;

        public class InputModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "El diagnóstico clínico es obligatorio al finalizar la consulta.")]
            [MinLength(5, ErrorMessage = "El diagnóstico debe contener al menos 5 caracteres.")]
            [Display(Name = "Diagnóstico Clínico")]
            public string Diagnostico { get; set; } = string.Empty;

            [Display(Name = "Tratamiento e Indicaciones Médicas")]
            public string? Tratamiento { get; set; }

            [Display(Name = "Actualizar peso del paciente en esta consulta (Kg)")]
            [Range(0.01, 200.0, ErrorMessage = "El peso debe estar entre 0.01 y 200 kg.")]
            public decimal? PesoActualizado { get; set; }
        }

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
            Input.Id = cita.Id;
            Input.Diagnostico = cita.Diagnostico ?? string.Empty;
            Input.Tratamiento = cita.Tratamiento ?? string.Empty;
            Input.PesoActualizado = cita.Mascota?.PesoKg;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cita = await _context.Citas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m!.Propietario)
                .Include(c => c.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == Input.Id);

            if (cita == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                Cita = cita;
                return Page();
            }

            // Actualizar diagnóstico y marcar como completada
            cita.Diagnostico = Input.Diagnostico;
            cita.Tratamiento = Input.Tratamiento;
            cita.Estado = EstadosCita.Completada;

            // Actualizar peso si se especificó
            if (Input.PesoActualizado.HasValue && cita.Mascota != null)
            {
                cita.Mascota.PesoKg = Input.PesoActualizado.Value;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Consulta de {cita.Mascota?.Nombre} finalizada y diagnóstico registrado con éxito.";
            return RedirectToPage("./Details", new { id = cita.Id });
        }
    }
}
