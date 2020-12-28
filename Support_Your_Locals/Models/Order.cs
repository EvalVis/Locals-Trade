using System;

namespace Support_Your_Locals.Models
{
    public class Order
    {
        
        public long Id { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public int Count { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public DateTime DateAdded { get; set; }

    }
}
