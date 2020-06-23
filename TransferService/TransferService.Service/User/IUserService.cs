using TransferService.Domain;
using TransferService.Domain.Common;
using TransferService.Service.Generic;
using System;
using System.Collections.Generic;

namespace TransferService.Service
{
    public interface IUserService : IBaseService<User>
    {
        Result<User> AuthenticationByEmailAndPassword(User user);
        new Result<User> Update(User user);
        Result<User> ValidOldPasswordAndChangeUserPassword(User user, string newPassword);
        Result<User> ChangeUserPassword(User user, string newPassword);
    }
}
