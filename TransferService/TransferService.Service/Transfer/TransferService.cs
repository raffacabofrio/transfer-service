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
        private readonly IEntryRepository _entryRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public TransferService(ITransferRepository transferRepository,
                            IUnitOfWork unitOfWork,
                            IValidator<Transfer> validator,
                            IUserRepository userRepository,
                            IEntryRepository entryRepository,
                            IBankAccountRepository bankAccountRepository)
            : base(transferRepository, unitOfWork, validator)
        {
            _transferRepository = transferRepository;
            _userRepository = userRepository;
            _entryRepository = entryRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public decimal getBalance(Guid userId)
        {
            var bankAccount = _bankAccountRepository.Get()
                .Where(b => b.UserId == userId)
                .FirstOrDefault();

            if (bankAccount == null)
                throw new TransferServiceException(TransferServiceException.Error.NotFound);

            return bankAccount.Balance;
        }

        public IList<Entry> getStatement(Guid userId)
        {
            var statement = _entryRepository.Get()
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreationDate)
                .Take(25)
                .ToList();

            return statement;
        }

        public Guid Transfer(Guid userId, int accountNumberOrigin, int accountNumberDestination, decimal value)
        {
            var userOrigin = GetUser(accountNumberOrigin);
            var userDestination = GetUser(accountNumberDestination);

            if(userOrigin == null)
                throw new TransferServiceException(TransferServiceException.Error.NotFound, "Conta de origem não encontrada.");

            if (userDestination == null)
                throw new TransferServiceException(TransferServiceException.Error.NotFound, "Conta de destino não encontrada.");

            if (userId != userOrigin.Id)
                throw new TransferServiceException(TransferServiceException.Error.Forbidden, "Essa conta de origem não é sua. Operação cancelada.");

            if (value < 0 || value > 10000)
                throw new TransferServiceException(TransferServiceException.Error.BadRequest, "O Valor deve estar entre 0 e 10 mil.");

            if (value > userOrigin.BankAccount.Balance)
                throw new TransferServiceException(TransferServiceException.Error.BadRequest, "Você não tem saldo suficiente para essa operação.");

            // 1 - create main transfer
            var transferId = Guid.NewGuid();
            var transfer = new Transfer()
            {
                UserOrigin = userOrigin,
                UserDestination = userDestination,
                Value = value,
                Id = transferId
            };
            _transferRepository.Insert(transfer);

            // 2 - create entry origin
            var entryOrigin = new Entry()
            {
                User = userOrigin,
                Description = "[envio] Transferência para " + userDestination.Name,
                Value = value * -1,
                Transfer = transfer
            };
            _entryRepository.Insert(entryOrigin);

            // 3 - create entry destination
            var entryDestination = new Entry()
            {
                User = userDestination,
                Description = "[recebimento] Transferência de " + userOrigin.Name,
                Value = value,
                Transfer = transfer
            };
            _entryRepository.Insert(entryDestination);

            // 4 - update balance
            _bankAccountRepository.updateBalance(userOrigin.Id, value * -1);
            _bankAccountRepository.updateBalance(userDestination.Id, value);

            return transferId;
        }

        // Todo: mover para a camada repository, pra facilitar os testes.
        private User GetUser(int accountNumber)
        {
            var user = _userRepository.Get()
                .Include(u => u.BankAccount)
                .Where(u => u.BankAccount.AccountNumber == accountNumber)
                .FirstOrDefault();

            return user;
        }

        
    }
}