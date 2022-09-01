using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface ILocationService : IEntityBaseRepository<Location>
    {
        public Task<bool> NameExist(Location loc, bool notthis = false);
    }
}
