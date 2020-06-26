using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using TransferService.Domain;
using TransferService.Domain.Exceptions;
using TransferService.Repository;
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
            var userOrigin = _userRepository.GetUser(accountNumberOrigin);
            var userDestination = _userRepository.GetUser(accountNumberDestination);

            if (userOrigin == null)
                throw new TransferServiceException(TransferServiceException.Error.NotFound, "Conta de origem não encontrada.");

            if (userDestination == null)
                throw new TransferServiceException(TransferServiceException.Error.NotFound, "Conta de destino não encontrada.");

            if (userId != userOrigin.Id)
                throw new TransferServiceException(TransferServiceException.Error.Forbidden, "Essa conta de origem não é sua. Operação cancelada.");

            if (value < 0 || value > 10000)
                throw new TransferServiceException(TransferServiceException.Error.BadRequest, "O Valor deve estar entre 0 e 10 mil.");

            if (value > userOrigin.BankAccount.Balance)
                throw new TransferServiceException(TransferServiceException.Error.BadRequest, "Você não tem saldo suficiente para essa operação.");

            try
            {
                _unitOfWork.BeginTransaction();

                var transfer = CreateMainTransfer(userOrigin, userDestination, value);

                CreateEntry(userOrigin, "[envio] Transferência para " + userDestination.Name, value * -1, transfer);
                CreateEntry(userDestination, "[recebimento] Transferência de " + userOrigin.Name, value, transfer);

                _bankAccountRepository.updateBalance(userOrigin.Id, value * -1);
                _bankAccountRepository.updateBalance(userDestination.Id, value);

                _unitOfWork.Commit();

                return transfer.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
            

        private Transfer CreateMainTransfer(User userOrigin, User userDestination, decimal value) {            
            var transfer = new Transfer()
            {
                UserOrigin = userOrigin,
                UserDestination = userDestination,
                Value = value,
            };

            return _transferRepository.Insert(transfer);
        }

        private void CreateEntry(User user, string description, decimal value, Transfer transfer)
        {
            var entryOrigin = new Entry()
            {
                User = user,
                Description = description,
                Value = value,
                Transfer = transfer
            };
            _entryRepository.Insert(entryOrigin);
        }
 
    }
}