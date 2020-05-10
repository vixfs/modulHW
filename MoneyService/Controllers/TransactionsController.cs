using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyService.BusinessLogic.Accounts;
using MoneyService.BusinessLogic.Transactions;
using MoneyService.Models.Transactions;
using MoneyService.Services.Interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService _transactionsService;
        private readonly RefillBalanceRequestHandler _refillBalanceRequestHandler;
        private readonly TransferMoneyRequestHandler _transferMoneyRequestHandler;

        public TransactionsController(
            ITransactionsService transactionsService,
            RefillBalanceRequestHandler refillBalanceRequestHandler,
            TransferMoneyRequestHandler transferMoneyRequestHandler)
        {
            _transactionsService = transactionsService;
            _refillBalanceRequestHandler = refillBalanceRequestHandler;
            _transferMoneyRequestHandler = transferMoneyRequestHandler;
        }

        [Authorize, HttpPost("refill")]
        public IActionResult Refill([FromBody] RefillModel refillModel)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var currentUserId = claim[0].Value;
            
            _refillBalanceRequestHandler.Handle(refillModel, currentUserId);
        
            return Ok("Пополнение успешно");
        }

        [Authorize, HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferModel transferModel)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var currentUserId = claim[0].Value;
            
            _transferMoneyRequestHandler.Handle(transferModel, currentUserId);
            
            return Ok("Перевод успешен");
        }
        
        
        
        
    }
}