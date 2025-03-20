namespace final_project_backend.Models.Users
{
    public class UserUpdateRequest
    {
        public string? UserName { get; set; }
        public  string? UserPassword { get; set; }
        public  string? UserPhoneNumber { get; set; }
        public string? UserEmail { get; set; }
        public string? UserAddress { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Gender { get; set; }
        public int? UserBalance { get; set; }
    }
}
