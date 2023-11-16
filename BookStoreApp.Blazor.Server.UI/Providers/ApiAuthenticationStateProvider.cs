using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreApp.Blazor.Server.UI.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage )
    {
        _localStorage = localStorage;
        _jwtSecurityTokenHandler =  new JwtSecurityTokenHandler();
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var savedToken = await _localStorage.GetItemAsync<string>("accessToken");

        //kad nema tokena - vracamo empty claimsPrincipal
        if (savedToken == null) 
        {
            return new AuthenticationState(user);
        }

        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);

        if (tokenContent.ValidTo < DateTime.Now)
        {
            return new AuthenticationState(user);
        }

        //getting claims
        var claims = tokenContent.Claims;
        //updating claimsPrincipal with claims
        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        return new AuthenticationState(user);
    }

    public async Task LoggedIn()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("accessToken");
        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
        var claims = tokenContent.Claims;
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void LoggedOut()
    {
        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(nobody));
        NotifyAuthenticationStateChanged(authState);
    }
}
