using LearnAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using LearnAspNetCoreMVC.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace LearnAspNetCoreMVC.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountAPIController : Controller
    {
        //protected APIResponse _response;
        //Fix bug
        //paging
        //custom message
        //authentication + authorization => login, logout + account table => role
        //mapper ??
        //redirection to getById method:  CreatedAtRoute("GetById", new { id = villa.Id }, _response);
        //viết hoa controller, cấu trúc một api
        //async
        //try catch

        private readonly IRepository<Account> _accountRepository;
        private readonly IConfiguration _configuration;
        public AccountAPIController(IEnumerable<IRepository<Account>> listRepo, IConfiguration configuration)
        {
            _accountRepository = listRepo.ToList()[0];
            _configuration = configuration;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public IActionResult Validate(Account account)
        {
            var data = _accountRepository.Get(acc => acc.Username.Equals(account.Username) &&
                                                    acc.Password.Equals(account.Password));

            if (!data.Any())
            {
                return NotFound(new APIResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                    Message = "Username or password isn't exist!"
                });
            }

            var user = data.FirstOrDefault();

            //cấp token
            return Ok(new APIResponse
            {
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                Message = "Login successfully!",
                Data = GenerateToken(user)
            });
        }

        private string GenerateToken(Account account)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration.GetValue<string>("AppSettings:SecretKey");
            var secretKeyBytes = Encoding.UTF8.GetBytes(key);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Username", account.Username),

                    //roles

                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),

                Expires = DateTime.UtcNow.AddMinutes(1),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

    }
}