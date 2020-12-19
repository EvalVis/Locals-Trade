using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSupportYourLocals.Infrastructure
{
    public static class DefaultAlert
    {

        public static async Task DisplayFailure(this Page page)
        {
            await page.DisplayAlert("Something went wrong", "Please try again.", "OK");
        }

        public static async Task DisplaySuccess(this Page page, string message)
        {
            await page.DisplayAlert("Success", message, "OK");
        }

    }
}
