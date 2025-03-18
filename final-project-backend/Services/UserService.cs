using Entities;
using Microsoft.EntityFrameworkCore;

namespace final_project_backend.Services
{
    public class UserService
    {
        private readonly FinalProjectTrainingDbContext _db;
        public UserService(FinalProjectTrainingDbContext db)
        {
            _db = db;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var data = await _db.Users.ToListAsync();
            return data;
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
    }
}
