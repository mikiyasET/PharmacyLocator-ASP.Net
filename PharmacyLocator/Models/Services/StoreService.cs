
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
        public async Task<IEnumerable<Medicine>> GetMedNotInStore(long pharmacyId)
        {
            IEnumerable<Medicine> medicine = from med in _context.medicines
                                        join store in _context.stores on med.Id equals store.MedicineId where pharmacyId == store.PharmacyId select med;
            return medicine;
        }
    }
}
