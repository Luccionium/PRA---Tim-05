using InfoedukaApi.Data;
using InfoedukaApi.DTOs;
using InfoedukaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoedukaApi.Services
{
    public class KorisnikService
    {
        private readonly AppDbContext _context;

        public KorisnikService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<KorisnikDto>> GetAll()
        {
            return await _context.Korisnici
                .Select(k => new KorisnikDto
                {
                    Id = k.Id,
                    Ime = k.Ime,
                    Prezime = k.Prezime,
                    Email = k.Email,
                    Tip = k.Tip
                }).ToListAsync();
        }

        public async Task<KorisnikDto?> GetById(int id)
        {
            var k = await _context.Korisnici.FindAsync(id);
            if (k == null) return null;

            return new KorisnikDto
            {
                Id = k.Id,
                Ime = k.Ime,
                Prezime = k.Prezime,
                Email = k.Email,
                Tip = k.Tip
            };
        }

        public async Task<KorisnikDto> Create(Korisnik korisnik)
        {
            korisnik.Lozinka = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);
            _context.Korisnici.Add(korisnik);
            await _context.SaveChangesAsync();

            return new KorisnikDto
            {
                Id = korisnik.Id,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                Email = korisnik.Email,
                Tip = korisnik.Tip
            };
        }

        public async Task<bool> Update(int id, Korisnik updated)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);
            if (korisnik == null) return false;

            korisnik.Ime = updated.Ime;
            korisnik.Prezime = updated.Prezime;
            korisnik.Email = updated.Email;
            korisnik.Tip = updated.Tip;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);
            if (korisnik == null) return false;

            _context.Korisnici.Remove(korisnik);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}