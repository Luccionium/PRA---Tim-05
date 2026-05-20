namespace InfoedukaApi.DTOs
{
    public class ObavijestDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public DateTime DatumObjave { get; set; }
        public DateTime DatumIsteka { get; set; }
        public int KolegijId { get; set; }
        public string KolegijNaziv { get; set; } = string.Empty;
        public string KreatorIme { get; set; } = string.Empty;
    }
}