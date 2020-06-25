using TransferService.Domain;
using System;

namespace TransferService.Api.ViewModels
{
    public class EntryVM : BaseViewModel
    {
        public string Description { get; set; }
        public decimal Value { get; set; }
        public Guid? TransferId { get; set; }
        public DateTime CreationDate { get; set; }

    }

}
