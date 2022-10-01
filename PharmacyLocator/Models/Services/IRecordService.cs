using PharmacyLocator.Base;

namespace PharmacyLocator.Models.Services
{
    public interface IRecordService : IEntityBaseRepository<Record>
    {
        public Task<Record> checkRecord(long userid, long mid);
        public Task<Record> getRecord(long userid, long mid);
        public Task<IEnumerable<Record>> getRecords();
    }
}
