using AutoMapper;
using ERNIBankSystem.Data.Models;
using ERNIBankSystem.Data.Services.Implementations;
using ERNIBankSystem.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ERNIBankSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private  ITransactionServices _transactionService;
        private  IAccountServices _accountService;
        private IMapper _mapper;
        public TransactionsController(IAccountServices accountService, ITransactionServices transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("view-account-balance")]
        public IActionResult ViewBalance(string AccountNumber, string TransactionPin)
        {
            var authResult = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authResult == null) return Unauthorized("Invalid Credentials");
            var account = _accountService.ViewBalance(AccountNumber, TransactionPin);
            var getAccountCurrentBalance = _mapper.Map<AccountCurrentBalance>(account);
            return Ok(getAccountCurrentBalance);
        }
        [HttpPost]
        [Route("make-deposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            var authResult = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authResult == null) return Unauthorized("Invalid Credentials");
            return Ok(_transactionService.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }
        [HttpPost]
        [Route("make-withdrawal")]
        public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            var authResult = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authResult == null) return Unauthorized("Invalid Credentials");
            return Ok(_transactionService.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
        }
        [HttpPost]
        [Route("make-transfer-money")]
        public IActionResult MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            var authResult = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authResult == null) return Unauthorized("Invalid Credentials");
            if (FromAccount.Equals(ToAccount)) return BadRequest("You cannot transfer money to yourself");
            return Ok(_transactionService.MakeFundsTransfer(FromAccount, ToAccount, Amount, TransactionPin));
        }
    }
}
