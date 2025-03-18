using Microsoft.EntityFrameworkCore;
using Entities;
using final_project_backend.Models.Item;
using final_project_backend.Services;

namespace Services
{
    public class ItemService
    {
        private readonly FinalProjectTrainingDbContext _context;
        private readonly ProductImageService _imageService;
        public ItemService(FinalProjectTrainingDbContext context, ProductImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<List<ItemResponse>> GetAllItemsAsync()
        {
            var items = await _context.Items.ToListAsync();

            var tasks = items.Select(async item => new ItemResponse
            {
                ItemId = item.ItemId,
                ShopId = item.ShopId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                TotalHarga = item.TotalHarga,
            });

            return (await Task.WhenAll(tasks)).ToList();
        }
        public async Task<ItemResponse?> GetItemById(Guid ItemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(q => q.ItemId  == ItemId);
            if(item == null)
            {
                return null;
            }
            return new ItemResponse
            {
                ItemId = item.ItemId,
                ShopId = item.ShopId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                TotalHarga = item.TotalHarga,
            };
        }
        public async Task<List<ItemResponse>> GetItemsFromShop(Guid ShopId)
        {
            var items = await _context.Items.Where(q => q.ShopId == ShopId).ToListAsync();

            var itemIds = items.Select(item => item.ItemId).ToList();

            var result = items.Select((item, index) => new ItemResponse
            {
                ItemId = item.ItemId,
                ShopId = item.ShopId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                TotalHarga = item.TotalHarga,
            }).ToList();
            return result;
        }

        public async Task<ItemResponse> CreateItem(Guid ShopId, CreateItemRequest request)
        {
            Guid itemId = Guid.NewGuid();

            var item = new Item
            {
                ItemId = itemId,
                ItemName = request.ItemName,
                ItemDesc = request.ItemDesc,
                Quantity = request.Quantity,
                ShopId = ShopId,
                TotalHarga = request.TotalHarga
            };
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return new ItemResponse
            {
                ItemId = item.ItemId,
                ShopId = ShopId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                TotalHarga = item.TotalHarga
            };
        }
        public async Task<ItemResponse?>EditItem(Guid ItemId, EditItemRequest request)
        {
            var item = await _context.Items.FirstOrDefaultAsync(q=>q.ItemId ==  ItemId);
            if(item == null)
            {
                return null;
            }
            item.ItemName = string.IsNullOrEmpty(request.ItemName)? item.ItemName: request.ItemName;
            item.ItemDesc = string.IsNullOrEmpty(request.ItemDesc)? item.ItemDesc: request.ItemDesc;
            item.Quantity = request.Quantity == null? item.Quantity: request.Quantity;
            item.TotalHarga = request.TotalHarga == null? item.TotalHarga: request.TotalHarga;
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return new ItemResponse
            {
                ItemId = item.ItemId,
                ShopId = item.ShopId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                TotalHarga = item.TotalHarga
            };
        }
        public async Task<string?> DeleteItem(Guid ItemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(q => q.ItemId == ItemId);
            if( item == null)
            {
                return null;    
            }
            _context.Items.Remove(item);
            return item.ItemName;
        }
    }
}
