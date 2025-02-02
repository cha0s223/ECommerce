namespace BlazorApp1.Models.Entity
{
    public class GoodsOrderList
    {
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public DateTime? OrderTime { get; set; }
        public double? TotalPrice { get; set; }
        public List<SubOrder> SubOrders { get; set; }
        
    }
}
