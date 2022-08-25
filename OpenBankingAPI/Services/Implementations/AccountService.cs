using OpenBankingAPI.ApplicationDb;
using OpenBankingAPI.Models;
using OpenBankingAPI.Services.Interfaces;

namespace OpenBankingAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly OpenBankingDbContext _openBankingDbContext;

        public Account Authenticate(string AccountNumber, string Pin)
        {
            throw new NotImplementedException();
        }

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            throw new NotImplementedException();
        }

        public Account GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(Account account, string Pin = null)
        {
            throw new NotImplementedException();
        }
    }
}
