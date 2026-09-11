using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PW3_04.Models
{
    [Table("propietarios")]
    public class Propietario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(60, ErrorMessage = "El nombre no puede exceder 60 caracteres.")]
        [Display(Name = "Nombre")]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(80, ErrorMessage = "Los apellidos no pueden exceder 80 caracteres.")]
        [Display(Name = "Apellidos")]
        [Column("apellidos")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "Formato de teléfono no válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres.")]
        [Display(Name = "Teléfono")]
        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres.")]
        [Display(Name = "Correo Electrónico")]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres.")]
        [Display(Name = "Dirección")]
        [Column("direccion")]
        public string? Direccion { get; set; }

        [Display(Name = "Estado")]
        [Column("estado")]
        public bool Estado { get; set; } = true; // true = Activo, false = Inactivo

        [Display(Name = "Fecha de Registro")]
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Display(Name = "Nombre Completo")]
        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellidos}";

        // Relaciones
        public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
    }
}
