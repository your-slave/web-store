using Store.Domain.Abstract;
using Store.Domain.Entities;
using Store.WebUI.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Store.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;

        public CartController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public async Task<RedirectToRouteResult> AddToCart(Cart cart, string IdString, string returnUrl)
        {
            Product product = await repository.GetProductById(IdString);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public async Task<RedirectToRouteResult> RemoveFromCart(Cart cart, string productId, string returnUrl)
        {
            Product product = await repository.GetProductById(productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult ShippingDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShippingDetails(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                TempData["shipping_details"] = shippingDetails;
                return RedirectToAction("PaymentWithPaypal", "PaymentProcessor");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        //private Cart GetCart()
        //{
        //    Cart cart = (Cart)Session["Cart"]; //coockiess!!! O_O
        //    if (cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}
    }
}