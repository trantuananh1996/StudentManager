using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using exam.Models;

namespace exam.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> FindByEmail(string email);
        Task<User> FindByUsername(string username);
        Task<List<User>> Search(string keyword);
    }
}
