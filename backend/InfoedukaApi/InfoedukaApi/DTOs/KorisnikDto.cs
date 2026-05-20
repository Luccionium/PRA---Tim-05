namespace InfoedukaApi.DTOs
{
    public class KorisnikDto
    {
        public int Id { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tip { get; set; } = string.Empty;
    }
}