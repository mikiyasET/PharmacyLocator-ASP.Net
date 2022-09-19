using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface IStoreService : IEntityBaseRepository<Store>
    {
        public Task<IEnumerable<Medicine>> GetMedNotInStore(long pharmacyId);
        public Task<IEnumerable<Store>> getByMedID(long id);
    }
}
