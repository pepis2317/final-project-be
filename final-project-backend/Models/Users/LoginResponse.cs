namespace final_project_backend.Models.Users
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
