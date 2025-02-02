using BlazorApp1.Data;
using BlazorApp1.Models.Dto;
using BlazorApp1.Models.Entity;
using BlazorApp1.Models.Vo;

namespace BlazorApp1.Service
{
    public class UserService
    {

        private readonly BlazorApp1Context _context;
        private readonly ILogger<GoodsService> _logger;
        public UserService(BlazorApp1Context context, ILogger<GoodsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddUser(User user)
        {
            var userEntity = await _context.User.FindAsync(user.Id);
            if (userEntity != null)
            {
                // 用户已存在
                return false;
            }
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<UserVo> GetMyInfo(string id )
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                // 用户不存在
                return null;
            }
            return new UserVo
            {
                Id = user.Id,
                Name = user.Name,
                Mobile = user.Mobile,
                Address = user.Address
            };
        }

        public async Task<bool> UpdateMyInfo(UserDto userDto,string id)
        {
            if (userDto.Id==null|| userDto.Password==null|| userDto.Mobile==null)
            {
                return false;
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                // 用户不存在
                return false;
            }
            User oldUser = new User(
                id: user.Id,
                name: user.Name,
                mobile: user.Mobile,
                password: user.Password,
                address: user.Address
                );
            if (oldUser == user)
            {
                return true;
            }
            user.Name = userDto.Name;
            user.Mobile = userDto.Mobile;
            user.Address = userDto.Address;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
