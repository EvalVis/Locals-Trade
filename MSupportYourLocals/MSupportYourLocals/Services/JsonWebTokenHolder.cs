namespace MSupportYourLocals.Services
{
    public class JsonWebTokenHolder
    {
        public bool IsLoggedIn => Token == null;
        public string Token { get; set; }
    }
}
