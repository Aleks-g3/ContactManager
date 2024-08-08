using ContactManager.Server.Entities;

namespace ContactManager.Server.DTOs
{
    public class ContactDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Category { get; set; }
        public string? SubCategory { get; set; }
        public string? OtherSubCategory { get; set; }
        public required string BirthDate { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
