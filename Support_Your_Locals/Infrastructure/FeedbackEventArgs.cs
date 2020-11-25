using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Support_Your_Locals.Models;

namespace Support_Your_Locals.Infrastructure
{
    public class FeedbackEventArgs : EventArgs
    {
        public Feedback Feedback;
        public FeedbackEventArgs(Feedback feedback)
        {
            Feedback = feedback;
        }
    }
}
