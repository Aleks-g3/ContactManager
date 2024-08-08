﻿using ContactManager.Server.Entities;

namespace ContactManager.Server.DTOs
{
    public class RegisterFormDTO
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Category { get; set; }
        public required string SubCategory { get; set; }
        public string? OtherSubCategory { get; set; }
        public required string BirthDate { get; set; }
    }
}
