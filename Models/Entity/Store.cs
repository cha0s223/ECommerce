using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models.Entity
{
    [Table(name: "Stores")]
    public class Store
    {
        [Key]
        public string Id { get; set; }
        public string? Name { get; set; }

        [Column(name: "master_id")]
        public string? MasterId { get; set; }
        public string Password { get; set; }

        [Column(name: "master_name")]
        public string? MasterName { get; set; }
    }
}
