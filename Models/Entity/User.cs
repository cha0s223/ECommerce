using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models.Entity
{
    [Table("Users")]
    public class User
    {
        public User(string id, string? name, string mobile, string password, string? address)
        {
            Id = id;
            Name = name;
            Mobile = mobile;
            Password = password;
            Address = address;
        }

        [Key]
        [Required(ErrorMessage = "账号是必填项")]
        public string Id { get; set; }
        public string? Name { get; set; }
        [Required(ErrorMessage = "电话号码是必填项")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "密码是必填项")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度必须在6到100个字符之间")]
        public string Password { get; set; }
        public string? Address { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Mobile: {Mobile}, Address: {Address}";
        }
    }
}
