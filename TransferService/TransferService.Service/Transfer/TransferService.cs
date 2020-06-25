using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TransferService.Domain;
using TransferService.Domain.Common;
using TransferService.Domain.Exceptions;
using TransferService.Helper.Crypto;
using TransferService.Repository;
using TransferService.Repository.Repository;
using TransferService.Repository.UoW;
using TransferService.Service.Generic;

namespace TransferService.Service
{
    public class TransferService : BaseService<Transfer>, ITransferService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransferRepository _transferRepository;


        public TransferService(ITransferRepository transferRepository,
                            IUnitOfWork unitOfWork,
                            IValidator<Transfer> validator,
                            IUserRepository userRepository)
            : base(transferRepository, unitOfWork, validator)
        {
            _transferRepository = transferRepository;
            _userRepository = userRepository;
        }

        public decimal getBalance(Guid userId)
        {
            var user = _userRepository.Get()
                .Include(u => u.BankAccount)
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            if (user == null)
                throw new TransferServiceException(TransferServiceException.Error.NotFound);

            return user.BankAccount.Balance;
        }



       
    }
}