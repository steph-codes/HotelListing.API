using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager authManager)
        {
            this._authManager = authManager;
        }

        //  api/account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest)]
        [ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError)]
        [ProducesResponseTypeAttribute(StatusCodes.Status200OK)]

        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
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


        //  api/account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest)]
        [ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError)]
        [ProducesResponseTypeAttribute(StatusCodes.Status200OK)]

        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            // isvalid User returns true or false and its expected to return true
            var isValidUser = await _authManager.Login(loginDto);

            if (!isValidUser)
            {
                //401 not authenticated or 403 means forbidden not authorized
                return Unauthorized();
            }
            return Ok();

        }

    }
}
