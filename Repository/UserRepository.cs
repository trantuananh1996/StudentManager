using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Models;
using Microsoft.EntityFrameworkCore;

namespace exam.Repository
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
           
        }

        public async Task<User> FindByEmail(string email)
        {
            var user = await _context.Users.Where(u => u.email == email).Include("Role").FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> FindByUsername(string username)
        {
            return await _context.Users.Where(u => u.username == username).Include("Role").FirstOrDefaultAsync();
        }

        public async Task<List<User>> Search(string keyword)
        {
            var users = await _context.Users.Where(u => u.username.Contains(keyword) ||
                                             u.email.Contains(keyword))
                               .Include("Role").ToListAsync();
            return users;
        }
        public new async Task<List<User>> getAll()
        {
            return await _context.Set<User>().Include("Role").ToListAsync();
        }
    }
}
