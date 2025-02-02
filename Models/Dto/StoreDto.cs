using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models.Dto
{
    public class StoreDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }

        public string? MasterId { get; set; }
        public string Password { get; set; }

        public string? MasterName { get; set; }
    }
}
