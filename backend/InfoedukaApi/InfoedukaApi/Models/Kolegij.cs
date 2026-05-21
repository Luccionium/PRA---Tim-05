namespace InfoedukaApi.Models
{
    public class Kolegij
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;

        public List<Korisnik> Predavaci { get; set; } = new();
        public List<Obavijest> Obavijesti { get; set; } = new();
    }
}