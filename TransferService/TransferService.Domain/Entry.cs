using System;
using System.Text.RegularExpressions;
using TransferService.Domain.Common;
using TransferService.Domain.Enums;

namespace TransferService.Domain
{
    public class Entry : BaseEntity
    {
        public string Description { get; set; }
        public decimal Value { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
