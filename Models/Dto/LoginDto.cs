using BootstrapBlazor.Components;

using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "账号是必填项")]
        public string id { get; set; }
        [Required(ErrorMessage = "密码是必填项")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度必须在6到100个字符之间")]
        public string password { get; set; }
        [Required(ErrorMessage = "角色是必填项")]
        public SelectedItem? role { get; set; }



        public override string ToString()
        {
            //return base.ToString();
            return $"id: {this.id}, password: {this.password}, role: {this.role?.Value}";
        }
    }
}
