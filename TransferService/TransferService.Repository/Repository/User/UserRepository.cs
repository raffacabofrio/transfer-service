using TransferService.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TransferService.Repository
{
    public class UserRepository : RepositoryGeneric<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> UpdatePassword(User user)
        {
            _dbSet.Update(user);
            _context.Entry(user).Property(x => x.Password).IsModified = true;
            _context.Entry(user).Property(x => x.PasswordSalt).IsModified = true;
            await _context.SaveChangesAsync();

            return user;
        }

        public User GetUser(int accountNumber)
        {
            var user = Get()
                .Include(u => u.BankAccount)
                .Where(u => u.BankAccount.AccountNumber == accountNumber)
                .FirstOrDefault();

            return user;
        }
    }
}
