using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface IUserService : IEntityBaseRepository<User>
    {
        public Task<bool> Login(string username, string password);
        public Task<long> getIdFromUsername(string username);
        public Task<bool> checkUsername(string username);
    }
}
