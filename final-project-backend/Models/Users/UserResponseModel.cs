namespace final_project_backend.Models.Users
{
    public class UserResponseModel
    {
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required int? UserBalance {  get; set; }
        public required string? UserProfile { get; set; }
        public required string UserPhoneNumber { get; set; }
        public required string UserEmail { get; set; }
        public required string UserAddress { get; set; }
        public required DateOnly? BirthDate { get; set; }
        public required string? Gender { get; set; } 


    }
}
