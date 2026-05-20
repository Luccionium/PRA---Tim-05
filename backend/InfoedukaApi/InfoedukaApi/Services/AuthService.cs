using InfoedukaApi.Data;
using InfoedukaApi.DTOs;
using InfoedukaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InfoedukaApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> Login(LoginDto dto)
        {
            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Email == dto.Email);

            if (korisnik == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(dto.Lozinka, korisnik.Lozinka)) return null;

            return GenerateToken(korisnik);
        }

        private string GenerateToken(Korisnik korisnik)
        {
            var claims = new[]
            {
                new Claim("id", korisnik.Id.ToString()),
                new Claim(ClaimTypes.Role, korisnik.Tip),
                new Claim(ClaimTypes.Email, korisnik.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}