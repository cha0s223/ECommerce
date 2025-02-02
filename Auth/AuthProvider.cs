using BlazorApp1.Models.Dto;
using BlazorApp1.Models.Vo;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorApp1.Auth
{
    public class AuthProvider : AuthenticationStateProvider
    {
        private readonly HttpClient HttpClient;
        private ProtectedSessionStorage protectedSessionStorage { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public AuthProvider(HttpClient httpClient, ProtectedSessionStorage protectedSessionStorage)
        {
            HttpClient = httpClient;
            this.protectedSessionStorage = protectedSessionStorage;

        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // 从本地存储中读取 Token
                var tokenResult = await protectedSessionStorage.GetAsync<string>($"ECommerce_Token");
                var token = tokenResult.Success ? tokenResult.Value : null;

                if (!string.IsNullOrEmpty(token))
                {
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                    var response = await HttpClient.GetAsync($"api/Auth/GetUser");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<AuthRoleVo>();
                        if (result?.Id != null)
                        {
                            var identity = new ClaimsIdentity(
                            [
                                new Claim(ClaimTypes.Name, result.Id),
                                new Claim(ClaimTypes.Role,result.Role),
                                new Claim("Token", result.Token)

                            ], "apiauth");
                            var user = new ClaimsPrincipal(identity);
                            return new AuthenticationState(user);
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // Token 过期或无效
                        MarkUserAsLoggedOut();
                        return new AuthenticationState(new ClaimsPrincipal());
                    }
                }
                MarkUserAsLoggedOut();
                return new AuthenticationState(new ClaimsPrincipal());
                
            }
            catch (Exception ex)
            {
                // 添加日志记录
                System.Console.WriteLine($"Error fetching user: {ex.Message}");
                MarkUserAsLoggedOut();
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public void MarkUserAsAuthenticated(AuthVo authVo,SelectedItem role)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authVo.Token);
            UserId = authVo.Id;
            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, authVo.Id),
                new Claim(ClaimTypes.Role, role.Value),
                new Claim("AuthUser", "Confirm")
            ], "apiauth");
            var user = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);


            //慈湖可以可以将Token存储在本地存储中，实现页面刷新无需登录
            // 将Token存储在本地存储中，实现页面刷新无需登录
            System.Console.WriteLine($"Storing Token: {authVo.Token}");
            protectedSessionStorage.SetAsync($"ECommerce_Token", authVo.Token);
        }
        public void MarkUserAsLoggedOut()
        {
            HttpClient.DefaultRequestHeaders.Authorization = null;
            UserId = null;

            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
            // 清除本地存储中的 Token
            protectedSessionStorage.DeleteAsync($"ECommerce_Token");
        }
    }
}
