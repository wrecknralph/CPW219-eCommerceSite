using System.Collections.Generic;
using System.Threading.Tasks;
using CPW219_eCommerceSite.Models;

namespace CPW219_eCommerceSite.Repository
{
    public interface ILogin
    {
        Task<IEnumerable<Users>> getuser();
        Task<Users> AuthenticateUser(string username, string password);
    }
}
