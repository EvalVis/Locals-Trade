using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Support_Your_Locals.Infrastructure
{
    public class Imager
    {
        public byte[] ByteMaker (string file)
        {
            string path = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"\Infrastructure\Images\" + file);
            byte[] imgdata = System.IO.File.ReadAllBytes(path);
            return imgdata;
        }
        
    }

    
}
