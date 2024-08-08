namespace ContactManager.Server.DTOs
{
    public class UpdatePasswordFormDTO
    {
        public required string Id { get; set; }
        public required string Password { get; set; }
        public required string NewPassword { get; set; }
    }
}
