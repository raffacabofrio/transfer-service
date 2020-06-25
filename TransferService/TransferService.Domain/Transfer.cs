using System;
using System.ComponentModel.DataAnnotations.Schema;
using TransferService.Domain.Common;

namespace TransferService.Domain
{
    public class Transfer : BaseEntity
    {
        public Guid UserIdOrigin { get; set; }
        public Guid UserIdDestination { get; set; }

        [ForeignKey("UserIdOrigin")]
        public User UserOrigin { get; set; }

        [ForeignKey("UserIdDestination")]
        public User UserDestination { get; set; }
        public decimal Value { get; set; }

    }
        
}
