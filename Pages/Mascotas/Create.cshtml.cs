using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Mascotas
{
    public class CreateModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public CreateModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mascota Mascota { get; set; } = new();

        public SelectList PropietariosList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? propietarioId)
        {
            await CargarPropietariosAsync();

            if (propietarioId.HasValue)
            {
                Mascota.PropietarioId = propietarioId.Value;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarPropietariosAsync();
                return Page();
            }

            _context.Mascotas.Add(Mascota);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Mascota registrada exitosamente en el sistema.";
            return RedirectToPage("./Index");
        }

        private async Task CargarPropietariosAsync()
        {
            var propietarios = await _context.Propietarios
                .Where(p => p.Estado)
                .OrderBy(p => p.Apellidos)
                .ThenBy(p => p.Nombre)
                .Select(p => new
                {
                    p.Id,
                    Descripcion = $"{p.Apellidos}, {p.Nombre} (Tel: {p.Telefono})"
                })
                .ToListAsync();

            PropietariosList = new SelectList(propietarios, "Id", "Descripcion");
        }
    }
}
