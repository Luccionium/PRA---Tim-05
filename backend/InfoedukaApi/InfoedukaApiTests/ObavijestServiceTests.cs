using InfoedukaApi.Data;
using InfoedukaApi.Models;
using InfoedukaApi.Services;
using Microsoft.EntityFrameworkCore;

namespace InfoedukaApiTests
{
    public class ObavijestServiceTests
    {
        private AppDbContext KreirajBazu()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetByKolegij_VracaObavijesti()
        {
            // Arrange
            var db = KreirajBazu();

            var kreator = new Korisnik { Id = 1, Ime = "Admin", Prezime = "Admin", Email = "admin@infoeduka.hr", Lozinka = "hash", Tip = "Admin" };
            var kolegij = new Kolegij { Id = 1, Naziv = "PRA", Opis = "Opis" };

            db.Korisnici.Add(kreator);
            db.Kolegiji.Add(kolegij);
            db.Obavijesti.AddRange(
                new Obavijest { Id = 1, Naziv = "Obavijest 1", Opis = "Opis", DatumObjave = DateTime.Now, DatumIsteka = DateTime.Now.AddDays(7), KolegijId = 1, KreatorId = 1 },
                new Obavijest { Id = 2, Naziv = "Obavijest 2", Opis = "Opis", DatumObjave = DateTime.Now, DatumIsteka = DateTime.Now.AddDays(7), KolegijId = 1, KreatorId = 1 }
            );
            await db.SaveChangesAsync();

            var service = new ObavijestService(db);

            // Act
            var obavijesti = await service.GetByKolegij(1);

            // Assert
            Assert.Equal(2, obavijesti.Count);
        }

        [Fact]
        public async Task Create_KreiraNovaObavijest()
        {
            // Arrange
            var db = KreirajBazu();

            var kreator = new Korisnik { Id = 1, Ime = "Admin", Prezime = "Admin", Email = "admin@infoeduka.hr", Lozinka = "hash", Tip = "Admin" };
            var kolegij = new Kolegij { Id = 1, Naziv = "PRA", Opis = "Opis" };

            db.Korisnici.Add(kreator);
            db.Kolegiji.Add(kolegij);
            await db.SaveChangesAsync();

            var service = new ObavijestService(db);

            var obavijest = new Obavijest
            {
                Naziv = "Nova Obavijest",
                Opis = "Opis",
                DatumObjave = DateTime.Now,
                DatumIsteka = DateTime.Now.AddDays(7),
                KolegijId = 1,
                KreatorId = 1
            };

            // Act
            var nova = await service.Create(obavijest);

            // Assert
            Assert.NotNull(nova);
            Assert.Equal("Nova Obavijest", nova.Naziv);
        }

        [Fact]
        public async Task Delete_BriseObavijest()
        {
            // Arrange
            var db = KreirajBazu();

            var kreator = new Korisnik { Id = 1, Ime = "Admin", Prezime = "Admin", Email = "admin@infoeduka.hr", Lozinka = "hash", Tip = "Admin" };
            var kolegij = new Kolegij { Id = 1, Naziv = "PRA", Opis = "Opis" };

            db.Korisnici.Add(kreator);
            db.Kolegiji.Add(kolegij);
            db.Obavijesti.Add(new Obavijest { Id = 1, Naziv = "Obavijest", Opis = "Opis", DatumObjave = DateTime.Now, DatumIsteka = DateTime.Now.AddDays(7), KolegijId = 1, KreatorId = 1 });
            await db.SaveChangesAsync();

            var service = new ObavijestService(db);

            // Act
            var rezultat = await service.Delete(1);

            // Assert
            Assert.True(rezultat);
            Assert.Empty(db.Obavijesti);
        }
    }
}