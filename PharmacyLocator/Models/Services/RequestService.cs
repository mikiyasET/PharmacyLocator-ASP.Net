using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public class RequestService : EntityBaseRepository<Requests>, IRequestService
    {
        private readonly PharmaDbContext _context;
        public RequestService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> NameExist(String name)
        {
            var request = await _context.requests.FirstOrDefaultAsync(x => x.Name == name);
            return request == null ? false : true;   
        }
    }
}
