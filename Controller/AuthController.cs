using BlazorApp1.Data;
using BlazorApp1.Models.Dto;
using BlazorApp1.Models.Entity;
using BlazorApp1.Models.Vo;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorApp1.Controller
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly BlazorApp1Context _context;

        public AuthController(IOptions<JwtSettings> jwtSettings, BlazorApp1Context context)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }

        //登录
        [HttpPost]
        public AuthVo Login(LoginDto dto)
        {
            var user = _context.User.Find(dto.id);
            if (user == null || user.Password != dto.password)
            {
                return new() { Id = dto.id, Token = null };
            }
            var jwtToken = GetToken(dto);
            return new() { Id = dto.id, Token = jwtToken };

            //var store = _context.Store.Find(dto.id);
            //if (store == null || store.Password != dto.password)
            //{
            //    return new() { Id = dto.id, Token = null };
            //}

        }

        [HttpGet]
        public AuthRoleVo GetUser()
        {
            AuthRoleVo authRoleVo = new();
            if (User.Identity.IsAuthenticated)
            {
                var name = User.FindFirst(ClaimTypes.Name)?.Value;
                var password = User.FindFirst("Password")?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                SelectedItem selectedItem = role == "buyer" ? new SelectedItem("buyer", "买家") : new SelectedItem("seller", "卖家");
                var user = _context.User.Select(i => i.Id == name && i.Password == password);
                if (user != null)
                {
                    authRoleVo = new AuthRoleVo { Id = name, Token = GetToken(new LoginDto { id = name, password = password, role = selectedItem }),Role = role };
                }
            }
            return authRoleVo;
        }

        private string GetToken(LoginDto dto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.id),
                new Claim("Password", dto.password),
                new Claim(ClaimTypes.Role, dto.role.Value)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var expires = DateTime.Now.AddMinutes(30);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
