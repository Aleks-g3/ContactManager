namespace ContactManager.Server.DTOs
{
    public class SimpleContactDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
    }
}
