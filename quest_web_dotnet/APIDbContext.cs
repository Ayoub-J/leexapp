

using Microsoft.EntityFrameworkCore;
using quest_web.Models;

namespace quest_web.DAL
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options): base(options)
        {
        }

        public APIDbContext()
        { }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet <Address> Address { get; set; }

    }
}