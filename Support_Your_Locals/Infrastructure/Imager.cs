namespace Support_Your_Locals.Infrastructure
{
    public class Imager
    {
        public byte[] ByteMaker (string file)
        {
            byte[] imgdata = System.IO.File.ReadAllBytes($"Infrastructure/Images/{file}");
            return imgdata;
        }
        
    }

    
}
