using Microsoft.AspNetCore.Mvc;

namespace Support_Your_Locals.Components
{
    public class SearchBusinessViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
