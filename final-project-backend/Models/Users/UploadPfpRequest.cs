namespace final_project_backend.Models.Users
{
    public class UploadPfpRequest
    {
        public required Guid UserId { get; set; }
        public required IFormFile file { get; set; }
    }
}
