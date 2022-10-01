using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyLocator.Models.Services
{
    public class RecordService : EntityBaseRepository<Record>, IRecordService
    {
        private readonly PharmaDbContext _context;
        public RecordService(PharmaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Record> checkRecord(long userid, long mid)
        {
            var result = await _context.records.FirstOrDefaultAsync(n => n.MedicineId == mid && n.UserId == userid);
            return result;
        }
        public async Task<Record> getRecord(long userid, long mid)
        {
            var query = from record in _context.records
                        where record.MedicineId == mid && record.UserId == userid
                        select record;
            Record records = await query.FirstOrDefaultAsync().ConfigureAwait(false);
            return records;
        }
        public async Task<IEnumerable<Record>> getRecords()
        {
           

            var query = from p in _context.records
                        group p by p.MedicineId into g
                        orderby g.Sum(x => x.Count) descending
                        select new Record
                        {
                            MedicineId = g.Key,
                            Count = g.Sum(x => x.Count)
                        };
            IEnumerable<Record> records = await query.ToListAsync().ConfigureAwait(false);

            return records;
        }
    }
}
