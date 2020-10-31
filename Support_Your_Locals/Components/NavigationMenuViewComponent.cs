using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Support_Your_Locals.Models.Repositories;

namespace Support_Your_Locals.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {

        private IServiceRepository repository;

        public NavigationMenuViewComponent(IServiceRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            string category = RouteData?.Values["category"] as string;
            IEnumerable<string> categories = repository.Business.Select(x => x.Product).Distinct().OrderBy(x => x);
            Tuple<string, IEnumerable<string>> data = Tuple.Create(category, categories);
            return View(data);
        }
    }
}
