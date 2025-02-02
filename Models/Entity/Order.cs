using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models.Entity
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public string Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Time { get; set; }
        [Column(name: "user_id")]
        public string? UserId { get; set; }
    }
}
