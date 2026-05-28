using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using InfoedukaWeb.Models;

namespace InfoedukaWeb.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _ctx;
    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public ApiService(HttpClient http, IHttpContextAccessor ctx)
    {
        _http = http;
        _ctx = ctx;
    }

    private HttpRequestMessage Req(HttpMethod method, string url, object? body = null)
    {
        var req = new HttpRequestMessage(method, url);
        var token = _ctx.HttpContext?.User.FindFirst("jwt")?.Value;
        if (token != null)
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (body != null)
            req.Content = JsonContent.Create(body);
        return req;
    }

    public async Task<string?> LoginAsync(string email, string lozinka)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Post, "api/auth/login", new { email, lozinka }));
        if (!resp.IsSuccessStatusCode) return null;
        var obj = await resp.Content.ReadFromJsonAsync<JsonElement>();
        return obj.GetProperty("token").GetString();
    }

    // Kolegij
    public async Task<List<KolegijViewModel>> GetKolegijiAsync()
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Get, "api/kolegij"));
        if (!resp.IsSuccessStatusCode) return new();
        return await resp.Content.ReadFromJsonAsync<List<KolegijViewModel>>(_json) ?? new();
    }

    public async Task<KolegijViewModel?> GetKolegijAsync(int id)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Get, $"api/kolegij/{id}"));
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<KolegijViewModel>(_json);
    }

    public async Task<KolegijViewModel?> CreateKolegijAsync(string naziv, string opis)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Post, "api/kolegij", new { naziv, opis }));
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<KolegijViewModel>(_json);
    }

    public async Task<bool> UpdateKolegijAsync(int id, string naziv, string opis)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Put, $"api/kolegij/{id}", new { naziv, opis }));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteKolegijAsync(int id)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Delete, $"api/kolegij/{id}"));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> AddPredavacAsync(int kolegijId, int predavacId)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Post, $"api/kolegij/{kolegijId}/predavac/{predavacId}"));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> RemovePredavacAsync(int kolegijId, int predavacId)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Delete, $"api/kolegij/{kolegijId}/predavac/{predavacId}"));
        return resp.IsSuccessStatusCode;
    }

    // Korisnik
    public async Task<List<KorisnikViewModel>> GetKorisniciAsync()
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Get, "api/korisnik"));
        if (!resp.IsSuccessStatusCode) return new();
        return await resp.Content.ReadFromJsonAsync<List<KorisnikViewModel>>(_json) ?? new();
    }

    public async Task<KorisnikViewModel?> GetKorisnikAsync(int id)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Get, $"api/korisnik/{id}"));
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<KorisnikViewModel>(_json);
    }

    public async Task<bool> CreateKorisnikAsync(string ime, string prezime, string email, string lozinka, string tip)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Post, "api/korisnik", new { ime, prezime, email, lozinka, tip }));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateKorisnikAsync(int id, string ime, string prezime, string email, string lozinka, string tip)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Put, $"api/korisnik/{id}", new { ime, prezime, email, lozinka, tip }));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteKorisnikAsync(int id)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Delete, $"api/korisnik/{id}"));
        return resp.IsSuccessStatusCode;
    }

    // Obavijest
    public async Task<List<ObavijestViewModel>> GetObavijestiAsync()
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Get, "api/obavijest"));
        if (!resp.IsSuccessStatusCode) return new();
        return await resp.Content.ReadFromJsonAsync<List<ObavijestViewModel>>(_json) ?? new();
    }

    public async Task<List<ObavijestViewModel>> GetObavijestiByKolegijAsync(int kolegijId)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Get, $"api/obavijest/kolegij/{kolegijId}"));
        if (!resp.IsSuccessStatusCode) return new();
        return await resp.Content.ReadFromJsonAsync<List<ObavijestViewModel>>(_json) ?? new();
    }

    public async Task<ObavijestViewModel?> GetObavijestAsync(int id)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Get, $"api/obavijest/{id}"));
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<ObavijestViewModel>(_json);
    }

    public async Task<bool> CreateObavijestAsync(string naziv, string opis, DateTime datumObjave, DateTime datumIsteka, int kolegijId)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Post, "api/obavijest", new { naziv, opis, datumObjave, datumIsteka, kolegijId }));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateObavijestAsync(int id, string naziv, string opis, DateTime datumObjave, DateTime datumIsteka, int kolegijId)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Put, $"api/obavijest/{id}", new { naziv, opis, datumObjave, datumIsteka, kolegijId }));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteObavijestAsync(int id)
    {
        var resp = await _http.SendAsync(Req(HttpMethod.Delete, $"api/obavijest/{id}"));
        return resp.IsSuccessStatusCode;
    }
}
