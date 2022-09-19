
using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var query = from med in _context.medicines
                                             where (from store in _context.stores where (med.Id == store.MedicineId) && (store.PharmacyId == pharmacyId) select store.MedicineId).FirstOrDefault() != med.Id
                                             select med;
            IEnumerable<Medicine> medicines = await query.ToListAsync().ConfigureAwait(false);
            return medicines;
        }
        public async Task<IEnumerable<Store>> getByMedID(long id)
        {
            var query = from med in _context.stores where med.MedicineId == id select med;
            IEnumerable<Store> res = await query.ToListAsync().ConfigureAwait(false);
            return res;
        }
    }
}
