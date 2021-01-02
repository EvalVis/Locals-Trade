using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Support_Your_Locals.Models
{
    public class Product
    {
        public long ProductID { get; set; }
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Unit { get; set; }
        public string Comment { get; set; }
        [NotMapped]
        public string Picture 
        {
            get
            {
                string imageBase64Data = Convert.ToBase64String(PictureData ?? File.ReadAllBytes("Content/Images/no-image.png"));
                return string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }
        }
        public byte[] PictureData { get; set; }
        public long BusinessID { get; set; }
        public Business Business { get; set; }
        public List<Order> Orders { get; set; }
    }
}
