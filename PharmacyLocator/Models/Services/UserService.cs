using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public class UserService : EntityBaseRepository<User>, IUserService
    {
        private readonly PharmaDbContext _context;
        public UserService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> Login(string username, string password)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            return user != null;
        }
        public async Task<long> getIdFromUsername(string username)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.Username == username);
            return user.Id;
        }
        public async Task<bool> checkUsername(string username)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.Username == username);
            return user != null;
        }

    }
}