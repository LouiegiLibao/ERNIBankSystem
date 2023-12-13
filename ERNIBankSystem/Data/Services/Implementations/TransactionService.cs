using ERNIBankSystem.Data.Models;
using ERNIBankSystem.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ERNIBankSystem.Data.Services.Implementations
{
    public class TransactionService : ITransactionServices
    {
        private ERNIDbContext _dbContext;

        private ILogger<TransactionService> _logger;
        private IAccountServices _accountServices;

        public TransactionService(ERNIDbContext dbContext, ILogger<TransactionService> logger, IAccountServices accountServices)
        {
            _dbContext = dbContext;
            _logger = logger;
            _accountServices = accountServices;
        }
        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account destinationAccount; 
            Transaction transaction = new Transaction();
            try
            {
                destinationAccount = _accountServices.GetByAccountNumber(AccountNumber);
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseMessage = "Transaction Successful!";
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseMessage = "Transaction Failed!";
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = TranType.Deposit;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDestinationAccount = AccountNumber;
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;
        }

        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();
            try
            {
                sourceAccount = _accountServices.GetByAccountNumber(FromAccount);
                destinationAccount = _accountServices.GetByAccountNumber(ToAccount);

                sourceAccount.CurrentAccountBalance -= Amount; 
                destinationAccount.CurrentAccountBalance += Amount; 

                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) && (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseMessage = "Transaction Successful!";
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseMessage = "Transaction Failed!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = TranType.Transfer;
            transaction.TransactionAmount = Amount;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount; 
            Transaction transaction = new Transaction();
            try
            {
                sourceAccount = _accountServices.GetByAccountNumber(AccountNumber);
                sourceAccount.CurrentAccountBalance -= Amount; 

                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so there was an update in the context State
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseMessage = "Transaction Successful!";
                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseMessage = "Transaction Failed!";
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionAmount = Amount;
            transaction.TransactionSourceAccount = AccountNumber;
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;
        }
    }
}
