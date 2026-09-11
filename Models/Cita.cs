using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PW3_04.Models
{
    public static class EstadosCita
    {
        public const string Pendiente = "Pendiente";
        public const string Completada = "Completada";
        public const string Cancelada = "Cancelada";

        public static readonly string[] Lista = { Pendiente, Completada, Cancelada };
    }

    [Table("citas")]
    public class Cita
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una mascota.")]
        [Display(Name = "Mascota")]
        [Column("mascota_id")]
        public int MascotaId { get; set; }

        [ForeignKey("MascotaId")]
        public virtual Mascota? Mascota { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un veterinario.")]
        [Display(Name = "Veterinario")]
        [Column("veterinario_id")]
        public int VeterinarioId { get; set; }

        [ForeignKey("VeterinarioId")]
        public virtual Veterinario? Veterinario { get; set; }

        [Required(ErrorMessage = "La fecha y hora de la cita es obligatoria.")]
        [Display(Name = "Fecha y Hora de la Cita")]
        [Column("fecha_hora")]
        public DateTime FechaHora { get; set; } = DateTime.Now.AddHours(2);

        [Required(ErrorMessage = "El motivo de la cita es obligatorio.")]
        [StringLength(250, ErrorMessage = "El motivo no puede superar 250 caracteres.")]
        [Display(Name = "Motivo de la Cita")]
        [Column("motivo")]
        public string Motivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado de la cita es obligatorio.")]
        [StringLength(20)]
        [Display(Name = "Estado")]
        [Column("estado")]
        public string Estado { get; set; } = EstadosCita.Pendiente; // Pendiente, Completada, Cancelada

        [Display(Name = "Diagnóstico Clínico")]
        [Column("diagnostico")]
        public string? Diagnostico { get; set; }

        [Display(Name = "Tratamiento / Receta")]
        [Column("tratamiento")]
        public string? Tratamiento { get; set; }

        [Display(Name = "Fecha de Registro")]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
