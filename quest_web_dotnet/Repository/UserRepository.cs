using quest_web.DAL;
using quest_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace quest_web.Repository
{
    public class UserRepository
    {
        private readonly APIDbContext _context;
        public UserRepository(APIDbContext context)
        {
            _context = context;
        }

        public int AddUser(User user)
        {
            User _user = new User { Username = user.Username, Password = user.Password, Role = user.Role };
            _user.Creation_Date = DateTime.Now;
            _user.Updated_Date = _user.Creation_Date;

            if (String.IsNullOrEmpty(_user.Role.ToString()) == true)
                _user.Role = 0;

            var use = _context.Users.Where(u => u.Username == user.Username).FirstOrDefault();
            if (String.IsNullOrEmpty(_user.Username) == true || String.IsNullOrEmpty(_user.Password) == true)
            {
                return -42;
            }
            else if (use != null)
            {
                return -84;
            }
            else
            {
                _user.Password = BCrypt.Net.BCrypt.HashPassword(_user.Password);
                
                _context.Users.Add(_user);
                _context.SaveChanges();
                return 42;
            }
        }
        public UserDetails getInfoUser(string username)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);
            UserDetails us = new UserDetails { Username = currentUser.Username, Role = currentUser.Role, Id = currentUser.ID, Password = currentUser.Password };

            return us;
        }

        public List<User> getAllUser()
        {
            var list = _context.Users.ToList();
            return list;
        }

        public User getInfoUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.ID == id);
            if (user == null)
                return null;
            return user;
        }
    }
}
