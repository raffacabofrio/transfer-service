using System;

namespace TransferService.Domain.Common
{
    public interface IIdProperty
    {
        Guid Id { get; set; }
    }
}
