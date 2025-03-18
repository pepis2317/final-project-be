using Entities;
using final_project_backend.Models.Shop;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace final_project_backend.Services
{
    public class ShopService
    {
        private readonly FinalProjectTrainingDbContext _db;
        public ShopService(FinalProjectTrainingDbContext db)
        {
            _db = db;
        }
        public async Task<List<ShopModel>> GetAllShops()
        {
            var shops = await _db.Shops.Include(q => q.Owner).Select(s => new ShopModel {
                ShopId = s.ShopId,
                ShopName = s.ShopName,
                OwnerId = s.OwnerId,
                Description = s.Description,
                Rating = s.Rating,
                Address = s.Address,
                CreatedAt = s.CreatedAt
            }).ToListAsync();
            return shops;
        }
        public async Task<ShopModel?>GetShopById(Guid ShopId)
        {
            //6dc05673-f42a-4e38-8bbd-4ce980fb4139
            var shop = await _db.Shops.Include(q=>q.Owner).FirstOrDefaultAsync(q => q.ShopId == ShopId);
            if (shop != null)
            {
                var data = new ShopModel
                {
                    ShopId = shop.ShopId,
                    ShopName = shop.ShopName,
                    OwnerId = shop.OwnerId,
                    Description = shop.Description,
                    Rating = shop.Rating,
                    Address = shop.Address,
                    CreatedAt = shop.CreatedAt
                };
                return data;
            }
            return null;
            
        }
        public async Task<ShopModel> CreateShop(CreateShopRequest request)
        {
            var shop = new Shop
            {
                ShopName = request.ShopName,
                OwnerId = request.OwnerId,
                Description = request.Description,
                Address = request.Address
            };
            _db.Shops.Add(shop);
            await _db.SaveChangesAsync();
            return new ShopModel
            {
                ShopId = shop.ShopId,
                ShopName = shop.ShopName,
                OwnerId = shop.OwnerId,
                Description = shop.Description,
                Rating = shop.Rating,
                Address = shop.Address,
                CreatedAt = shop.CreatedAt
            };
        }
        public async Task<ShopModel?> EditShop(Guid ShopId, EditShopRequest request)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(q=>q.ShopId == ShopId);
            if(shop != null)
            {
                shop.ShopName = string.IsNullOrEmpty(request.ShopName)?shop.ShopName : request.ShopName;
                shop.Description = string.IsNullOrEmpty(request.Description)?shop.Description : request.Description;
                shop.Address = string.IsNullOrEmpty(request.Address)?shop.Address : request.Address;
                _db.Shops.Update(shop);
                await _db.SaveChangesAsync();
                return new ShopModel
                {
                    ShopId = shop.ShopId,
                    ShopName = shop.ShopName,
                    OwnerId = shop.OwnerId,
                    Description = shop.Description,
                    Rating = shop.Rating,
                    Address = shop.Address,
                    CreatedAt = shop.CreatedAt
                };
            }
            return null;
        }
        public async Task<string?> DeleteShop(Guid ShopId)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(q => q.ShopId == ShopId);
            if(shop != null)
            {
                _db.Shops.Remove(shop);
                await _db.SaveChangesAsync();
                return "Shop deleted successfully";
            }
            return null;
        }
    }
}
