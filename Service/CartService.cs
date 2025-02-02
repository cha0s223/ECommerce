using BlazorApp1.Models.Vo;

namespace BlazorApp1.Service
{
    public class CartService
    {
        private readonly List<CartItem> cartItems= new List<CartItem>();
        public void AddToCart(GoodsVo product, int num)
        {
            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Number += num;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price ?? 0,
                    Number = num
                });
            }
        }

        public void RemoveFromCart(int productId)
        {
            var item = cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cartItems.Remove(item);
            }
        }

        public List<CartItem> GetCartItems()
        {
            return cartItems;
        }

        public void SetCartItems(List<CartItem>? items)
        {
            cartItems.Clear();
            cartItems.AddRange(items!);
        }

        public void ClearCart()
        {
            cartItems.Clear();
        }

        public double GetTotalPrice()
        {
            return cartItems.Sum(item => item.Price * item.Number);
        }
    }
}
