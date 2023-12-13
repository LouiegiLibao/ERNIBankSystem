using ERNIBankSystem.Data.Models;
using ERNIBankSystem.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace ERNIBankSystem.Data.Services.Implementations
{
    public class AccountService : IAccountServices
    {
        private readonly ERNIDbContext _dbContext;
        private ILogger<AccountService> _logger;

        public AccountService(ERNIDbContext dbContext,ILogger<AccountService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public Account Authenticate(string AccountNumber, string Pin)
        {
            if (string.IsNullOrEmpty(AccountNumber) || string.IsNullOrEmpty(Pin))
            return null;
            var account = _dbContext.Accounts.SingleOrDefault(x => x.AccountNumberGenerated == AccountNumber);
            if (account == null)
                return null;
            if (!VerifyPinHash(Pin, account.PinStoredHash, account.PinStoredSalt))
                return null;
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
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin cannot be empty");
            if (!Pin.Equals(ConfirmPin)) throw new ApplicationException("Pins do not match.");
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);
            account.PinStoredHash = pinHash;
            account.PinStoredSalt = pinSalt;
            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();
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
        public IEnumerable<Account> GetAllAccounts()
        {
            return _dbContext.Accounts.ToList();
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public Account GetById(int Id)
        {
            var account = _dbContext.Accounts.Where(x => x.Id == Id).FirstOrDefault();
            return account;
        }
        public Account ViewBalance(string AccountNumber, string Pin)
        {
            if (string.IsNullOrEmpty(AccountNumber) || string.IsNullOrEmpty(Pin))
                return null;
            var account = _dbContext.Accounts.SingleOrDefault(x => x.AccountNumberGenerated == AccountNumber);
            if (account == null)
                return null;
            if (!VerifyPinHash(Pin, account.PinStoredHash, account.PinStoredSalt))
                return null;
            return account;
        }

    }
}
