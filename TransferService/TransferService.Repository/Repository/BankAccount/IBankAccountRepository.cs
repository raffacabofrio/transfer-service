using System;
using TransferService.Domain;

namespace TransferService.Repository
{
    public interface IBankAccountRepository : IRepositoryGeneric<BankAccount>
    {
        public void updateBalance(Guid userId, decimal value);
    }
}
