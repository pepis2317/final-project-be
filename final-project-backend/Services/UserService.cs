using Entities;
using final_project_backend.Models.Users;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace final_project_backend.Services
{
    public class UserService
    {
        private readonly FinalProjectTrainingDbContext _db;
        private readonly BlobStorageService _blobStorageService;
        private readonly JwtService _jwtService;
        private readonly IDataProtector _protector;
        public UserService(FinalProjectTrainingDbContext db , BlobStorageService blobStorageService, IDataProtectionProvider provider, JwtService jwtService)
        {
            _db = db;
            _blobStorageService = blobStorageService;
            _protector = provider.CreateProtector("CredentialsProtector");
            _jwtService = jwtService;
        }
        public async Task<UserResponse?> Get(Guid UserId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(q => q.UserId == UserId);
            if (user == null)
            {
                return null;
            }
            var pfp = await PfpHelper(user.UserProfile);
            return new UserResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserBalance = user.UserBalance,
                UserProfile = pfp,
                UserPhoneNumber = user.UserPhoneNumber,
                UserEmail = user.UserEmail,
                UserAddress = user.UserAddress,
                BirthDate = user.BirthDate,
                Gender = user.Gender
            };
        }
        public async Task<UserResponse> Register(RegisterRequest request)
        {
            var user = new User
            {
                UserName = request.UserName,
                UserEmail = request.UserEmail,
                UserPassword = _protector.Protect(request.UserPassword),
                UserPhoneNumber = request.UserPhoneNumber,
                UserAddress = request.UserAddress,
                BirthDate = request.BirthDate,
                Gender = request.Gender
            };
            _db.Users.Add(user);
            var pfp = await PfpHelper(user.UserProfile);
            await _db.SaveChangesAsync();
            return new UserResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserBalance = user.UserBalance,
                UserProfile = pfp,
                UserPhoneNumber = user.UserPhoneNumber,
                UserEmail = user.UserEmail,
                UserAddress = user.UserAddress,
                BirthDate = user.BirthDate,
                Gender = user.Gender
            };
        }
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        public async Task<LoginResponse?> Login(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(q => q.UserEmail == email);
            if(user == null)
            {
                return null;
            }
            if (_protector.Unprotect(user.UserPassword) != password)
            {
                return null;
            }
            var token = _jwtService.GenerateToken(user.UserId);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }
        public async Task<LoginResponse?> RefreshToken(string refreshToken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(q => q.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return null;
            }
            var newToken = _jwtService.GenerateToken(user.UserId);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return new LoginResponse
            {
                Token = newToken,
                RefreshToken = user.RefreshToken
            };
        }
        public async Task<List<UserResponse>> GetAllUsers()
        {
            var users = await _db.Users.ToListAsync();
            var pfpFilenames = users.Select(u => u.UserProfile).ToList();
            var pfpUrls = await Task.WhenAll(pfpFilenames.Select(PfpHelper));
            var result = users.Select((user, index) => new UserResponse
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

        public async Task<UserResponse?> UpdateUserById(UserUpdateRequest updatedUser)
        {
            var existingUser = await _db.Users.FindAsync(updatedUser.UserId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.UserName = string.IsNullOrWhiteSpace(updatedUser.UserName) ? existingUser.UserName : updatedUser.UserName;
            existingUser.UserPassword = string.IsNullOrWhiteSpace(updatedUser.UserPassword) ? existingUser.UserPassword : _protector.Protect(updatedUser.UserPassword);
            existingUser.UserBalance = updatedUser.UserBalance ?? existingUser.UserBalance;
            existingUser.UserPhoneNumber = string.IsNullOrWhiteSpace(updatedUser.UserPhoneNumber) ? existingUser.UserPhoneNumber : updatedUser.UserPhoneNumber;
            existingUser.UserEmail = string.IsNullOrWhiteSpace(updatedUser.UserEmail) ? existingUser.UserEmail : updatedUser.UserEmail;
            existingUser.UserAddress = string.IsNullOrWhiteSpace(updatedUser.UserAddress) ? existingUser.UserAddress : updatedUser.UserAddress;
            existingUser.BirthDate = updatedUser.BirthDate ?? existingUser.BirthDate;
            existingUser.Gender = string.IsNullOrWhiteSpace(updatedUser.Gender) ? existingUser.Gender : updatedUser.Gender;

            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();
            var pfp = await PfpHelper(existingUser.UserProfile);
            return new UserResponse
            {
                UserId = existingUser.UserId,
                UserName = existingUser.UserName,
                UserBalance = existingUser.UserBalance,
                UserProfile = pfp,
                UserPhoneNumber = existingUser.UserPhoneNumber,
                UserEmail = existingUser.UserEmail,
                UserAddress = existingUser.UserAddress,
                BirthDate = existingUser.BirthDate,
                Gender = existingUser.Gender
            };
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
            string imageUrl = await _blobStorageService.UploadImageAsync(imageStream, fileName, contentType, "user-pfps", 200);
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
