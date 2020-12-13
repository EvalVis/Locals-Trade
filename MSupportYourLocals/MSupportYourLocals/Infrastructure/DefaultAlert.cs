using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSupportYourLocals.Infrastructure
{
    public static class DefaultAlert
    {

        public static async Task DisplayFailure(this Page page)
        {
            await page.DisplayAlert("Something went wrong.", "Please try again.", "OK");
        }

    }
}
