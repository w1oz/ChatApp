using Microsoft.AspNetCore.Mvc;
using ChatApp.Services;
using ChatApp.Model;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DnsClient;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration.Json;

namespace ChatApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController :ControllerBase
    {
        private readonly UserServices _booksService;
        private readonly IConfiguration? _configuration;
        public UserController(UserServices booksService,IConfiguration config) {
            _booksService = booksService;
            _configuration = config;
                }
        [NonAction]
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes("64A63153-11C1-4919-9133-EFAF99A9B456");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }


            return principal;
        }
        [NonAction]
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        [HttpGet]
        public async Task<List<User>> Get() =>
            await _booksService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }


        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserReg newBook)
        {
            try
            {
                var user = new User()
                {
                    UserName = newBook.UserName,
                    Password = newBook.Password,
                    FirstName = newBook.FirstName,
                    LastName = newBook.LastName
                };
                await _booksService.CreateAsync(user);

                return CreatedAtAction(nameof(Get), new { id = user.Id }, newBook);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
     
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async  Task<IActionResult> Login(UserLogin User1)
        {
           
            try
            {
             var User_acc =  await _booksService.GetAccountAsync(User1?.Username);
                if (User_acc is null) return Unauthorized("Username not exist");
                if(User_acc.Password !=User1.Password) return Unauthorized("wrong username or password");

                var claims = new[]
                {
                new Claim(ClaimTypes.Name,User_acc.UserName),
                new Claim("UserId", User_acc.Id.ToString())
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("64A63153-11C1-4919-9133-EFAF99A9B456"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //  var expiry = DateTime.Now.AddDays(Convert.ToInt32("1"));
                var expiry = DateTime.Now.AddSeconds(Convert.ToInt32("30"));
                var token = new JwtSecurityToken(
                 "http://192.168.3.12:8080",
               "http://192.168.3.12:8080",
                  claims,
                  expires: expiry,
                  signingCredentials: creds
                   
      );
              
                return Ok( new
                {
                    token=new JwtSecurityTokenHandler().WriteToken(token),
                    refresh_token = GenerateRefreshToken(),
                    message ="login sucess"
                    

                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Refresh_Token")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh_Token(TokenModel token)
        {
            try
            {
          Console.WriteLine(GetPrincipalFromExpiredToken(token.Access_Token).Identity.Name);
                GenerateRefreshToken();
                await _booksService.UpdateToken("646c0f1b9d0013abd2e57e71", GenerateRefreshToken());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User updatedBook)
        {
            try
            {
                var book = await _booksService.GetAsync(id);

                if (book is null)
                {
                    return NotFound();
                }

                updatedBook.Id = book.Id;

                await _booksService.UpdateAsync(id, updatedBook);

                return NoContent();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var book = await _booksService.GetAsync(id);

                if (book is null)
                {
                    return NotFound();
                }

                await _booksService.RemoveAsync(id);

                return NoContent();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
