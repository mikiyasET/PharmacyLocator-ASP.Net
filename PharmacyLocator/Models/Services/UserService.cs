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
    }
}