using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface IRequestService : IEntityBaseRepository<Requests>
    {
        public Task<bool> NameExist(String name);
    }
}
