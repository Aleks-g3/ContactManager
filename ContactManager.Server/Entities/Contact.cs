using Microsoft.AspNetCore.Identity;

namespace ContactManager.Server.Entities
{
    public class Contact : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Category Category { get; set; }
        public SubCategory? SubCategory { get; set; }
        public string? OtherSubCategory { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
