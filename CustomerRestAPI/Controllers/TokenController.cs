using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CustomerAppBLL;
using CustomerAppBLL.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerRestAPI.Controllers
{
    [Route("/token")]
    public class TokenController : Controller
    {
        IBLLFacade _facade;
        public TokenController(IBLLFacade facade) {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult Create(string username, string password)
        {
            var user = IsValidUserAndPasswordCombination(username, password);
            if(!string.IsNullOrEmpty(username) && user != null){
                var token = GenerateToken(user);
                return new ObjectResult(token);
            }
            return BadRequest();
        }

        private UserBO IsValidUserAndPasswordCombination(string username, string password)
        {
            List<UserBO> list = _facade.UserService.GetAll();
            var userFound = list.FirstOrDefault(u => u.Username == username && u.Password == password);
            return userFound;
        }

        private string GenerateToken(UserBO user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            if (user.Role == "Administrator")
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));


            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BOErgeOsTSpiser AErter 123 STK I ALT!")),
                                             SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
