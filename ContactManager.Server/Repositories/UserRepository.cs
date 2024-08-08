using ContactManager.Server.Contexts;
using ContactManager.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext context;

        public UserRepository(ApplicationContext context) 
        {
            this.context = context;
        }

        public Task<Contact[]> GetAllUser()
        {
            return context.Users.Where(u => u.Email != "technican@test.com").ToArrayAsync();
        }

        public Task<Contact?> GetUserById(string id)
        {
            return context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
