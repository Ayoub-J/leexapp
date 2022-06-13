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
    public class UserController : Controller
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly APIDbContext _context;
        private readonly JwtTokenUtil _jwt;
        private readonly UserRepository _userRepository;

        public UserController(ILogger<DefaultController> logger, APIDbContext context, JwtTokenUtil jwt)
        {
            _context = context;
            _logger = logger;
            _jwt = jwt;
            _userRepository = new UserRepository(context);
        }

        [Authorize]
        [HttpGet("/user")]
        public async Task<IEnumerable<User>> GetUser()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string token = accessToken.ToString();
            var username = _jwt.GetUsernameFromToken(token);
//            var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);


            return _userRepository.getAllUser();
        }

        [Authorize]
        [HttpGet("/user/{id}")]
        public async Task<User> GetUserById(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string token = accessToken.ToString();
            var username = _jwt.GetUsernameFromToken(token);
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);

            return _userRepository.getInfoUserById(id);
            //return StatusCode(201 , "Street "  +  _address.road  + " City " + _address.City + " was Created!");
        }

        [Authorize]
        [HttpPut("/user")]
        public async Task<User> ChangeUser([FromBody] User address)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string token = accessToken.ToString();
            var username = _jwt.GetUsernameFromToken(token);
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);

            var user = _context.Users.FirstOrDefault(u => u.ID == currentUser.ID);
            if (user == null)
                return null;
            
            user.Role = address.Role;
            if (address.Username != null)
                user.Username = address.Username;

            if (currentUser.Role.Equals(UserRole.ROLE_ADMIN) || currentUser.ID == user.ID) {
                _context.Update(user);
                await _context.SaveChangesAsync();

                return user;
            }
            return null;
        }

        [Authorize]
        [HttpDelete("/user/{id}")]
        public async Task<String> DeleteAddressById(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string token = accessToken.ToString();
            var username = _jwt.GetUsernameFromToken(token);
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);

            var user = _context.Users.FirstOrDefault(u => u.ID == id);
            if (user == null)
                return "Echec";
            if (currentUser.Role.Equals(UserRole.ROLE_ADMIN) || currentUser.ID == user.ID ) {
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return "Success";
            }
            return "Error";
        }


    }
}