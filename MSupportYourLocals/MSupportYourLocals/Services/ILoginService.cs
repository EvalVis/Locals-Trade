using System.Threading.Tasks;

namespace MSupportYourLocals.Services
{
    public interface ILoginService
    {
        Task Login(string email, string password);
    }
}
