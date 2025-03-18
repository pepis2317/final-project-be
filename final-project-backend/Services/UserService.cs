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
    }
}
