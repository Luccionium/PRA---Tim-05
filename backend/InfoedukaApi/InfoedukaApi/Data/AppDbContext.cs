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
                .UsingEntity(j => j.ToTable("KolegijPredavac"));

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
        }
    }
}