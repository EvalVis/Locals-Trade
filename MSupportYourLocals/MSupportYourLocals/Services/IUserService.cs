using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IUserService
    {
        Task<User> GetUser();
    }
}
