namespace MSupportYourLocals.Services
{
    public class JsonWebTokenHolder
    {
        public string Token { get; set; }

        public void Logout()
        {
            Token = null;
        }
    }
}
