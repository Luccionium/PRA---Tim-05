using InfoedukaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoedukaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Kolegij> Kolegiji { get; set; }
        public DbSet<Obavijest> Obavijesti { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-many veza između Korisnik i Kolegij
            modelBuilder.Entity<Kolegij>()
                .HasMany(k => k.Predavaci)
                .WithMany(k => k.Kolegiji)
                .UsingEntity(j =>
                {
                    j.ToTable("KolegijPredavac");
                    j.HasData(new { KolegijiId = 1, PredavaciId = 2 });
                });

            // Obavijest -> Kolegij
            modelBuilder.Entity<Obavijest>()
                .HasOne(o => o.Kolegij)
                .WithMany(k => k.Obavijesti)
                .HasForeignKey(o => o.KolegijId);

            // Obavijest -> Kreator
            modelBuilder.Entity<Obavijest>()
                .HasOne(o => o.Kreator)
                .WithMany(k => k.Obavijesti)
                .HasForeignKey(o => o.KreatorId);

            // Seed admin korisnik
            modelBuilder.Entity<Korisnik>().HasData(new Korisnik
            {
                Id = 1,
                Ime = "Admin",
                Prezime = "Admin",
                Email = "admin@infoeduka.hr",
                Lozinka = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Tip = "Admin"
            });

            // Seed predavač
            modelBuilder.Entity<Korisnik>().HasData(new Korisnik
            {
                Id = 2,
                Ime = "Ivan",
                Prezime = "Ivić",
                Email = "ivan@infoeduka.hr",
                Lozinka = BCrypt.Net.BCrypt.HashPassword("predavac123"),
                Tip = "Predavac"
            });

            // Seed kolegiji
            modelBuilder.Entity<Kolegij>().HasData(
                new Kolegij { Id = 1, Naziv = "Projektni Razvoj Aplikacija", Opis = "Kolegij o razvoju aplikacija" },
                new Kolegij { Id = 2, Naziv = "Baze Podataka", Opis = "Kolegij o bazama podataka" }
            );

        }
    }
}