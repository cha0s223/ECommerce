using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models.Entity
{
    [Table("SubOrders")]
    public class SubOrder
    {
        [Key]
        public string Id { get; set; }
        [Column(name: "order_id")]
        public string? OrderId { get; set; }
        [Column(name: "goods_id")]
        public int? GoodsId { get; set; }
        [Column(name: "goods_price")]
        public double? GoodsPrice { get; set; }
    }
}
