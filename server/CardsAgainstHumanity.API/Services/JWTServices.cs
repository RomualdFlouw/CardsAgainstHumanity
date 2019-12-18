using CardsAgainstHumanity.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.API.Services
{
    public class JWTServices
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        public JWTServices(IConfiguration configuration, ILogger logger)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        //returnen van een anoniem object
        public object GenerateJwtToken()
        {
            try
            {
                //1. Noodzakelijke claims komende vd JWD spec
                var claims = new List<Claim>
                {
                    //JWT claims zijn ingebouwd in de JWT spec: "sub"scriber, JWT Id
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                //2. Sigin credentials met de symmetric key & encryptie methode
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //key en protocol

                //3. Aanmaken van het token
                var token = new JwtSecurityToken(
                    issuer: configuration["Tokens:Issuer"], //onze website
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Tokens: Expires"])),
                    signingCredentials: creds //controleert token v
                    );

                //3. user info returnen (vervaldatum als additionele info)
                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token), //token generator
                    expiration = token.ValidTo,
                };
            }
            catch (Exception exc)
            {
                logger.LogError($"Exception thrown when creating JWT: {exc}");
            }
            return new { error = "Failed to generate JWT token" }; //minimale info ->meer in de logger
        }
    }
}
