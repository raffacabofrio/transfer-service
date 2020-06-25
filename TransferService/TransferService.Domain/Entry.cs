using System;
using TransferService.Domain.Common;

namespace TransferService.Domain
{
    public class Entry : BaseEntity
    {
        public string Description { get; set; }
        public decimal Value { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? TransferId { get; set; }
        public Transfer Transfer { get; set; }
    }
}
