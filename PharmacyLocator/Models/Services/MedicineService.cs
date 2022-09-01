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
                var medicine = await _context.locations.FirstOrDefaultAsync(x => x.Name == med.Name && x.Id != med.Id);
                return medicine == null ? false : true;
            }
            else
            {
                var medicine = await _context.locations.FirstOrDefaultAsync(x => x.Name == med.Name);
                return medicine == null ? false : true;
            }
        }
    }
}
