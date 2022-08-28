
using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public class StoreService : EntityBaseRepository<Store>, IStoreService
    {
        private readonly PharmaDbContext _context;
        public StoreService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
