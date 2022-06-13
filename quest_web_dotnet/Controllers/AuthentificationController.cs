using System.Text.Encodings.Web;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using quest_web.Models;
using quest_web.DAL;
using quest_web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System;
using quest_web.Repository;

namespace MvcMovie.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly APIDbContext _context;
        private readonly JwtTokenUtil _jwt;
        public readonly UserRepository _userRepository;

        public AuthenticationController(ILogger<DefaultController> logger, APIDbContext context, JwtTokenUtil jwt)
        {
            _context = context;
            _logger = logger;
            _userRepository = new UserRepository(_context);
            _jwt = jwt;
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        public IActionResult register([FromBody]User user)
        {
            int res = _userRepository.AddUser(user);
            if (res == -42) {
                return BadRequest("Username or password expected value but is none!");
            } else if (res == -84) {
                return Conflict("Error username already exist!!!");
            } else {
                 return StatusCode(201 , "User "  +  user.Username   + " was Created!");
            }
        }

        [AllowAnonymous]
        [HttpPost("/authenticate")]
        public IActionResult authenticate([FromBody]User user)
        {
            UserDetails us = new UserDetails {Username = user.Username, Password = user.Password, Role = user.Role};
            try {
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
                if (BCrypt.Net.BCrypt.Verify(user.Password, currentUser.Password) == false)
                    return StatusCode(401 , "Password is not valid");
                var Token = _jwt.GenerateToken(us);
                return StatusCode(200, Token);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return StatusCode(401 , "User doesn't exist");
            }
        }

        [Authorize]
        [HttpGet("/me")]
        public async Task<UserDetails> me()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string token = accessToken.ToString();
            var username = _jwt.GetUsernameFromToken(token);
            
            

            return _userRepository.getInfoUser(username);
        }

    }
}