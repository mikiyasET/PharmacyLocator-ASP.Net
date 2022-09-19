using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public class MedicineService : EntityBaseRepository<Medicine>, IMedicineService
    {
        private readonly PharmaDbContext _context;
        public MedicineService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> NameExist(Medicine med, bool notthis = false)
        {
            if (notthis)
            {
                var medicine = await _context.medicines.FirstOrDefaultAsync(x => x.Name == med.Name && x.Id != med.Id);
                return medicine == null ? false : true;
            }
            else
            {
                var medicine = await _context.medicines.FirstOrDefaultAsync(x => x.Name == med.Name);
                return medicine == null ? false : true;
            }
        }
        public async Task<IEnumerable<Medicine>> getLikeName(string q)
        {
            var quary = from med in _context.medicines where med.Name.Contains(q) select med;
            IEnumerable<Medicine> medicines = await quary.ToListAsync().ConfigureAwait(false);
            return medicines;
        }
        public async Task<long> getIdByName(string q)
        {
            Medicine medicine = await _context.medicines.FirstOrDefaultAsync(med => med.Name == q);
            return medicine.Id;
        }
    }
}
