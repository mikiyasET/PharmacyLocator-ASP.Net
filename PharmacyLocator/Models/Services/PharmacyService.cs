using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public class PharmacyService : EntityBaseRepository<Pharmacy>, IPharmacyService
    {
        private readonly PharmaDbContext _context;
        public PharmacyService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
