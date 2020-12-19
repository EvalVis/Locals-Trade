using System;

namespace Support_Your_Locals.Infrastructure
{
     public class ResponseEventArgs : EventArgs
    {
        public string Email;
        public string Response;
        public ResponseEventArgs(string email, string response)
        {
            Email = email;
            Response = response;
        }
    }
}
