using TransferService.Domain;

namespace TransferService.Repository
{
    public class EntryRepository : RepositoryGeneric<Entry>, IEntryRepository
    {
        public EntryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
