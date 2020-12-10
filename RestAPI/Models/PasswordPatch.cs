namespace RestAPI.Models
{
    public class PasswordPatch
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
