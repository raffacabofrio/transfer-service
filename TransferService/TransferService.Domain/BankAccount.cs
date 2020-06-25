using System;
using System.Text.RegularExpressions;
using TransferService.Domain.Common;
using TransferService.Domain.Enums;

namespace TransferService.Domain
{
    public class BankAccount : BaseEntity
    {
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; } = 0;
        public Guid UserId { get; set; }
    }
}
