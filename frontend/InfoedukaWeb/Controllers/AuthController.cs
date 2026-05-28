using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InfoedukaWeb.Models;
using InfoedukaWeb.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace InfoedukaWeb.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _api;

    public AuthController(ApiService api) => _api = api;

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var token = await _api.LoginAsync(model.Email, model.Lozinka);
        if (token == null)
        {
            ModelState.AddModelError("", "Pogrešan email ili lozinka.");
            return View(model);
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claims = new List<Claim> { new("jwt", token) };
        foreach (var c in jwt.Claims)
        {
            if (c.Type == "role")
                claims.Add(new Claim(ClaimTypes.Role, c.Value));
            else if (c.Type == "email")
                claims.Add(new Claim(ClaimTypes.Email, c.Value));
            else
                claims.Add(c);
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Kolegij");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
}
