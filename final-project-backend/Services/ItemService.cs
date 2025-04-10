﻿using Microsoft.EntityFrameworkCore;
using Entities;
using final_project_backend.Models.Item;
using final_project_backend.Services;
using System.Net;

namespace Services
{
    public class ItemService
    {
        private readonly FinalProjectTrainingDbContext _context;
        private readonly BlobStorageService _blobStorageService;
        public ItemService(FinalProjectTrainingDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }
        private async Task<string?> ImageHelper(string? fileName)
        {
            string? imageUrl = await _blobStorageService.GetTemporaryImageUrl(fileName, "item-images");
            return imageUrl;
        }
        public async Task<List<ItemResponse>?> GetItemsFromQuery(Guid? ShopId, string? searchTerm)
        {
            var query = _context.Items.Include(q => q.Shop).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(q =>
                    q.ItemName.ToLower().Contains(searchTerm) ||
                    q.ItemDesc.ToLower().Contains(searchTerm) ||
                    q.Shop.ShopName.ToLower().Contains(searchTerm)
                );
            }

            if (ShopId != null)
            {
                query = query.Where(q=>q.ShopId == ShopId);
            }
            var items = await query.ToListAsync();
            var result = new List<ItemResponse>();
            foreach (var item in items)
            {
                var thumbnailUrl = await GetThumbnail(item.ItemId);
                result.Add(new ItemResponse
                {
                    ItemId = item.ItemId,
                    ShopId = item.ShopId,
                    ItemName = item.ItemName,
                    ItemDesc = item.ItemDesc,
                    Quantity = item.Quantity,
                    HargaPerItem = item.HargaPerItem,
                    Thumbnail = thumbnailUrl
                });

            }
            return result;
        }
        public async Task<string?> GetThumbnail(Guid ItemId)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(q => q.ItemId == ItemId && q.IsPrimary == "true");
            if (image == null)
            {
                return null;
            }
            var thumbnail = await ImageHelper(image.Image);
            return thumbnail;
        }
        public async Task<List<ItemResponse>> GetAllItemsAsync()
        {
            var items = await _context.Items.ToListAsync();
            var result = new List<ItemResponse>();
            foreach (var item in items)
            {
                var thumbnailUrl = await GetThumbnail(item.ItemId);
                result.Add(new ItemResponse
                {
                    ItemId = item.ItemId,
                    ShopId = item.ShopId,
                    ItemName = item.ItemName,
                    ItemDesc = item.ItemDesc,
                    Quantity = item.Quantity,
                    HargaPerItem = item.HargaPerItem,
                    Thumbnail = thumbnailUrl
                });

            }
            return result;
        }
        public async Task<ItemResponse?> GetItemById(Guid ItemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(q => q.ItemId  == ItemId);
            if(item == null)
            {
                return null;
            }
            var thumbnailUrl = await GetThumbnail(item.ItemId);
            return new ItemResponse
            {
                ItemId = item.ItemId,
                ShopId = item.ShopId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                HargaPerItem = item.HargaPerItem,
                Thumbnail = thumbnailUrl
            };
        }
        public async Task<List<ItemResponse>> GetItemsFromShop(Guid ShopId)
        {
            var items = await _context.Items.Where(q => q.ShopId == ShopId).ToListAsync();
            var result = new List<ItemResponse>();
            foreach(var item in items)
            {
                var thumbnailUrl = await GetThumbnail(item.ItemId);
                result.Add(new ItemResponse
                {
                    ItemId = item.ItemId,
                    ShopId = item.ShopId,
                    ItemName = item.ItemName,
                    ItemDesc = item.ItemDesc,
                    Quantity = item.Quantity,
                    HargaPerItem = item.HargaPerItem,
                    Thumbnail = item.Thumbnail
                });
            }
            return result;


        }

        public async Task<ItemResponse> CreateItem(CreateItemRequest request)
        {
            Guid itemId = Guid.NewGuid();

            var item = new Item
            {
                ItemId = itemId,
                ItemName = request.ItemName,
                ItemDesc = request.ItemDesc,
                Quantity = request.Quantity,
                ShopId = request.ShopId,
                HargaPerItem = request.HargaPerItem
            };
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return new ItemResponse
            {
                ItemId = item.ItemId,
                ShopId = item.ShopId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                HargaPerItem = item.HargaPerItem
            };
        }
        public async Task<ItemResponse?>EditItem(EditItemRequest request)
        {
            var item = await _context.Items.FirstOrDefaultAsync(q=>q.ItemId ==  request.ItemId);
            if(item == null)
            {
                return null;
            }
            item.ItemName = string.IsNullOrEmpty(request.ItemName)? item.ItemName: request.ItemName;
            item.ItemDesc = string.IsNullOrEmpty(request.ItemDesc)? item.ItemDesc: request.ItemDesc;
            item.Quantity = request.Quantity == null? item.Quantity: request.Quantity;
            item.HargaPerItem = request.HargaPerItem == null? item.HargaPerItem: request.HargaPerItem;
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return new ItemResponse
            {
                ItemId = item.ItemId,
                ShopId = item.ShopId,
                ItemName = item.ItemName,
                ItemDesc = item.ItemDesc,
                Quantity = item.Quantity,
                HargaPerItem = item.HargaPerItem,
                Thumbnail = item.Thumbnail
            };
        }
        public async Task<string?> DeleteItem(Guid ItemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(q => q.ItemId == ItemId);
            if (item == null)
            {
                return null;
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return item.ItemName;
        }
    }
}
