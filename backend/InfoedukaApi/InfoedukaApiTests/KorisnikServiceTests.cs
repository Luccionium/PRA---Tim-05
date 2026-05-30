using InfoedukaApi.Data;
using InfoedukaApi.Models;
using InfoedukaApi.Services;
using Microsoft.EntityFrameworkCore;

namespace InfoedukaApiTests
{
    public class KorisnikServiceTests
    {
        private AppDbContext KreirajBazu()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAll_VracaSveKorisnike()
        {
            // Arrange
            var db = KreirajBazu();
            db.Korisnici.AddRange(
                new Korisnik { Id = 1, Ime = "Admin", Prezime = "Admin", Email = "admin@infoeduka.hr", Lozinka = "hash", Tip = "Admin" },
                new Korisnik { Id = 2, Ime = "Ivan", Prezime = "Ivić", Email = "ivan@infoeduka.hr", Lozinka = "hash", Tip = "Predavac" }
            );
            await db.SaveChangesAsync();

            var service = new KorisnikService(db);

            // Act
            var korisnici = await service.GetAll();

            // Assert
            Assert.Equal(2, korisnici.Count);
        }

        [Fact]
        public async Task GetById_PostojeciId_VracaKorisnika()
        {
            // Arrange
            var db = KreirajBazu();
            db.Korisnici.Add(new Korisnik { Id = 1, Ime = "Admin", Prezime = "Admin", Email = "admin@infoeduka.hr", Lozinka = "hash", Tip = "Admin" });
            await db.SaveChangesAsync();

            var service = new KorisnikService(db);

            // Act
            var korisnik = await service.GetById(1);

            // Assert
            Assert.NotNull(korisnik);
            Assert.Equal("Admin", korisnik.Ime);
        }

        [Fact]
        public async Task GetById_NepostojeciId_VracaNull()
        {
            // Arrange
            var db = KreirajBazu();
            var service = new KorisnikService(db);

            // Act
            var korisnik = await service.GetById(999);

            // Assert
            Assert.Null(korisnik);
        }
    }
}