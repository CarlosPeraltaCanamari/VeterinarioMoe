using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PW3_04.Models;
using PW3_04.Services;

namespace PW3_04.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ArcaMoeDbContext context, IPasswordHasherService hasher)
        {
            try
            {
                // Verifica si la base de datos existe o puede consultar
                if (!context.Database.CanConnect())
                {
                    return;
                }

                // Usuarios iniciales
                if (!context.Usuarios.Any())
                {
                    var admin = new Usuario
                    {
                        NombreCompleto = "Administrador del Sistema",
                        Email = "admin@elarcademoe.com",
                        PasswordHash = hasher.HashPassword("Admin123!"),
                        Rol = RolesUsuario.Administrador,
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    var vetUser = new Usuario
                    {
                        NombreCompleto = "Dr. Moe Szyslak",
                        Email = "moe@elarcademoe.com",
                        PasswordHash = hasher.HashPassword("Vet123!"),
                        Rol = RolesUsuario.Veterinario,
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    var recepUser = new Usuario
                    {
                        NombreCompleto = "Ana Recepción",
                        Email = "recepcion@elarcademoe.com",
                        PasswordHash = hasher.HashPassword("Recepcion123!"),
                        Rol = RolesUsuario.Recepcionista,
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    context.Usuarios.AddRange(admin, vetUser, recepUser);
                    context.SaveChanges();
                }

                // Veterinarios iniciales
                if (!context.Veterinarios.Any())
                {
                    var v1 = new Veterinario
                    {
                        Nombre = "Moe",
                        Apellidos = "Szyslak",
                        Especialidad = "Cirugía y Traumatología",
                        Telefono = "+591 71234567",
                        Email = "moe@elarcademoe.com",
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    var v2 = new Veterinario
                    {
                        Nombre = "Lisa",
                        Apellidos = "Simpson",
                        Especialidad = "Medicina Felina y Exóticos",
                        Telefono = "+591 72345678",
                        Email = "lisa.simpson@elarcademoe.com",
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    var v3 = new Veterinario
                    {
                        Nombre = "Carlos",
                        Apellidos = "Mendoza",
                        Especialidad = "Medicina General y Preventiva",
                        Telefono = "+591 73456789",
                        Email = "cmendoza@elarcademoe.com",
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    context.Veterinarios.AddRange(v1, v2, v3);
                    context.SaveChanges();
                }

                // Propietarios y Mascotas
                if (!context.Propietarios.Any())
                {
                    var p1 = new Propietario
                    {
                        Nombre = "Homero",
                        Apellidos = "Simpson",
                        Telefono = "+591 70112233",
                        Email = "homero@springfield.com",
                        Direccion = "Av. Siempre Viva 742",
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    var p2 = new Propietario
                    {
                        Nombre = "Ned",
                        Apellidos = "Flanders",
                        Telefono = "+591 70223344",
                        Email = "ned@flanders.com",
                        Direccion = "Av. Siempre Viva 744",
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    var p3 = new Propietario
                    {
                        Nombre = "Patricia",
                        Apellidos = "Bouvier",
                        Telefono = "+591 70334455",
                        Email = "patricia.b@springfield.com",
                        Direccion = "Calle Elm 123",
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    context.Propietarios.AddRange(p1, p2, p3);
                    context.SaveChanges();

                    var m1 = new Mascota
                    {
                        PropietarioId = p1.Id,
                        Nombre = "Ayudante de Santa",
                        Especie = "Canino",
                        Raza = "Galgo Inglés",
                        FechaNacimiento = DateTime.UtcNow.AddYears(-3),
                        Sexo = "Macho",
                        PesoKg = 22.5m,
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    var m2 = new Mascota
                    {
                        PropietarioId = p1.Id,
                        Nombre = "Bola de Nieve V",
                        Especie = "Felino",
                        Raza = "Persa Mestizo",
                        FechaNacimiento = DateTime.UtcNow.AddYears(-2),
                        Sexo = "Hembra",
                        PesoKg = 4.2m,
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    var m3 = new Mascota
                    {
                        PropietarioId = p2.Id,
                        Nombre = "Barty",
                        Especie = "Canino",
                        Raza = "Golden Retriever",
                        FechaNacimiento = DateTime.UtcNow.AddYears(-4),
                        Sexo = "Macho",
                        PesoKg = 31.0m,
                        Estado = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    context.Mascotas.AddRange(m1, m2, m3);
                    context.SaveChanges();

                    // Citas de prueba
                    var vetMoe = context.Veterinarios.First(v => v.Nombre == "Moe");
                    var vetLisa = context.Veterinarios.First(v => v.Nombre == "Lisa");

                    var c1 = new Cita
                    {
                        MascotaId = m1.Id,
                        VeterinarioId = vetMoe.Id,
                        FechaHora = DateTime.Now.AddHours(2),
                        Motivo = "Revisión posoperatoria de extremidad posterior derecha",
                        Estado = EstadosCita.Pendiente,
                        Diagnostico = null,
                        Tratamiento = null,
                        FechaCreacion = DateTime.UtcNow
                    };

                    var c2 = new Cita
                    {
                        MascotaId = m2.Id,
                        VeterinarioId = vetLisa.Id,
                        FechaHora = DateTime.Now.AddDays(-2),
                        Motivo = "Vacunación anual trivalente felina y desparasitación",
                        Estado = EstadosCita.Completada,
                        Diagnostico = "Paciente clínicamente sano. Mucosas rosadas, ganglios normales, sin pulgas ni parásitos visibles.",
                        Tratamiento = "Se administra vacuna Felocell 3 (Lote: F3-2026-X) y antiparasitario interno comprimido.",
                        FechaCreacion = DateTime.UtcNow.AddDays(-3)
                    };

                    var c3 = new Cita
                    {
                        MascotaId = m3.Id,
                        VeterinarioId = vetMoe.Id,
                        FechaHora = DateTime.Now.AddDays(1).Date.AddHours(11),
                        Motivo = "Molestia en oreja izquierda, rascado frecuente y sacudidas",
                        Estado = EstadosCita.Pendiente,
                        Diagnostico = null,
                        Tratamiento = null,
                        FechaCreacion = DateTime.UtcNow
                    };

                    context.Citas.AddRange(c1, c2, c3);
                    context.SaveChanges();
                }
            }
            catch
            {
                // Si la BD aún no está disponible o no se ha corrido el script SQL, continúa
            }
        }
    }
}
