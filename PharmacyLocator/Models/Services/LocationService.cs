using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> NameExist(Location loc, bool notthis = false)
        {
            if (notthis) {
                var location = await _context.locations.FirstOrDefaultAsync(x => x.Name == loc.Name && x.Id != loc.Id);
                return location == null ? false : true;
            }
            else
            {
                var location = await _context.locations.FirstOrDefaultAsync(x => x.Name == loc.Name);
                return location == null ? false : true;
            }
        }
    }
}
