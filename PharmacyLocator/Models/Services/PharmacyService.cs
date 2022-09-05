using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> Login(string email, string password)
        {
            var pharmacy = await _context.pharmacies.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            return pharmacy != null;
        }
        public async Task<long> getIdFromEmail(string email)
        {
            var pharmacy = await _context.pharmacies.FirstOrDefaultAsync(x => x.Email == email);
            return pharmacy.Id;
        }
        
        public async Task<bool> NameExist(Pharmacy pharma, bool notthis = false)
        {
            if (notthis) {
                var pharmacy = await _context.pharmacies.FirstOrDefaultAsync(x => x.Name == pharma.Name && x.Id != pharma.Id);
                return pharmacy == null ? false : true;
            }
            else {
                var pharmacy = await _context.pharmacies.FirstOrDefaultAsync(x => x.Name == pharma.Name);
                return pharmacy == null ? false : true;
            }
        }
    }
}
