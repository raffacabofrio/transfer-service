using TransferService.Domain;

namespace TransferService.Repository
{
    public class TransferRepository : RepositoryGeneric<Transfer>, ITransferRepository
    {
        public TransferRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
