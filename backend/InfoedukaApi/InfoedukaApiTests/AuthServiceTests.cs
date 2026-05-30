using InfoedukaApi.Data;
using InfoedukaApi.DTOs;
using InfoedukaApi.Models;
using InfoedukaApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfoedukaApiTests
{
    public class AuthServiceTests
    {
        private AppDbContext KreirajBazu()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private IConfiguration KreirajKonfiguraciju()
        {
            var config = new Dictionary<string, string>
            {
                { "Jwt:Key", "InfoedukaSecretKey12345678901234" },
                { "Jwt:Issuer", "InfoedukaApi" },
                { "Jwt:Audience", "InfoedukaClient" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(config!)
                .Build();
        }

        [Fact]
        public async Task Login_IspravniPodaci_VracaToken()
        {
            // Arrange
            var db = KreirajBazu();
            var config = KreirajKonfiguraciju();

            db.Korisnici.Add(new Korisnik
            {
                Id = 1,
                Ime = "Admin",
                Prezime = "Admin",
                Email = "admin@infoeduka.hr",
                Lozinka = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Tip = "Admin"
            });
            await db.SaveChangesAsync();

            var service = new AuthService(db, config);
            var dto = new LoginDto { Email = "admin@infoeduka.hr", Lozinka = "admin123" };

            // Act
            var token = await service.Login(dto);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public async Task Login_NeispravnaLozinka_VracaNull()
        {
            // Arrange
            var db = KreirajBazu();
            var config = KreirajKonfiguraciju();

            db.Korisnici.Add(new Korisnik
            {
                Id = 1,
                Ime = "Admin",
                Prezime = "Admin",
                Email = "admin@infoeduka.hr",
                Lozinka = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Tip = "Admin"
            });
            await db.SaveChangesAsync();

            var service = new AuthService(db, config);
            var dto = new LoginDto { Email = "admin@infoeduka.hr", Lozinka = "pogresna" };

            // Act
            var token = await service.Login(dto);

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public async Task Login_NepostojeciEmail_VracaNull()
        {
            // Arrange
            var db = KreirajBazu();
            var config = KreirajKonfiguraciju();

            var service = new AuthService(db, config);
            var dto = new LoginDto { Email = "nepostoji@infoeduka.hr", Lozinka = "admin123" };

            // Act
            var token = await service.Login(dto);

            // Assert
            Assert.Null(token);
        }
    }
}