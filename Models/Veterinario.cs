using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PW3_04.Models
{
    [Table("veterinarios")]
    public class Veterinario
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

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [StringLength(80, ErrorMessage = "La especialidad no puede exceder 80 caracteres.")]
        [Display(Name = "Especialidad")]
        [Column("especialidad")]
        public string Especialidad { get; set; } = "Medicina General";

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "Formato de teléfono no válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres.")]
        [Display(Name = "Teléfono")]
        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres.")]
        [Display(Name = "Correo Electrónico")]
        [Column("email")]
        public string? Email { get; set; }

        [Display(Name = "Estado")]
        [Column("estado")]
        public bool Estado { get; set; } = true; // true = Activo, false = Inactivo

        [Display(Name = "Fecha de Registro")]
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Display(Name = "Nombre Completo")]
        [NotMapped]
        public string NombreCompleto => $"Dr(a). {Nombre} {Apellidos}";

        [NotMapped]
        public string DescripcionCompleta => $"Dr(a). {Nombre} {Apellidos} - {Especialidad}";

        // Relaciones
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
