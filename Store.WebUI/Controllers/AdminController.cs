using Store.Domain.Abstract;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Store.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public async Task<ViewResult> Index()
        {
            IEnumerable<Product> allproducts = await repository.GetProducts();

            return View(allproducts);
        }

        public async Task<ViewResult> Edit(string IdString)
        {
            Product product = await repository.GetProductById(IdString);
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Product product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageDataBig = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageDataBig, 0, image.ContentLength);

                    using (var img = Image.FromStream(image.InputStream, true, true)) /* Creates Image from specified data stream */
                    {
                        using (var thumb = img.GetThumbnailImage(
                             75, /* width*/
                             75, /* height*/
                             () => false,
                             IntPtr.Zero))
                        {
                            var imgInfo = ImageCodecInfo.GetImageEncoders().Where(codecInfo => codecInfo.MimeType == product.ImageMimeType).First(); /* Returns array of image encoder objects built into GDI+ */
                            using (var encParams = new EncoderParameters(1))
                            {
                                long quality = 100;
                                encParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                                var temp = new MemoryStream();
                                thumb.Save(temp, imgInfo, encParams);
   
                                product.ImageDataSmall = temp.ToArray();
                            }
                        }
                    }
                }

                await repository.SaveProductAsync(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string productId)
        {
            Product deletedProduct = await repository.DeleteProductAsync(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted",
                deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}