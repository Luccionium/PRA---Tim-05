namespace InfoedukaApi.DTOs
{
    public class KolegijDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public List<KorisnikDto> Predavaci { get; set; } = new();
    }
}