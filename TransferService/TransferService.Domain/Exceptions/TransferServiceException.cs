using System;
using System.Collections.Generic;

namespace TransferService.Domain.Exceptions
{
    public class TransferServiceException : Exception
    {
        public static Dictionary<Error, string> ErrorMessages = new Dictionary<Error, string>()
        {
            { Error.NotAuthorized, "O usuário precisa estar logado para efetuar essa ação." },
            { Error.Forbidden, "Usuário não tem as permissões necessárias para efetuar esta ação." },
            { Error.NotFound, "Entidade não encontrada. Por favor, verifique." }
        };

        public enum Error
        {
            BadRequest = 400,
            NotAuthorized = 401,
            Forbidden = 403,
            NotFound = 404
        }

        public Error ErrorType { get; set; }

        public TransferServiceException(string message) : this(Error.BadRequest, message) { }
        public TransferServiceException(Error error) : this(error, ErrorMessages[error]) { }
        public TransferServiceException(Error error, string message) : base(message)
        {
            ErrorType = error;
        }
    }
}
