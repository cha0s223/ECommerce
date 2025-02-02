using BlazorApp1.Data;
using BlazorApp1.Models.Vo;

namespace BlazorApp1.Service
{
    public class GoodsService
    {
        private readonly BlazorApp1Context _context;
        private readonly ILogger<GoodsService> _logger;

        public GoodsService(BlazorApp1Context context, ILogger<GoodsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<GoodsVo> GetGoodsList()
        {
            var goodsList = _context.Goods.ToList();
            var goodsVoList = new List<GoodsVo>();
            foreach (var goods in goodsList)
            {
                goodsVoList.Add(new GoodsVo
                {
                    Id = goods.Id,
                    Name = goods.Name,
                    Price = goods.Price,
                    Reserve = goods.Reserve,
                    StoreId = goods.StoreId,
                    ImgPath = goods.ImgPath,
                    State = goods.State
                });
            }
            return goodsVoList;
        }

        public GoodsVo GetGoodsById(int id)
        {
            var goods = _context.Goods.Find(id);
            if (goods == null)
            {
                return null;
            }
            return new GoodsVo
            {
                Id = goods.Id,
                Name = goods.Name,
                Price = goods.Price,
                Reserve = goods.Reserve,
                StoreId = goods.StoreId,
                ImgPath = goods.ImgPath,
                State = goods.State
            };
        }
    }
}
