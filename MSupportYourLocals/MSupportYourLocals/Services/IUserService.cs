using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IUserService
    {
        Task<bool> Login(string email, string password);
        Task<bool> Register(UserBindingTarget target);
        Task<User> GetUser();
        Task<bool> PatchPassword(string currentPassword, string newPassword);
        Task<bool> PatchEmail(string password, string email);
    }
}
