using InfoedukaApi.Data;
using InfoedukaApi.DTOs;
using InfoedukaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoedukaApi.Services
{
    public class KolegijService
    {
        private readonly AppDbContext _context;

        public KolegijService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<KolegijDto>> GetAll()
        {
            return await _context.Kolegiji
                .Include(k => k.Predavaci)
                .Select(k => new KolegijDto
                {
                    Id = k.Id,
                    Naziv = k.Naziv,
                    Opis = k.Opis,
                    Predavaci = k.Predavaci.Select(p => new KorisnikDto
                    {
                        Id = p.Id,
                        Ime = p.Ime,
                        Prezime = p.Prezime,
                        Email = p.Email,
                        Tip = p.Tip
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<List<KolegijDto>> GetByPredavac(int predavacId)
        {
            return await _context.Kolegiji
                .Include(k => k.Predavaci)
                .Where(k => k.Predavaci.Any(p => p.Id == predavacId))
                .Select(k => new KolegijDto
                {
                    Id = k.Id,
                    Naziv = k.Naziv,
                    Opis = k.Opis,
                    Predavaci = k.Predavaci.Select(p => new KorisnikDto
                    {
                        Id = p.Id,
                        Ime = p.Ime,
                        Prezime = p.Prezime,
                        Email = p.Email,
                        Tip = p.Tip
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<KolegijDto?> GetById(int id)
        {
            var k = await _context.Kolegiji
                .Include(k => k.Predavaci)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (k == null) return null;

            return new KolegijDto
            {
                Id = k.Id,
                Naziv = k.Naziv,
                Opis = k.Opis,
                Predavaci = k.Predavaci.Select(p => new KorisnikDto
                {
                    Id = p.Id,
                    Ime = p.Ime,
                    Prezime = p.Prezime,
                    Email = p.Email,
                    Tip = p.Tip
                }).ToList()
            };
        }

        public async Task<KolegijDto> Create(Kolegij kolegij)
        {
            _context.Kolegiji.Add(kolegij);
            await _context.SaveChangesAsync();

            return new KolegijDto
            {
                Id = kolegij.Id,
                Naziv = kolegij.Naziv,
                Opis = kolegij.Opis
            };
        }

        public async Task<bool> Update(int id, Kolegij updated)
        {
            var kolegij = await _context.Kolegiji.FindAsync(id);
            if (kolegij == null) return false;

            kolegij.Naziv = updated.Naziv;
            kolegij.Opis = updated.Opis;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var kolegij = await _context.Kolegiji.FindAsync(id);
            if (kolegij == null) return false;

            _context.Kolegiji.Remove(kolegij);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddPredavac(int kolegijId, int predavacId)
        {
            var kolegij = await _context.Kolegiji
                .Include(k => k.Predavaci)
                .FirstOrDefaultAsync(k => k.Id == kolegijId);

            var predavac = await _context.Korisnici.FindAsync(predavacId);

            if (kolegij == null || predavac == null) return false;

            kolegij.Predavaci.Add(predavac);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePredavac(int kolegijId, int predavacId)
        {
            var kolegij = await _context.Kolegiji
                .Include(k => k.Predavaci)
                .FirstOrDefaultAsync(k => k.Id == kolegijId);

            var predavac = kolegij?.Predavaci.FirstOrDefault(p => p.Id == predavacId);

            if (kolegij == null || predavac == null) return false;

            kolegij.Predavaci.Remove(predavac);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}