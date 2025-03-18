using Microsoft.EntityFrameworkCore;
using Entities;
using final_project_backend.Models.Users;

namespace Services
{
    public class ItemService
    {
        private readonly FinalProjectTrainingDbContext _context;

        public ItemService(FinalProjectTrainingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SeeItems>> GetAllItemsAsync()
        {
            return await _context.Items
                .Select(item => new SeeItems
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    ItemDesc = item.ItemDesc,
                    Quantity = item.Quantity,
                    TotalHarga = item.TotalHarga
                })
                .ToListAsync();
        }
    }
}
