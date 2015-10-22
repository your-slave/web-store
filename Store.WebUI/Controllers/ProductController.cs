using Store.Domain.Abstract;
using Store.Domain.Entities;
using Store.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Store.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public async Task<ViewResult> List(string category, int page = 1)
        {

            IEnumerable<Product> products = await repository.GetProductsByCategory(category);

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = products,

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,

                    TotalItems = products.Count()
                },

                CurrentCategory = category

            };

            return View(model);
        }

        public async Task<FileContentResult> GetImageBig(string idString)
        {
            Product prod = await repository.GetProductById(idString);

            if (prod != null)
            {
                return File(prod.ImageDataBig, prod.ImageMimeType);
            }
            else
            { 
                return null;
            }
        }

        public async Task<FileContentResult> GetImageSmall(string idString)
        {
            Product prod = await repository.GetProductById(idString);

            if (prod != null)
            {
                return File(prod.ImageDataSmall, prod.ImageMimeType);
            }
            else
            { 
                return null;
            }
        }
    }
}