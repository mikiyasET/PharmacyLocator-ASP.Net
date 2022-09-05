using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface IPharmacyService : IEntityBaseRepository<Pharmacy>
    {
        public Task<bool> NameExist(Pharmacy pharmacy, bool notthis = false);
        public Task<bool> Login(string email, string password);
        public Task<long> getIdFromEmail(string email);
    }
}
