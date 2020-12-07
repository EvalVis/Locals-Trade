using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IRegisterUserService
    {
        Task Register(User user);
    }
}
