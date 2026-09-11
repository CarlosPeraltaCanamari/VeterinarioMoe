using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PW3_04.Models
{
    [Table("mascotas")]
    public class Mascota
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe asociar la mascota a un propietario.")]
        [Display(Name = "Propietario")]
        [Column("propietario_id")]
        public int PropietarioId { get; set; }

        [ForeignKey("PropietarioId")]
        public virtual Propietario? Propietario { get; set; }

        [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        [Display(Name = "Nombre de la Mascota")]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especie es obligatoria.")]
        [StringLength(40, ErrorMessage = "La especie no puede exceder 40 caracteres.")]
        [Display(Name = "Especie")]
        [Column("especie")]
        public string Especie { get; set; } = "Canino"; // Canino, Felino, Ave, Roedor, Reptil, Otro

        [StringLength(60, ErrorMessage = "La raza no puede exceder 60 caracteres.")]
        [Display(Name = "Raza")]
        [Column("raza")]
        public string Raza { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        [Column("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; } = DateTime.Today.AddYears(-1);

        [StringLength(10)]
        [Display(Name = "Sexo")]
        [Column("sexo")]
        public string Sexo { get; set; } = "Macho"; // Macho / Hembra

        [Range(0.01, 200.0, ErrorMessage = "El peso debe estar entre 0.01 y 200 kg.")]
        [Display(Name = "Peso (Kg)")]
        [Column("peso_kg", TypeName = "decimal(5,2)")]
        public decimal? PesoKg { get; set; }

        [Display(Name = "Estado")]
        [Column("estado")]
        public bool Estado { get; set; } = true; // true = Activo, false = Inactivo

        [Display(Name = "Fecha de Registro")]
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string DescripcionCompleta => $"{Nombre} ({Especie} - {Raza})";

        // Relaciones
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
