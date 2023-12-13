using ERNIBankSystem.Data.Models;



namespace ERNIBankSystem.Data.Services.Interfaces
{
    public interface ITransactionServices
    {
        Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
    }
}
