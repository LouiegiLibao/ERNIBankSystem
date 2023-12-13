using ERNIBankSystem.Data.Models;

namespace ERNIBankSystem.Data.Services.Interfaces
{
    public interface IAccountServices
    {
        Account Authenticate(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string Pin, string ConfirmPin);
        Account GetByAccountNumber(string AccountNumber);
        Account ViewBalance(string AccountNumber, string TransactionPin);

    }
}
