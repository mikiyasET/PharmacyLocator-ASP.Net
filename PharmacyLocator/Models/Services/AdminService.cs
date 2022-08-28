using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;
using System.Security.Claims;

namespace PharmacyLocator.Models.Services
{
    public class AdminService : EntityBaseRepository<Admin>, IAdminService
    {
        private readonly PharmaDbContext _context;
        public AdminService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> Login(string username,string password)
        {
            var admin = await _context.admins.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            return admin != null;
        }
        public async Task<long> getIdFromUsername(string username)
        {
            var admin = await _context.admins.FirstOrDefaultAsync(x => x.Username == username);
            return admin.Id;
        }
    }
}
