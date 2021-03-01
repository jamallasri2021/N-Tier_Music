using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        #region Properties & constructors

        private readonly IUserService _userService;
        private readonly IMapper _mapperService;
        private readonly IConfiguration _config;

        public UserController(IUserService userService, IMapper mapperService, IConfiguration config)
        {
            _userService = userService;
            _mapperService = mapperService;
            _config = config;
        }
        #endregion

        #region functions
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticate userAuthenticate)
        {
            try
            {
                var user = await _userService.Authenticate(userAuthenticate.UserName, userAuthenticate.Password);
                if (user == null)
                {
                    return BadRequest("Invalid Username/Password");
                }

                // Send Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("AppSettings:Secret"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(
                                                                new SymmetricSecurityKey(key),
                                                                SecurityAlgorithms.HmacSha512Signature
                                                                )
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(
                    new
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Token = tokenString
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResource>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAll();

                if (users == null)
                {
                    return NotFound("Users not found");
                }

                // Mapping
                var usersResources = _mapperService.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

                return Ok(usersResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserToSave userToSave)
        {
            try
            {
                // Validation of input Data
                var validation = new UserToSaveResourceValidation();
                var validationResult = await validation.ValidateAsync(userToSave);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                // Mapping
                var user = _mapperService.Map<UserToSave, User>(userToSave);

                // Creation
                var userCreated = await _userService.Create(user, userToSave.Password);

                // Send Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("AppSettings:Secret"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(
                                                                new SymmetricSecurityKey(key),
                                                                SecurityAlgorithms.HmacSha512Signature
                                                                )
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(
                    new
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Token = tokenString
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
