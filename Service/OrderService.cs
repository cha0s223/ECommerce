using BlazorApp1.Data;
using BlazorApp1.Models.Entity;
using BlazorApp1.Models.Vo;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Service
{
    public class OrderService
    {
        private readonly BlazorApp1Context _context;
        private readonly ILogger<GoodsService> _logger;
        public OrderService(BlazorApp1Context context, ILogger<GoodsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<OrderVo> GetOrderList()
        {
            var orderList = _context.Order.ToList();
            var orderVoList = new List<OrderVo>();
            foreach (var order in orderList)
            {
                orderVoList.Add(new OrderVo
                {
                    Id = order.Id,
                    Time = order.Time,
                    UserId = order.UserId
                });
            }
            return orderVoList;
        }

        public async Task<List<Order>> GetOrders(string userId)
        {
            var orders =from order in _context.Order
                         where order.UserId == userId
                         select order;
            return [..orders];
        }

        public async Task<List<SubOrder>> GetSubOrdersList(string orderId)
        {
            var subOrders=from subOrder in _context.SubOrders
                          where subOrder.OrderId == orderId
                          select subOrder;
            return [.. subOrders];
        }

        public async Task<List<SubOrder>> GetOrderList(string storeId)
        {
            var subOrders = from subOrder in _context.SubOrders
                            join goods in _context.Goods on subOrder.GoodsId equals goods.Id
                            where goods.StoreId == storeId
                            select subOrder;
            return [.. subOrders];
        }

        public async Task<Order> GetOrderById(string orderId)
        {
            return await _context.Order.FindAsync(orderId);
        }


        public async Task<bool> CommitCartList(string userId, List<CartItem> cartItems)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string orderId = Guid.NewGuid().ToString();
                _context.Order.Add(new Order
                {
                    Id = orderId,
                    Time = DateTime.Now,
                    UserId = userId
                });
                foreach (var cartItem in cartItems)
                {
                    var goods = await _context.Goods.FindAsync(cartItem.ProductId);
                    if (goods == null)
                    {
                        // 商品不存在，回滚事务
                        await transaction.RollbackAsync();
                        return false;
                    }
                    if (goods.Reserve < cartItem.Number)
                    {
                        // 库存不足，回滚事务
                        await transaction.RollbackAsync();
                        return false;
                    }
                    goods.Reserve -= cartItem.Number;
                    _context.Goods.Update(goods);
                    _context.SubOrders.Add(new SubOrder
                    {
                        Id = Guid.NewGuid().ToString(),
                        OrderId = orderId,
                        GoodsId = cartItem.ProductId,
                        GoodsPrice = Math.Round((double)(cartItem.Number * goods.Price),2)
                    });
                }
                // 保存更改
                await _context.SaveChangesAsync();

                // 提交事务
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                // 发生异常，回滚事务
                await transaction.RollbackAsync();
                // 可以在此记录日志或处理异常
                _logger.LogError(ex, "提交购物车时发生异常");
                return false;
            }
        }
    }
}
