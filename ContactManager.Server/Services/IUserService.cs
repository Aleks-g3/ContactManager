using ContactManager.Server.Entities;

namespace ContactManager.Server.Services
{
    public interface IUserService
    {
        Task<Contact[]> GetAll();
        Task<Contact> GetById(string id);
    }
}
