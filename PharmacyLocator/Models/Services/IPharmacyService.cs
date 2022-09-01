using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface IPharmacyService : IEntityBaseRepository<Pharmacy>
    {
        public Task<bool> NameExist(Pharmacy pharmacy, bool notthis = false);
    }
}
