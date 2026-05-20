namespace InfoedukaApi.Models
{
    public class Obavijest
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public DateTime DatumObjave { get; set; }
        public DateTime DatumIsteka { get; set; }

        public int KolegijId { get; set; }
        public Kolegij Kolegij { get; set; } = null!;

        public int KreatorId { get; set; }
        public Korisnik Kreator { get; set; } = null!;
    }
}