using TransferService.Domain;
using System.Threading.Tasks;

namespace TransferService.Repository
{
    public interface IUserRepository : IRepositoryGeneric<User>
    {
        public Task<User> UpdatePassword(User user);
        public User GetUser(int accountNumber);
    }
}
