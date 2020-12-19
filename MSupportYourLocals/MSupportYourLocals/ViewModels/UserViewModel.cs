using MSupportYourLocals.Models;

namespace MSupportYourLocals.ViewModels
{
    public class UserViewModel
    {

        public User User { get; set; }

        public UserViewModel(User user)
        {
            User = user;
        }

    }
}
