using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Services;

namespace PW3_04.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ArcaMoeDbContext _context;
        private readonly IPasswordHasherService _hasher;

        public LoginModel(ArcaMoeDbContext context, IPasswordHasherService hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El correo electrónico es requerido.")]
            [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
            [Display(Name = "Correo Electrónico")]
            public string Email { get; set; } = "admin@elarcademoe.com";

            [Required(ErrorMessage = "La contraseña es requerida.")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; } = "Admin123!";

            [Display(Name = "Recordar sesión en este equipo")]
            public bool RememberMe { get; set; } = true;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == Input.Email.ToLower());

            if (usuario == null || !_hasher.VerifyPassword(Input.Password, usuario.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Credenciales de acceso incorrectas.");
                return Page();
            }

            if (!usuario.Estado)
            {
                ModelState.AddModelError(string.Empty, "Esta cuenta de usuario se encuentra inactiva. Contacte a soporte.");
                return Page();
            }

            // Actualizar último acceso
            usuario.UltimoAcceso = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Claims del usuario
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Input.RememberMe,
                ExpiresUtc = Input.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(returnUrl);
        }
    }
}
