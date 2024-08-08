using ContactManager.Server.Entities;
using ContactManager.Server.Exceptions;
using ContactManager.Server.Repositories;

namespace ContactManager.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task<Contact[]> GetAll()
        {
            return userRepository.GetAllUser();
        }

        public async Task<Contact> GetById(string id)
        {
            var contact = await userRepository.GetUserById(id);

            if (contact == null)
            {
                throw new NotFoundException($"user with id {id} does not exist");
            }

            return contact;
        }
    }
}
