using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            this._authManager = authManager;
            this._logger = logger;
        }

        //  api/account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest)]
        [ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError)]
        [ProducesResponseTypeAttribute(StatusCodes.Status200OK)]

        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {

            _logger.LogInformation($"Registration attempt for {apiUserDto.Email}");

            //warp all in try catch block
            try
            {


                //registration is done in the authManager service
                var errors = await _authManager.Register(apiUserDto);

                if (errors.Any())
                {
                    //model state is the body that is sent as a param
                    foreach (var error in errors)
                    {
                        //code is related error field, string Error message both coming from IdentityError return Object
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Ok(apiUserDto);

            }
            catch (Exception ex)
            {
                //{nameof(Register)} is th name of the method error came from
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)} - User Registration attempt for {apiUserDto.Email}");
                return Problem($"Something went Wrong in the {nameof(Register)} . Please Contact Support. ", statusCode: 500);
                

            }
            
        }


        //  api/account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest)]
        [ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError)]
        [ProducesResponseTypeAttribute(StatusCodes.Status200OK)]

        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation($"Login Attempt for {loginDto.Email} ");

            try
            {
                //auth response returns userId and Token as an object;
                var authResponse = await _authManager.Login(loginDto);

                if (authResponse == null)
                {
                    //401 not authenticated or 403 means forbidden not authorized
                    return Unauthorized();
                }
                return Ok(authResponse);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);

            }

        }

        //  api/account/refreshtoken
        //[HttpPost]
        //[Route("refreshtoken")]
        //[ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest)]
        //[ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseTypeAttribute(StatusCodes.Status200OK)]

        //public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        //{

        //    //auth response returns userId and Token as an object;
        //    var authResponse = await _authManager.VerifyRefreshToken(request);

        //    if (authResponse == null)
        //    {
        //        //401 not authenticated or 403 means forbidden not authorized
        //        return Unauthorized();
        //    }
        //    return Ok(authResponse);

        //}

    }
}
