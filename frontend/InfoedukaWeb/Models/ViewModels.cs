using System.ComponentModel.DataAnnotations;

namespace InfoedukaWeb.Models;

public class LoginViewModel
{
    [Required] public string Email { get; set; } = "";
    [Required] public string Lozinka { get; set; } = "";
}

public class KorisnikViewModel
{
    public int Id { get; set; }
    public string Ime { get; set; } = "";
    public string Prezime { get; set; } = "";
    public string Email { get; set; } = "";
    public string Tip { get; set; } = "";
}

public class KorisnikFormViewModel
{
    public int Id { get; set; }
    [Required] public string Ime { get; set; } = "";
    [Required] public string Prezime { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required] public string Lozinka { get; set; } = "";
    [Required] public string Tip { get; set; } = "Predavac";
}

public class KolegijViewModel
{
    public int Id { get; set; }
    public string Naziv { get; set; } = "";
    public string Opis { get; set; } = "";
    public List<KorisnikViewModel> Predavaci { get; set; } = new();
}

public class KolegijFormViewModel
{
    public int Id { get; set; }
    [Required] public string Naziv { get; set; } = "";
    public string Opis { get; set; } = "";
    public List<int> SelectedPredavaciIds { get; set; } = new();
    public List<KorisnikViewModel> SviPredavaci { get; set; } = new();
}

public class KolegijDetailsViewModel
{
    public KolegijViewModel Kolegij { get; set; } = new();
    public List<ObavijestViewModel> Obavijesti { get; set; } = new();
    public List<KorisnikViewModel> SviPredavaci { get; set; } = new();
}

public class ObavijestViewModel
{
    public int Id { get; set; }
    public string Naziv { get; set; } = "";
    public string Opis { get; set; } = "";
    public DateTime DatumObjave { get; set; }
    public DateTime DatumIsteka { get; set; }
    public int KolegijId { get; set; }
    public string KolegijNaziv { get; set; } = "";
    public string KreatorIme { get; set; } = "";
}

public class ObavijestFormViewModel
{
    public int Id { get; set; }
    [Required] public string Naziv { get; set; } = "";
    public string Opis { get; set; } = "";
    public DateTime DatumObjave { get; set; } = DateTime.Today;
    public DateTime DatumIsteka { get; set; } = DateTime.Today.AddDays(7);
    [Range(1, int.MaxValue, ErrorMessage = "Odaberite kolegij.")]
    public int KolegijId { get; set; }
    public int? ReturnKolegijId { get; set; }
    public List<KolegijViewModel> Kolegiji { get; set; } = new();
}
