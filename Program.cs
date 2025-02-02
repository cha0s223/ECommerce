using BlazorApp1.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlazorApp1.Data;
using BlazorApp1.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlazorApp1.Models.Entity;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp1.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<BlazorApp1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlazorApp1Context") ?? throw new InvalidOperationException("Connection string 'BlazorApp1Context' not found.")));
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer( options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10MB
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7126/") });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBootstrapBlazor();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddAuthorizationCore(option =>
{
    option.AddPolicy("AuthUser", policy => policy.RequireClaim("Confirm"));
    option.AddPolicy("buyer", policy => policy.RequireClaim(ClaimTypes.Role, "buyer"));
    option.AddPolicy("seller", policy => policy.RequireClaim(ClaimTypes.Role, "seller"));

});

builder.Services.AddLogging();


builder.Services.AddControllers();
builder.Services.AddScoped<GoodsService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StoreService>();
builder.Services.AddSingleton<UserStroge>();
builder.Services.AddSingleton<SellerStroge>();
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();



app.UseAntiforgery();
app.MapControllers(); // 确保映射了控制器
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseStatusCodePagesWithRedirects("/Error");
app.Run();
