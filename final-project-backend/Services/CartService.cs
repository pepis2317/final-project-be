using Azure.Core;
using Entities;
using final_project_backend.Models.Cart;
using final_project_backend.Models.Item;
using final_project_backend.Models.Order;
using Microsoft.EntityFrameworkCore;
using Services;

namespace final_project_backend.Services
{
    public class CartService
    {
        private readonly FinalProjectTrainingDbContext _db;
        private readonly OrderService _orderService;
        private readonly ItemService _itemService;
        public CartService(FinalProjectTrainingDbContext db, ItemService itemService, OrderService orderService)
        {
            _db = db;
            _itemService = itemService;
            _orderService = orderService;
        }
        public async Task<bool> CheckInCart(Guid UserId, Guid ItemId)
        {
            var data = await _db.CartItems.Where(ci => ci.Cart.BuyerId == UserId && ci.ItemId == ItemId && ci.Cart.CompletedAt == null).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;

        }
        public async Task<List<CartItemResponse>> GetIncompleteCartItems(Guid UserId)
        {
            var cartItems = await _db.CartItems.Where(ci => ci.Cart.CartId == _db.Carts.Where(c => c.BuyerId == UserId && c.CompletedAt == null).Select(c => c.CartId).FirstOrDefault()).ToListAsync();
            var result = new List<CartItemResponse>();
            foreach (var cartItem in cartItems)
            {
                var item = await _itemService.GetItemById(cartItem.ItemId);
                result.Add(new CartItemResponse
                {
                    CartItemId = cartItem.CartItemId,
                    CartId = cartItem.CartId,
                    Item = item,
                    Quantity = cartItem.Quantity,
                });
            }
            return result;
        }
        public async Task<CartItemResponse> PostIncompleteCartItem(CartItemRequest request)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(q => q.CompletedAt == null && q.BuyerId == request.UserId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = Guid.NewGuid(),
                    BuyerId = request.UserId,
                    CompletedAt = null
                };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }
            var cartItem = new CartItem
            {
                CartItemId = Guid.NewGuid(),
                CartId = cart.CartId,
                ItemId = request.ItemId,
                Quantity = request.Quantity,
            };
            _db.CartItems.Add(cartItem);
            await _db.SaveChangesAsync();


            return new CartItemResponse
            {
                CartItemId = cartItem.CartItemId,
                CartId = cartItem.CartId,
                Quantity = cartItem.Quantity,
            };
        }
        public async Task<CartItemResponse?> EditIncompleteCartItem(CartItemEditRequest request)
        {
            var data = await _db.CartItems.FirstOrDefaultAsync(q => q.CartItemId == request.CartItemId);
            if (data == null)
            {
                return null;
            }
            data.Quantity = request.Quantity != null ? (int)request.Quantity : data.Quantity;
            if (request.Quantity == null)
            {
                await DeleteIncompleteCartItem(request.CartItemId);
                return null;
            }
            _db.CartItems.Update(data);
            await _db.SaveChangesAsync();
            return new CartItemResponse
            {
                CartItemId = data.CartItemId,
                CartId = data.CartId,
                Quantity = data.Quantity,
            };
        }
        public async Task<string?> DeleteIncompleteCartItem(Guid CartItemId)
        {
            var data = await _db.CartItems.FirstOrDefaultAsync(q => q.CartItemId == CartItemId);
            if (data == null)
            {
                return null;
            }
            _db.CartItems.Remove(data);
            await _db.SaveChangesAsync();
            return "cart item deleted successfully";
        }
        public async Task<CartResponse?> CompleteCart(Guid UserId)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(q => q.CompletedAt == null && q.BuyerId == UserId);
            if (cart == null)
            {
                return null;
            }
            cart.CompletedAt = DateTime.Now;
            _db.Carts.Update(cart);
            await _db.SaveChangesAsync();

            var cartItems = await _db.CartItems.Include(q => q.Item).Where(q => q.CartId == cart.CartId).ToListAsync();
            foreach (var item in cartItems)
            {
                await _orderService.CreateOrderAsync(new CreateOrderRequest
                {
                    BuyerId = UserId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    TotalHarga = item.Item.HargaPerItem ?? 0
                });
            }
            return new CartResponse
            {
                CartId = cart.CartId,
                BuyerId = cart.BuyerId,
                CompletedAt = cart.CompletedAt,
            };
        }

    }
}

