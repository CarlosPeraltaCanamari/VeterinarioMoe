using Microsoft.EntityFrameworkCore;
using PW3_04.Models;

namespace PW3_04.Data
{
    public class ArcaMoeDbContext : DbContext
    {
        public ArcaMoeDbContext(DbContextOptions<ArcaMoeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Propietario> Propietarios => Set<Propietario>();
        public DbSet<Mascota> Mascotas => Set<Mascota>();
        public DbSet<Veterinario> Veterinarios => Set<Veterinario>();
        public DbSet<Cita> Citas => Set<Cita>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índices y configuraciones Propietario
            modelBuilder.Entity<Propietario>(entity =>
            {
                entity.HasIndex(p => p.Email);
                entity.HasIndex(p => p.Telefono);
                entity.Property(p => p.Estado).HasDefaultValue(true);
            });

            // Relación Mascota -> Propietario
            modelBuilder.Entity<Mascota>(entity =>
            {
                entity.HasOne(m => m.Propietario)
                      .WithMany(p => p.Mascotas)
                      .HasForeignKey(m => m.PropietarioId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(m => m.PropietarioId);
                entity.HasIndex(m => m.Nombre);
                entity.Property(m => m.Estado).HasDefaultValue(true);
            });

            // Relación Cita -> Mascota y Cita -> Veterinario
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasOne(c => c.Mascota)
                      .WithMany(m => m.Citas)
                      .HasForeignKey(c => c.MascotaId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Veterinario)
                      .WithMany(v => v.Citas)
                      .HasForeignKey(c => c.VeterinarioId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => c.FechaHora);
                entity.HasIndex(c => c.Estado);
            });

            // Configuración Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Estado).HasDefaultValue(true);
            });
        }
    }
}
