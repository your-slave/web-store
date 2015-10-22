using log4net;
using PayPal.Api;
using Store.Domain.Abstract;
using Store.Domain.Entities;
using Store.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace Store.WebUI.Controllers
{
    public class PaymentProcessorController : Controller
    {
        private Payment payment;
        public IOrderProcessor orderProcessor;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PaymentProcessorController(IOrderProcessor proc)
        {
            orderProcessor = proc;
        }

        // GET: Paypal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaypalCheckout()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(Cart cart)
        {
            ShippingDetails shippingDetails = (ShippingDetails)TempData["shipping_details"];

            //getting the apiContext as earlier
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            
            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    var payer = new Payer() { payment_method = "paypal" };

                    var redirUrls = new RedirectUrls()
                    {
                        cancel_url = baseURI + "guid=" + guid,
                        return_url = baseURI + "guid=" + guid
                    };

                    List<Transaction> transactions = GenerateteTransactions(cart);

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    payment = new Payment()
                    {
                        intent = "sale",
                        payer = payer,
                        transactions = transactions,
                        redirect_urls = redirUrls
                    };

                    var createdPayment = payment.Create(apiContext);
                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return View("Failure");
            }

            orderProcessor.ProcessOrder(cart, shippingDetails);
            cart.Clear();

            return View("Success");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        private List<Transaction> GenerateteTransactions(Cart cart)
        {
            var itemList = new ItemList() { items = new List<Item>() };

            foreach(CartLine cartLine in cart.Lines)
            {
                itemList.items.Add(new Item()
                {
                    name = cartLine.Product.Name,
                    currency = "USD",
                    price = cartLine.Product.Price.ToString(),
                    quantity = cartLine.Quantity.ToString(),
                    //sku = "sku"
                });
            }

            var payer = new Payer() { payment_method = "paypal" };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = "0"
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = cart.ComputeTotalValue().ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "your invoice number",
                amount = amount,
                item_list = itemList
            });

            return transactionList;
        }

    }
}