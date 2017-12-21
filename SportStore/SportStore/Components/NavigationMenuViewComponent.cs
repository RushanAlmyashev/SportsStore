using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SportStore.Models;

namespace SportStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository repository;

        public NavigationMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            var result = repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);
            return View(result);
        }

        //public string Invoke()
        //{
        //    return "Hello from the Nav View Component";
        //}
    }
}
