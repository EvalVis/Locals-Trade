namespace RestAPI.Models.BindingTargets
{
    public class PasswordPatch
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
