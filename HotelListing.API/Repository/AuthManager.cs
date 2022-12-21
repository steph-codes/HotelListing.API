using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repository
{
    public class AuthManager : IAuthManager

    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<string> CreateRefreshToken()
        {
            await 
        }
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            //booleans by default are false, isVald User changes if 
         
            //var user object gets user info by email

            var _user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
                
            if(_user == null || isValidUser == false)
            {
                return null;
            }

            var token = await GenerateToken(_user);
            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id
            };
            
            
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if(result.Succeeded)
            {
                if (userDto.JobRoles == "Employee" || userDto.JobRoles == null)
                {
                    await _userManager.AddToRoleAsync(_user, "User");
                    return result.Errors;
                }

                await _userManager.AddToRoleAsync(_user, "Administrator");

            }
            //returns empty error meaing it was a success , if there wer any it displays error message, returns errors both ways
            return result.Errors;
        }

        private async Task<string> GenerateToken(ApiUser _user)
        {
            //instead of using builder u inject the configurations service into this
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration
            ["JwtSettings:Key"]));

            //generate credentials
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //get roles of user
            var roles = await _userManager.GetRolesAsync(_user);

            // list claim generated for the roles, gets the role claim and then adds the db value which is x,ties claims and roles
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            //get claims of user
            var userClaims = await _userManager.GetClaimsAsync(_user);

            //generate list of claims
            var claims = new List<Claim>
            {
               // userName or Email Sub claim represents the subject /person the claim was issued to
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //generates random Guid, it changes everytime
                new Claim(JwtRegisteredClaimNames.Email, _user.Email), //Specifies email to that particular ClaimTypes
                new Claim("uid",_user.Id)  //Get user Id of record nd Claim in db
            }
            .Union(userClaims).Union(roleClaims);  //at the end we have a big list of claims with what  number of users and roles came from db

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                //expires: DateTime.Now.AddDays(1));
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration
                ["JwtSettings:DurationInMinutes"])),  //convert from string to int with int 32 for it to work
                signingCredentials: credentials
                );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
