using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public class RecordService : EntityBaseRepository<Record>, IRecordService
    {
        private readonly PharmaDbContext _context;
        public RecordService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
