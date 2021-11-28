using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Data;
using Microsoft.Extensions.Logging;
using TestApplication.Data.Dtos;
using BCrypt;
using TestApplication.Data.Other;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        //public readonly DataContext _dbContext;
        //public readonly ILogger<AuthController> _logger;
        public readonly JwtService _jwtService;
        public readonly ILogger<AuthController> _logger;
        public readonly IUserRepository _userRepository;
        private static string errorMessage = "Incorrect Username Or Password";

        public AuthController(ILogger<AuthController> logger, DataContext dbContext, IUserRepository userRepository, JwtService jwtService) {
            //_dbContext = dbContext;
            _logger = logger;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost]
        public IActionResult Post(LoginDto dto) {
            var user = _userRepository.GetAccountByUsername(dto.Username);

            if (user == null) return BadRequest(errorMessage);
            var x = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return BadRequest(errorMessage);
            else {

                var jwt = _jwtService.Generate(user.UserrId);
                Response.Cookies.Append("jwt", jwt, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true });
                return Ok(new { status = "success" });
            }

        }

        [HttpGet("user")]
        public IActionResult getUser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _userRepository.GetAccountById(userId);

                return Ok(new {user.Username, user.UserrId });
            }
            catch (Exception e) {

                return Unauthorized();
            }


        }


        [HttpPost("logout")]
        public IActionResult Logout() {
            Response.Cookies.Delete("jwt");

            return Ok(new { message = "logged out"});

        }
    }
}