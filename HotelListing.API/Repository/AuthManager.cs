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

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            //booleans by default are false, isVald User changes if 
            bool isValidUser = false;
            try
            {
                //var user object gets user info by email

                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user is null)
                {
                    return default;
                }

                isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isValidUser)
                {
                    return default;
                }

                return isValidUser;
            }
            catch (Exception)
            {
                //throw;
            }
            return isValidUser;
            
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            //returns empty error meaing it was a success , if there wer any it displays error message, returns errors both ways
            return result.Errors;
        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            //instead of using builder u inject the configurations service into this
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration
            ["JwtSettings:Key"]));

            //generate credentials
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //get roles of user
            var roles = await _userManager.GetRolesAsync(user);

            // list claim generated for the roles, gets the role claim and then adds the db value which is x,ties claims and roles
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            //get claims of user
            var userClaims = await _userManager.GetClaimsAsync(user);

            //generate list of claims
            var claims = new List<Claim>
            {
               // userName or Email Sub claim represents the subject /person the claim was issued to
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //generates random Guid, it changes everytime
                new Claim(JwtRegisteredClaimNames.Email, user.Email), //Specifies email to that particular ClaimTypes
                new Claim("uid",user.Id)  //Get user Id of record nd Claim in db
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
