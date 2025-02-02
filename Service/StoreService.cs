
using BlazorApp1.Data;
using BlazorApp1.Models.Dto;
using BlazorApp1.Models.Vo;
using BlazorApp1.Models.Entity;

namespace BlazorApp1.Service
{
    public class StoreService
    {
        private readonly BlazorApp1Context _context;
        private readonly ILogger<StoreService> _logger;
        public StoreService(BlazorApp1Context context, ILogger<StoreService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<StoreVo>> GetMyStore(string userId)
        {
            var storeList = _context.Store.Where(store => store.MasterId == userId).ToList();
            var storeVoList = new List<StoreVo>();
            foreach (var store in storeList)
            {
                storeVoList.Add(new StoreVo
                {
                    Id = store.Id,
                    Name = store.Name,
                    MasterId = store.MasterId,
                    Password = store.Password,
                    MasterName = store.MasterName
                });
            }
            return storeVoList;
        }

        public async Task<bool> UpdateMyStore(StoreDto storeDto)
        {
            if (storeDto == null)
            {
                return false;
            }
            var store =   _context.Store.Find(storeDto.Id);
            if(store is not null)
            {
                store.Name = storeDto.Name;
                _context.Store.Update(store);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<GoodsVo>> GetMyGoods(string storeId)
        {
            var goodsList = _context.Goods.Where(goods => goods.StoreId == storeId).ToList();
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

        public async Task<bool> AddGoods(GoodsDto goodsDto)
        {
            var goodsEntity = await _context.Goods.FindAsync(goodsDto.Id);
            if (goodsEntity != null)
            {
                // 商品已存在
                return false;
            }
            Goods goods = new Goods
            {
                Name = goodsDto.Name,
                Price = goodsDto.Price,
                Reserve = goodsDto.Reserve,
                StoreId = goodsDto.StoreId,
                ImgPath = goodsDto.ImgPath,
                State = goodsDto.State
            };
            _context.Goods.Add(goods);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> UpdateGoods(GoodsDto goodsDto)
        {
            if (goodsDto == null)
            {
                return false;
            }
            if(goodsDto.Reserve < 0)
            {
                return false;
            }
            var goods = _context.Goods.Find(goodsDto.Id);
            if (goods is not null)
            {
                goods.Name = goodsDto.Name;
                goods.Price = goodsDto.Price;
                goods.Reserve = goodsDto.Reserve;
                goods.StoreId = goodsDto.StoreId;
                goods.ImgPath = goodsDto.ImgPath;
                goods.State = goodsDto.State;
                _context.Goods.Update(goods);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteGoods(int goodsId)
        {
            var goods = _context.Goods.Find(goodsId);
            if (goods is not null)
            {
                _context.Goods.Remove(goods);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
