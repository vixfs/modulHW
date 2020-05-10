using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyService.BusinessLogic;
using MoneyService.BusinessLogic.Accounts;
using MoneyService.Models.Accounts;
using MoneyService.Services.Interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly AddAccountRequestHandler _addAccountRequestHandler;

        public AccountsController(
            IAccountService accountService, 
            AddAccountRequestHandler addAccountRequestHandler)
        {
            _accountService = accountService;
            _addAccountRequestHandler = addAccountRequestHandler;
        }
        [Authorize, HttpGet("add")]
        public IActionResult AddAccount()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var currentUserId = claim[0].Value;
            var newAccount = _addAccountRequestHandler.Handle(currentUserId).Result;
            
            return Ok(new {newAccount});
        }
    }
}