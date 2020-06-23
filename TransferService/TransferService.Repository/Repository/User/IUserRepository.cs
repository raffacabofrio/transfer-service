using TransferService.Domain;
using System.Threading.Tasks;

namespace TransferService.Repository
{
    public interface IUserRepository : IRepositoryGeneric<User>
    {
        Task<User> UpdatePassword(User user);
    }
}
