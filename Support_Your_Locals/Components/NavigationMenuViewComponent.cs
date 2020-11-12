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
            string product = RouteData?.Values["product"] as string;
            IEnumerable<string> products = repository.Products.Select(p => p.Name).Distinct().OrderBy(x => x);
            Tuple<string, IEnumerable<string>> data = Tuple.Create(product, products);
            return View(data);
        }
    }
}
