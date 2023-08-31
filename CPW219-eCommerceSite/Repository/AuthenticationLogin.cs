using CPW219_eCommerceSite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CPW219_eCommerceSite.Data;

namespace CPW219_eCommerceSite.Repository
{
    public class AuthenticateLogin : ILogin
    {
        private readonly UsersContext _context;

        public AuthenticateLogin(UsersContext context)
        {
            _context = context;
        }

        //  Calls the database to see if a username and password exists and return true or false
        public async Task<Users> AuthenticateUser(string username, string password)
        {
            var succeeded = await _context.Users.FirstOrDefaultAsync(authUser => authUser.UserName == username && authUser.Password == password);
            return succeeded;
        }

        public async Task<IEnumerable<Users>> getuser()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
