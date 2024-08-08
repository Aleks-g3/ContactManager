using ContactManager.Server.Entities;

namespace ContactManager.Server.Repositories
{
    public interface IUserRepository
    {
        Task<Contact[]> GetAllUser();
        Task<Contact?> GetUserById(string id);
    }
}
