using TransferService.Domain;
using TransferService.Domain.Common;
using TransferService.Service.Generic;
using System;
using System.Collections.Generic;

namespace TransferService.Service
{
    public interface ITransferService : IBaseService<Transfer>
    {
        decimal getBalance(Guid userId);
        IList<Entry> getStatement(Guid userId);
        Guid Transfer(Guid userId, int accountNumberOrigin, int accountNumberDestination, decimal value);
    }
}
