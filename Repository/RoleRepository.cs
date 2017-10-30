using exam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class RoleRepository : Repository<Role>,IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Role> FindRoleById(int id)
        {
            var role = await _context.roles.Where(r => r.Id == id).FirstOrDefaultAsync();
            return role;
        }
    }
}
