using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers {
    public class User {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public static class UserStorage {
        public static List<User> Users { get; set; } = new List<User> {
                new User {ID=Guid.NewGuid(),Username="user1",Password = "111" },
                new User {ID=Guid.NewGuid(),Username="user2",Password = "222" },
                new User {ID=Guid.NewGuid(),Username="user3",Password = "333" }
            };
    }


    [Route("api/[controller]")]
    public class TokenAuthController : Controller {


        private string GenerateToken(User user, DateTime expires) {
            var handler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity(
                new GenericIdentity(user.Username, "TokenAuth"),
                new[] {
                    new Claim("ID", user.ID.ToString())
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = expires
            });

            return handler.WriteToken(securityToken);
        }



        [HttpPost]
        public string GetAuthToken(User user) {
            var existUser = UserStorage.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (existUser != null) {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
                var token = GenerateToken(existUser, expiresIn);

                return JsonConvert.SerializeObject(new {
                    stateCode = 1,
                    requertAt = requestAt,
                    expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
                    accessToken = token
                });
            } else {
                return JsonConvert.SerializeObject(new { stateCode = -1, errors = "Username or password is invalid" });
            }
        }


        [HttpGet]
        [Authorize("Bearer")]
        public string Get() {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var id = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "ID").Value;

            return $"Hello! {HttpContext.User.Identity.Name}, your ID is:{id}";
        }

    }
}
