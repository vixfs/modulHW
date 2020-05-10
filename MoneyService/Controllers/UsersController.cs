using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoneyService.BusinessLogic;
using MoneyService.BusinessLogic.Accounts;
using MoneyService.BusinessLogic.Users;
using MoneyService.Models.Users;
using MoneyService.Services.Interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly AuthOptions _authOptions;
        private readonly AddUserRequestHandler _addUserRequestHandler;
        private readonly CheckUserCredentialsRequestHandler _checkUserCredentialsRequestHandler;
        private readonly AddAccountRequestHandler _addAccountRequestHandler;


        public UsersController(IUserService userService,
            IOptions<AuthOptions> authOptionsAccessor,
            AddUserRequestHandler addUserRequestHandler,
            CheckUserCredentialsRequestHandler checkUserCredentialsRequestHandler,
            AddAccountRequestHandler addAccountRequestHandler)
        {
            _userService = userService;
            _authOptions = authOptionsAccessor.Value;
            _addUserRequestHandler = addUserRequestHandler;
            _checkUserCredentialsRequestHandler = checkUserCredentialsRequestHandler;
            _addAccountRequestHandler = addAccountRequestHandler;
        }

        
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            var addedUser = _addUserRequestHandler.Handle(model).Result;
            _addAccountRequestHandler.Handle(addedUser.Id);
            return Ok("Registration successful");
        }
        
        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult GetToken([FromBody] UserCredentials userCredentials)
        {
            var user = _checkUserCredentialsRequestHandler.Handle(userCredentials);
            if (user == null)
            {
                return Unauthorized();
            }
         
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience:_authOptions.Audience,
                expires: DateTime.Now.AddMinutes(_authOptions.ExpiresInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecureKey)), 
                    SecurityAlgorithms.HmacSha256Signature)
                );
                
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
            });
        }
        
        /*[Authorize, HttpGet("secure")]
        public dynamic Secret()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var currentId = claim[0].Value;
            var currentUser = _getUserByIdRequestHandler.Handle(currentId).Result;
            return currentUser;
        }*/
        
    }

}