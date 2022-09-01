using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface IMedicineService : IEntityBaseRepository<Medicine>
    {
        public Task<bool> NameExist(Medicine med, bool notthis = false);
    }
}
