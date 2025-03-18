using Microsoft.EntityFrameworkCore;
using Entities;
using final_project_backend.Models.Item;

namespace Services
{
    public class ItemService
    {
        private readonly FinalProjectTrainingDbContext _context;

        public ItemService(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemResponse>> GetAllItemsAsync()
        {
            return await _context.Items
                .Select(item => new ItemResponse
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    ItemDesc = item.ItemDesc,
                    Quantity = item.Quantity,
                    TotalHarga = item.TotalHarga
                })
                .ToListAsync();
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
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                TotalHarga = item.TotalHarga
            };
        }
        public async Task<List<ItemResponse>> GetItemsFromShop(Guid ShopId)
        {
            var items = await _context.Items.Where(q=>q.ShopId == ShopId).Select(item => new ItemResponse
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                TotalHarga = item.TotalHarga
            }).ToListAsync();
            return items;
        }
        public async Task<ItemResponse> CreateItem(Guid ShopId, CreateItemRequest request)
        {
            var item = new Item
            {
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
