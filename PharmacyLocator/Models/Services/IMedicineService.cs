using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface IMedicineService : IEntityBaseRepository<Medicine>
    {
        public Task<bool> NameExist(Medicine med, bool notthis = false);
        public Task<IEnumerable<Medicine>> getLikeName(string q);
        public Task<long> getIdByName(string q);
    }
}
