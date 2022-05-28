using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum
{
    public class KalumDbContext : DbContext
    {
        public DbSet<CarreraTecnica> CarreraTecnica { get; set; }
        public DbSet<Aspirante> Aspirante {get; set;}

        public DbSet<Jornada> Jornada { get; set; }
        public KalumDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new {ct.CarreraId});
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new {a.NoExpediente});
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new {j.JornadaId});
            modelBuilder.Entity<Aspirante>()
                .HasOne<CarreraTecnica>(c => c.CarreraTecnica)
                .WithMany(ct => ct.Aspirante)
                .HasForeignKey(c => c.CarreraId);
            modelBuilder.Entity<Aspirante>()
                        .HasOne<Jornada>(j => j.Jornada)
                        .WithMany(ct => ct.Aspirante)
                        .HasForeignKey(j => j.JornadaId);

        }
    }
}