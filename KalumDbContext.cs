using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum
{
    public class KalumDbContext : DbContext
    {
        public DbSet<CarreraTecnica> CarreraTecnica { get; set; }
        public DbSet<Aspirante> Aspirante {get; set;}

        public DbSet<Jornada> Jornada { get; set; }
        
        public DbSet<ExamenAdmision> ExamenAdmision {get; set; }

        public DbSet <Inscripcion> Inscripcion { get; set; }
        public DbSet <Alumno> Alumno { get; set; }
        public DbSet <Cargo> Cargo { get; set; }
        public DbSet <CuentaXCobrar> CuentaXCobrar { get; set; }
        public DbSet <InscripcionPago> InscripcionPago { get; set; }
        public DbSet <ResultadoExamenAdmision> ResultadoExamenAdmision { get; set; }
        public DbSet <InversionCarreraTecnica> InversionCarreraTecnica { get; set; }






        public KalumDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new {ct.CarreraId});
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new {a.NoExpediente});
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new {j.JornadaId});
            modelBuilder.Entity<ExamenAdmision>().ToTable("ExamenAdmision").HasKey(ex => new {ex.ExamenId});
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion").HasKey(i => new {i.InscripcionId});
            modelBuilder.Entity<Alumno>().ToTable("Alumno").HasKey(a => new {a.Carne});
            modelBuilder.Entity<Cargo>().ToTable("Cargo").HasKey(c => new {c.CargoId});
            modelBuilder.Entity<CuentaXCobrar>().ToTable("CuentaXCobrar").HasKey(cxc => new {cxc.Correlativo,cxc.Carne,cxc.Anio});
            modelBuilder.Entity<InscripcionPago>().ToTable("InscripcionPago").HasKey(ip => new {ip.NoExpediente,ip.BoletaPago,ip.Anio});
            modelBuilder.Entity<ResultadoExamenAdmision>().ToTable("ResultadoExamenAdmision").HasKey(rea => new {rea.NoExpediente,rea.Anio});
            modelBuilder.Entity<InversionCarreraTecnica>().ToTable("InversionCarreraTecnica").HasKey(ict => new {ict.InversionId,ict.CarreraId});
           
            modelBuilder.Entity<Aspirante>()
                        .HasOne<CarreraTecnica>(a => a.CarreraTecnica)
                .WithMany(ct => ct.Aspirantes)
                .HasForeignKey(a => a.CarreraId);

            modelBuilder.Entity<Aspirante>()
                        .HasOne<Jornada>(a => a.Jornada)
                        .WithMany(j => j.Aspirantes)
                        .HasForeignKey(a => a.JornadaId);

            modelBuilder.Entity<Aspirante>()
                        .HasOne<ExamenAdmision>(a => a.ExamenAdmision)
                        .WithMany(ex => ex.Aspirantes)
                        .HasForeignKey(a => a.ExamenId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<CarreraTecnica>(i => i.CarreraTecnica)
                .WithMany(ct => ct.Inscripciones)
                .HasForeignKey(i => i.CarreraId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Jornada>(i => i.Jornada)
                .WithMany(j => j.Inscripciones)
                .HasForeignKey(i=> i.JornadaId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Alumno>(i => i.Alumno)
                .WithMany(a => a.Inscripciones)
                .HasForeignKey(i => i.Carne);


            modelBuilder.Entity<CuentaXCobrar>().ToTable("CuentaXCobrar")
                .HasOne<Alumno>(cxc => cxc.Alumno)
                .WithMany(a => a.CuentaXCobrar)
                .HasForeignKey(cxc=> cxc.Carne);
                
            modelBuilder.Entity<CuentaXCobrar>().ToTable("CuentaXCobrar")
                .HasOne<Cargo>(cxc => cxc.Cargo)
                .WithMany(c => c.CuentaXCobrar)
                .HasForeignKey(cxc=> cxc.CargoId);
            
            modelBuilder.Entity<InscripcionPago>().ToTable("InscripcionPago")
                .HasOne<Aspirante>(ip => ip.Aspirante)
                .WithMany(a => a.InscripcionesPago)
                .HasForeignKey(ip=> ip.NoExpediente);

            modelBuilder.Entity<ResultadoExamenAdmision>().ToTable("ResultadoExamenAdmision")
                .HasOne<Aspirante>(rea => rea.Aspirante)
                .WithMany(a => a.ResultadoExamenAdmision)
                .HasForeignKey(rea=> rea.NoExpediente);

            modelBuilder.Entity<InversionCarreraTecnica>().ToTable("InversionCarreraTecnica")
                .HasOne<CarreraTecnica>(ict => ict.CarreraTecnica)
                .WithMany(a => a.InversionCarreraTecnica)
                .HasForeignKey(ict=> ict.InversionId);


        }
    }
}