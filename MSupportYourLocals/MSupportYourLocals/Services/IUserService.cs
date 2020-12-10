using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IUserService
    {
        Task Login(string email, string password);
        Task Register(UserBindingTarget target);
        Task<User> GetUser();
    }
}
