using InfoedukaApi.Data;
using InfoedukaApi.Models;
using InfoedukaApi.Services;
using Microsoft.EntityFrameworkCore;

namespace InfoedukaApiTests
{
    public class KolegijServiceTests
    {
        private AppDbContext KreirajBazu()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAll_VracaSveKolegije()
        {
            // Arrange
            var db = KreirajBazu();
            db.Kolegiji.AddRange(
                new Kolegij { Id = 1, Naziv = "PRA", Opis = "Opis" },
                new Kolegij { Id = 2, Naziv = "Baze Podataka", Opis = "Opis" }
            );
            await db.SaveChangesAsync();

            var service = new KolegijService(db);

            // Act
            var kolegiji = await service.GetAll();

            // Assert
            Assert.Equal(2, kolegiji.Count);
        }

        [Fact]
        public async Task GetByPredavac_VracaSamoKolegijePreavaca()
        {
            // Arrange
            var db = KreirajBazu();

            var predavac = new Korisnik { Id = 1, Ime = "Ivan", Prezime = "Ivić", Email = "ivan@infoeduka.hr", Lozinka = "hash", Tip = "Predavac" };
            var kolegij1 = new Kolegij { Id = 1, Naziv = "PRA", Opis = "Opis" };
            var kolegij2 = new Kolegij { Id = 2, Naziv = "Baze Podataka", Opis = "Opis" };

            kolegij1.Predavaci.Add(predavac);

            db.Kolegiji.AddRange(kolegij1, kolegij2);
            await db.SaveChangesAsync();

            var service = new KolegijService(db);

            // Act
            var kolegiji = await service.GetByPredavac(1);

            // Assert
            Assert.Single(kolegiji);
            Assert.Equal("PRA", kolegiji[0].Naziv);
        }

        [Fact]
        public async Task Create_KreiraNoviKolegij()
        {
            // Arrange
            var db = KreirajBazu();
            var service = new KolegijService(db);

            var kolegij = new Kolegij { Naziv = "Novi Kolegij", Opis = "Opis" };

            // Act
            var novi = await service.Create(kolegij);

            // Assert
            Assert.NotNull(novi);
            Assert.Equal("Novi Kolegij", novi.Naziv);
        }

        [Fact]
        public async Task Delete_BriseKolegij()
        {
            // Arrange
            var db = KreirajBazu();
            db.Kolegiji.Add(new Kolegij { Id = 1, Naziv = "PRA", Opis = "Opis" });
            await db.SaveChangesAsync();

            var service = new KolegijService(db);

            // Act
            var rezultat = await service.Delete(1);

            // Assert
            Assert.True(rezultat);
            Assert.Empty(db.Kolegiji);
        }
    }
}