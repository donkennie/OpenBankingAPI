using OpenBankingAPI.ApplicationDb;
using OpenBankingAPI.Models;
using OpenBankingAPI.Services.Interfaces;
using System.Text;

namespace OpenBankingAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly OpenBankingDbContext _openBankingDbContext;

        private readonly ILogger<AccountService> _logger;

        public AccountService(OpenBankingDbContext openBankingDbContext, ILogger<AccountService> logger)
        {
            _openBankingDbContext = openBankingDbContext;
            _logger = logger;
        }


        public Account Authenticate(string AccountNumber, string Pin)
        {
            if (string.IsNullOrEmpty(AccountNumber) || string.IsNullOrEmpty(Pin))
                return null;
            var account = _openBankingDbContext.Accounts.SingleOrDefault(x => x.AccountNumberGenerated == AccountNumber);
            //is account null
            if (account == null)
                return null;

            //so user exists,

            if (!VerifyPinHash(Pin, account.PinStoredHash, account.PinStoredSalt))
                return null;

            //auth successful
            return account;
        }


        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            //
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }

            return true;
        }

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            //validate
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin cannot be empty");
            //does a user with this email exist already?
            if (_openBankingDbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("A userr with thiss email exists");
            //is pin eequal to confirmmpin
            if (!Pin.Equals(ConfirmPin)) throw new ApplicationException("Pins do not match.");

            //if validation passes
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);

            account.PinStoredHash = pinHash;
            account.PinStoredSalt = pinSalt;

            _openBankingDbContext.Accounts.Add(account);
            _openBankingDbContext.SaveChanges();

            return account;



        }

        private static void CreatePinHash(string Pin, out byte[] pinHash, out byte[] pinSalt)
        {
            //checks pin
            if (string.IsNullOrEmpty(Pin)) throw new ArgumentNullException("Pin");
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
            }
        }

        public void Delete(int Id)
        {
            var account = _openBankingDbContext.Accounts.Find(Id);
            if (account != null)
            {
                _openBankingDbContext.Accounts.Remove(account);

                _openBankingDbContext.SaveChanges();
            }
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _openBankingDbContext.Accounts.ToList();
        }

        public Account GetById(int Id)
        {
            var account = _openBankingDbContext.Accounts.Where(x => x.Id == Id).FirstOrDefault();
            return account;
        }

        public void Update(Account account, string Pin = null)
        {
            // fnd userr
            var accountToBeUpdated = _openBankingDbContext.Accounts.Find(account.Id);
            if (accountToBeUpdated == null) throw new ApplicationException("Account not found");
            //so we have a match
            if (!string.IsNullOrWhiteSpace(account.Email) && account.Email != accountToBeUpdated.Email)
            {
                //throw error because email passeed doesn't matc wiith
                if (_openBankingDbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("Email " + account.Email + " has been taken");
                accountToBeUpdated.Email = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber) && account.Email != accountToBeUpdated.PhoneNumber)
            {
                //throw error because email passeed doesn't matc wiith
                if (_openBankingDbContext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException("PhoneNumber " + account.PhoneNumber + " has been taken");
                accountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }


            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                accountToBeUpdated.PinStoredHash = pinHash;
                accountToBeUpdated.PinStoredSalt = pinSalt;

            }

            _openBankingDbContext.Accounts.Update(accountToBeUpdated);
            _openBankingDbContext.SaveChanges();




        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _openBankingDbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if (account == null)
            {
                return null;
            }


            return account;
        }
    }
}
