using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PW3_04.Models
{
    public static class RolesUsuario
    {
        public const string Administrador = "Administrador";
        public const string Veterinario = "Veterinario";
        public const string Recepcionista = "Recepcionista";

        public static readonly string[] Todos = { Administrador, Veterinario, Recepcionista };
    }

    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        [Column("nombre_completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
        [StringLength(120)]
        [Display(Name = "Correo Electrónico")]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Display(Name = "Rol")]
        [Column("rol")]
        public string Rol { get; set; } = RolesUsuario.Recepcionista;

        [Display(Name = "Estado")]
        [Column("estado")]
        public bool Estado { get; set; } = true;

        [Display(Name = "Último Acceso")]
        [Column("ultimo_acceso")]
        public DateTime? UltimoAcceso { get; set; }

        [Display(Name = "Fecha de Registro")]
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
