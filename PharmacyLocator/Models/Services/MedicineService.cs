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
    }
}
