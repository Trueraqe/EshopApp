using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Security.Cryptography;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _js;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(IJSRuntime js)
    {
        _js = js;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userId = await _js.InvokeAsync<string>("localStorage.getItem", "userId");

            var userRole = await _js.InvokeAsync<string>("localStorage.getItem", "userRole");

            if (string.IsNullOrEmpty(userId))
                return new AuthenticationState(_anonymous);

            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, userRole)
            }, "CustomAuth");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }
    public async Task MarkUserAsAuthenticated(string userId, string userRole)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", "userId", userId);

        await _js.InvokeVoidAsync("localStorage.setItem", "userRole", userRole);

        var identity = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, userRole)
        }, "CustomAuth", ClaimTypes.Name, ClaimTypes.Role);

        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "userId");
        await _js.InvokeVoidAsync("localStorage.removeItem", "userRole");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    public async Task<int?> GetCurrentUserId()
    {
        var state = await GetAuthenticationStateAsync();

        var user = state.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (int.TryParse(user, out var id))
        {
            return id;
        }
        return null;
    }
    public async Task SetColorUI(string colorUI)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", "colorUI", colorUI);
    }
    public async Task<string> GetColorUI()
    {
        return await _js.InvokeAsync<string>("localStorage.getItem", "colorUI");
    }
    public async Task<string> ResetColorUI()
    {
        return await _js.InvokeAsync<string>("localStorage.setItem", "colorUI", "null");
    }

}
