using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public class LocationService : EntityBaseRepository<Location>, ILocationService
    {
        private readonly PharmaDbContext _context;
        public LocationService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
