using Entities;
using final_project_backend.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Services
{
    public class UserService
    {
        private readonly FinalProjectTrainingDbContext _db;
        private readonly BlobStorageService _blobStorageService;
        public UserService(FinalProjectTrainingDbContext db , BlobStorageService blobStorageService)
        {
            _db = db;
            _blobStorageService = blobStorageService;
        }
        public async Task<List<UserResponseModel>> GetAllUsers()
        {
            var users = await _db.Users.ToListAsync();
            var pfpFilenames = users.Select(u => u.UserProfile).ToList();
            var pfpUrls = await Task.WhenAll(pfpFilenames.Select(PfpHelper));
            var result = users.Select((user, index) => new UserResponseModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserBalance = user.UserBalance,
                UserProfile = pfpUrls[index],
                UserPhoneNumber = user.UserPhoneNumber,
                UserEmail = user.UserEmail,
                UserAddress = user.UserAddress, 
                BirthDate = user.BirthDate,
                Gender = user.Gender
            }).ToList();
            return result;
        }

        public async Task<User?> UpdateUserById(Guid userId, User updatedUser)
        {
            var existingUser = await _db.Users.FindAsync(userId);
            if (existingUser == null) return null;

            existingUser.UserName = string.IsNullOrWhiteSpace(updatedUser.UserName) ? existingUser.UserName : updatedUser.UserName;
            existingUser.UserPassword = string.IsNullOrWhiteSpace(updatedUser.UserPassword) ? existingUser.UserPassword : updatedUser.UserPassword;
            existingUser.UserBalance = updatedUser.UserBalance ?? existingUser.UserBalance;
            existingUser.UserProfile = string.IsNullOrWhiteSpace(updatedUser.UserProfile) ? existingUser.UserProfile : updatedUser.UserProfile;
            existingUser.UserPhoneNumber = string.IsNullOrWhiteSpace(updatedUser.UserPhoneNumber) ? existingUser.UserPhoneNumber : updatedUser.UserPhoneNumber;
            existingUser.UserEmail = string.IsNullOrWhiteSpace(updatedUser.UserEmail) ? existingUser.UserEmail : updatedUser.UserEmail;
            existingUser.UserAddress = string.IsNullOrWhiteSpace(updatedUser.UserAddress) ? existingUser.UserAddress : updatedUser.UserAddress;
            existingUser.BirthDate = updatedUser.BirthDate ?? existingUser.BirthDate;
            existingUser.Gender = string.IsNullOrWhiteSpace(updatedUser.Gender) ? existingUser.Gender : updatedUser.Gender;

            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();
            return existingUser;
        }

        public async Task<string?> UploadPfp(Guid UserId, Stream imageStream, string fileName, string contentType)
        {
            var user = await _db.Users.FirstOrDefaultAsync(q => q.UserId == UserId);
            if (user == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(user.UserProfile))
            {
                await _blobStorageService.DeletePfpAsync(user.UserProfile, "user-pfps");
            }
            string imageUrl = await _blobStorageService.UploadPfpAsync(imageStream, fileName, contentType, "user-pfps");
            user.UserProfile = fileName;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return imageUrl;
        }
        private async Task<string?> PfpHelper(string? fileName)
        {
            string? imageUrl = await _blobStorageService.GetTemporaryImageUrl(fileName, "user-pfps");
            return imageUrl;
        }
    }
}
