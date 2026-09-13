using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Veterinarios
{
    public class CreateModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public CreateModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Veterinario Veterinario { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Veterinarios.Add(Veterinario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Veterinario registrado exitosamente en el equipo médico.";
            return RedirectToPage("./Index");
        }
    }
}
