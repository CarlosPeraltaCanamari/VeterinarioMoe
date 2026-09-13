using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PW3_04.Data;
using PW3_04.Models;

namespace PW3_04.Pages.Propietarios
{
    public class CreateModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;

        public CreateModel(ArcaMoeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Propietario Propietario { get; set; } = new();

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

            _context.Propietarios.Add(Propietario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Propietario registrado exitosamente.";
            return RedirectToPage("./Index");
        }
    }
}
