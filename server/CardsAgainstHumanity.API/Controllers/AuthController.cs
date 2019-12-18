using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsAgainstHumanity.API.Models;
using CardsAgainstHumanity.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CardsAgainstHumanity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILogger<AuthController> logger;
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        //JWT Authentication -----------------------------------------------------------------
        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult GenerateJwtToken()
        {
            // TODO
            // CHECK IF USERNAME IS TAKEN INSIDE USERSCONTROLLER
            try
            {
                var jwtsvc = new JWTServices(configuration, logger);
                var token = jwtsvc.GenerateJwtToken();
                return Ok(token);
            }
            catch (Exception exc)
            {
                logger.LogError($"Exception thrown when creating JWT: {exc}");
            }
            //Bij niet succesvolle authenticatie wordt een Badrequest (=zo weinig mogelijke info) teruggeven.
            return BadRequest("Failed to generate JWT token");
        }

    }
}