using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    /* Repository interface with user safty methods */
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}