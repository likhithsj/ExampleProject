﻿using Example.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Example.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IList<Credential> _appUsers
            = new List<Credential>
            {
                new Credential { FullName = "Admin User", UserName = "admin", Password = "1234", UserRole = "Admin"},
                new Credential { FullName = "Test User", UserName = "user", Password = "1234", UserRole = "User"}
            };
        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Credential credential)
        {
            IActionResult response = Unauthorized();

            var user = AuthenticateUser(credential);

            if (user != null)
            {
                var tokenString = GenerateJWTToken(user);

                response = Ok(
                    new
                    {
                        token = tokenString,
                        UserDetails = user
                    }
                    );
            }
            return response;

        }

        Credential AuthenticateUser(Credential loginCredentials)
        {
            Credential user = _appUsers.SingleOrDefault(x =>
                                x.UserName == loginCredentials.UserName
                                && x.Password == loginCredentials.Password);
            return user;
        }

        private string GenerateJWTToken(Credential credential)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]));
            var signingCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            // Set the claims which will also include "roles".
            var claims = new[]
            {
                new Claim (JwtRegisteredClaimNames.Sub, credential.UserName),

                new Claim ("fullName", credential.FullName),

                new Claim ("role", credential.UserRole),

                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:ValidIssuer"],
                audience: Configuration["Jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);



        }


    }
}
