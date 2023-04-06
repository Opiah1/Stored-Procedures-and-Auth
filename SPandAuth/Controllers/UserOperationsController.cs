using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPandAuth.AuthClass;
using SPandAuth.Data;
using SPandAuth.Dbervices;
using SPandAuth.RequestObj;
using SPandAuth.ResponseObj;
using SPandAuth.Services;

namespace SPandAuth.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IdbOperations _dbOperations;
        private readonly IJWTManagerRepository _jWTManager;

        public UserOperationsController(IConfiguration config, DataContext context, IdbOperations dbOperations, IJWTManagerRepository jWTManager)
        {
            _config = config;
            _context = context;
            _dbOperations = dbOperations;
            _jWTManager = jWTManager;
        }


        [HttpPost("CreateAccount")]
        [Produces("application/json")]
        [AllowAnonymous]
        public CreateAcctResponse CreateAccount([FromBody] UserRegisterRequest credparam)
        {
            CreateAcctData cretat = _dbOperations.RegisterAccount(credparam);

            if (cretat.issuccessful == true)
            {
                return new CreateAcctResponse { ResponseCode = "00", ResponseDescription = cretat.message, AccountNumber = cretat.AccountNumber,};
            }
            else
            {
                return new CreateAcctResponse { ResponseCode = "09", ResponseDescription = cretat.message, AccountNumber = null };
            }

        }
        [HttpPost("Login")]
        [Produces("application/json")]
        [AllowAnonymous]
        public Tokens Login([FromBody] UserLogin credparam)
        {
            var cretat = _jWTManager.Authenticate(credparam);
            return cretat;
        }

        [HttpGet("BalanceEnquiry")]
        [Produces("application/json")]
        public ActionResult<BalanceResponse2> Enquiry(string accountnumber)
        {
            var cretat = _dbOperations.BalanceEnquiry(accountnumber);
            return Ok(cretat);
        }
    }
}
