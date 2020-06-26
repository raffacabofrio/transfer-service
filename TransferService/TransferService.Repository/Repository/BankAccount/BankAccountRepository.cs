using System;
using System.Linq;
using TransferService.Domain;

namespace TransferService.Repository
{
    public class BankAccountRepository : RepositoryGeneric<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void updateBalance(Guid userId, decimal value)
        {
            var bankAccount = Get()
                .Where(b => b.UserId == userId)
                .FirstOrDefault();

            bankAccount.Balance += value;
            Update(bankAccount);
        }
    }
}
