using AutoMapper;
using ContactManager.Server.DTOs;
using ContactManager.Server.Entities;
using ContactManager.Server.Extensions;

namespace ContactManager.Server.Profiles
{
    public static class ContactMappers
    {
        public static SimpleContactDTO ToSimpleContactDTO(this Contact contact)
        {
            return new SimpleContactDTO
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                Email = contact.Email!
            };
        }

        public static ContactDTO ToContactDTO(this Contact contact)
        {
            return new ContactDTO
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                Category = contact.Category.ConvertToString()!,
                SubCategory = contact.SubCategory.ConvertToString(),
                OtherSubCategory = contact.OtherSubCategory,
                PhoneNumber = contact.PhoneNumber!,
                Email = contact.Email!,
                BirthDate = contact.BirthDate.ConvertToString(),
            };
        }
    }
}
