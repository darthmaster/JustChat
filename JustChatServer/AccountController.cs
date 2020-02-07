using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace JustChat
{
    public class AccountController : Controller
    {
        private List<Person> people = new List<Person>
        {
            new Person {Login="Yoba", Password="1"},
            new Person {Login="Anonymous",Password="pidor"}
        };

        private ClaimsIdentity GetIdentity(string login, string pass)
        {
            Person person = people.FirstOrDefault(x => x.Login == login && x.Password == pass);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim( ClaimsIdentity.DefaultNameClaimType ,person.Login)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);
            }
            return null;
        }
    }
}
