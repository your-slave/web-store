using Store.Domain.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Store.WebUI.Controllers
{
    public class NavigationController : Controller
    {
        private IProductRepository repository;
        public NavigationController(IProductRepository repo)
        {
            repository = repo;
        }

        public async Task<PartialViewResult> Menu(string category = null, bool horizontalLayout = false)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = await repository.GetCategories();

            return PartialView("Menu", categories);
        }
    }
}
