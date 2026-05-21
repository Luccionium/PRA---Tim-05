namespace InfoedukaApi.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
        public string Tip { get; set; } = string.Empty; // "Admin" ili "Predavac"

        public List<Kolegij> Kolegiji { get; set; } = new();
        public List<Obavijest> Obavijesti { get; set; } = new();
    }
}