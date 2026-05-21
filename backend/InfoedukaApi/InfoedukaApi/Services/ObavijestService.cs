using InfoedukaApi.Data;
using InfoedukaApi.DTOs;
using InfoedukaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoedukaApi.Services
{
    public class ObavijestService
    {
        private readonly AppDbContext _context;

        public ObavijestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ObavijestDto>> GetAll()
        {
            return await _context.Obavijesti
                .Include(o => o.Kolegij)
                .Include(o => o.Kreator)
                .Select(o => new ObavijestDto
                {
                    Id = o.Id,
                    Naziv = o.Naziv,
                    Opis = o.Opis,
                    DatumObjave = o.DatumObjave,
                    DatumIsteka = o.DatumIsteka,
                    KolegijId = o.KolegijId,
                    KolegijNaziv = o.Kolegij.Naziv,
                    KreatorIme = o.Kreator.Ime + " " + o.Kreator.Prezime
                }).ToListAsync();
        }

        public async Task<List<ObavijestDto>> GetByKolegij(int kolegijId)
        {
            return await _context.Obavijesti
                .Include(o => o.Kolegij)
                .Include(o => o.Kreator)
                .Where(o => o.KolegijId == kolegijId)
                .Select(o => new ObavijestDto
                {
                    Id = o.Id,
                    Naziv = o.Naziv,
                    Opis = o.Opis,
                    DatumObjave = o.DatumObjave,
                    DatumIsteka = o.DatumIsteka,
                    KolegijId = o.KolegijId,
                    KolegijNaziv = o.Kolegij.Naziv,
                    KreatorIme = o.Kreator.Ime + " " + o.Kreator.Prezime
                }).ToListAsync();
        }

        public async Task<ObavijestDto?> GetById(int id)
        {
            var o = await _context.Obavijesti
                .Include(o => o.Kolegij)
                .Include(o => o.Kreator)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (o == null) return null;

            return new ObavijestDto
            {
                Id = o.Id,
                Naziv = o.Naziv,
                Opis = o.Opis,
                DatumObjave = o.DatumObjave,
                DatumIsteka = o.DatumIsteka,
                KolegijId = o.KolegijId,
                KolegijNaziv = o.Kolegij.Naziv,
                KreatorIme = o.Kreator.Ime + " " + o.Kreator.Prezime
            };
        }

        public async Task<ObavijestDto> Create(Obavijest obavijest)
        {
            _context.Obavijesti.Add(obavijest);
            await _context.SaveChangesAsync();

            return await GetById(obavijest.Id) ?? new ObavijestDto();
        }

        public async Task<bool> Update(int id, Obavijest updated)
        {
            var obavijest = await _context.Obavijesti.FindAsync(id);
            if (obavijest == null) return false;

            obavijest.Naziv = updated.Naziv;
            obavijest.Opis = updated.Opis;
            obavijest.DatumObjave = updated.DatumObjave;
            obavijest.DatumIsteka = updated.DatumIsteka;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var obavijest = await _context.Obavijesti.FindAsync(id);
            if (obavijest == null) return false;

            _context.Obavijesti.Remove(obavijest);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}