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

        public AuthenticationController(ILogger<DefaultController> logger, APIDbContext context, JwtTokenUtil jwt)
        {
            _context = context;
            _logger = logger;
            _jwt = jwt;
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        public IActionResult register([FromBody]User user)
        {
            User _user = new User {Username = user.Username, Password = user.Password, Role = user.Role};
            _user.Creation_Date = DateTime.Now;
            _user.Updated_Date = _user.Creation_Date;
             
            if (String.IsNullOrEmpty(_user.Role.ToString()) == true)
                _user.Role = 0;

            var use = _context.Users.Where(u => u.Username == user.Username).FirstOrDefault();
            if (String.IsNullOrEmpty(_user.Username) == true || String.IsNullOrEmpty(_user.Password) == true) {
                return BadRequest("Username or password expected value but is none!");
            } else if (use!=null) {
                return Conflict("Error username already exist!!!");
            } else {
                _user.Password = BCrypt.Net.BCrypt.HashPassword(_user.Password);
                var test = _context.Set<User>();
                test.Add(_user);
                _context.SaveChanges();
                 return StatusCode(201 , "User "  +  _user.Username  + " Role " + _user.Role.ToString() + " was Created!");
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
                return Ok(Token);
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
            
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);
            UserDetails us =  new UserDetails {Username = currentUser.Username, Role = currentUser.Role};

            return us;
        }

    }
}