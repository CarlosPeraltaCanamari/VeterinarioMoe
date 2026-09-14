using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PW3_04.Data;
using PW3_04.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SupabaseConnection") ?? string.Empty;

if (string.IsNullOrWhiteSpace(connectionString) 
    || connectionString.Contains("yourprojectid") 
    || connectionString.Contains("[YOUR-PASSWORD]") 
    || connectionString.Contains("[TU_CONTRASEÑA]"))
{
    builder.Services.AddDbContext<ArcaMoeDbContext>(options =>
        options.UseInMemoryDatabase("ArcaDeMoeLocalDb"));
}
else
{
    builder.Services.AddDbContext<ArcaMoeDbContext>(options =>
        options.UseNpgsql(connectionString));
}

builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

builder.Services.AddAuthorization();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Citas");
    options.Conventions.AuthorizeFolder("/Mascotas");
    options.Conventions.AuthorizeFolder("/Propietarios");
    options.Conventions.AuthorizeFolder("/Veterinarios");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/AccessDenied");
    options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/Privacy");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ArcaMoeDbContext>();
        var hasher = services.GetRequiredService<IPasswordHasherService>();
        
        if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            context.Database.EnsureCreated();
        }
        
        DbInitializer.Initialize(context, hasher);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar la base de datos.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

