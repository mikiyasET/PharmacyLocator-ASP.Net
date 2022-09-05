
using Microsoft.EntityFrameworkCore;
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
            var quary = from med in _context.medicines
                                             where (from store in _context.stores where (med.Id == store.MedicineId) && (store.PharmacyId == pharmacyId) select store.MedicineId).FirstOrDefault() != med.Id
                                             select med;
            IEnumerable<Medicine> medicines = await quary.ToListAsync().ConfigureAwait(false);
            return medicines;
        }
    }
}
