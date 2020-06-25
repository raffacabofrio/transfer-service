using System;
using TransferService.Domain.Common;

namespace TransferService.Domain
{
    public class BankAccount : BaseEntity
    {
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; } = 0;
        public Guid UserId { get; set; }
    }
}
