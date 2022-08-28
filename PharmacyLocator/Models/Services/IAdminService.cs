using PharmacyLocator.Base;
using System.Security.Claims;

namespace PharmacyLocator.Models.Services
{
    public interface IAdminService : IEntityBaseRepository<Admin>
    {
        public Task<bool> Login(string username, string password);
        public Task<long> getIdFromUsername(string username);
    }
}
