using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models.Vo
{
    public class UserVo
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        [Required(ErrorMessage = "电话号码是必填项")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "密码是必填项")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度必须在6到100个字符之间")]
        public string Password { get; set; }
        public string? Address { get; set; }

        
    }
}
