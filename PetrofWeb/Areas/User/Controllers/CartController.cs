﻿namespace PetrofWeb.Areas.Customer.Controllers
{
    using Petrof.DataAccess.Repository.IRepository;
    using Petrof.Models;
    using Petrof.Models.ViewModels;
    using Petrof.Utility;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Stripe.Checkout;
    using System.Security.Claims;
    using Stripe;
    [Area("User")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            IEnumerable<ProductImage> productImages = _unitOfWork.ProductImage.GetAll();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            //ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            //ShoppingCartVM.OrderHeader.EmailAddress = "";
            //ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            //ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            //ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            //ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            //ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            //TODO: Add validation here
            Dictionary<string, string?> attributes = new() {
                { "OrderHeader.Name", ShoppingCartVM.OrderHeader.Name },
                { "OrderHeader.PhoneNumber", ShoppingCartVM.OrderHeader.PhoneNumber },
                { "OrderHeader.EmailAddress", ShoppingCartVM.OrderHeader.EmailAddress },
                { "OrderHeader.StreetAddress", ShoppingCartVM.OrderHeader.StreetAddress },
                { "OrderHeader.City", ShoppingCartVM.OrderHeader.City },
                { "OrderHeader.State", ShoppingCartVM.OrderHeader.State },
                { "OrderHeader.PostalCode", ShoppingCartVM.OrderHeader.PostalCode } };
            Dictionary<string, string> errorMap = Validator.Parse(attributes);
            foreach ((string attribute, string value) in errorMap)
            {
                if (value != "Success")
                {
                    ModelState.AddModelError(attribute, value);
                }
            }
            if (ModelState.IsValid)
            {
                if (ShoppingCartVM.ShoppingCartList.Any(u => u.Product.InStock == false))
                {
                    ////Remove concurrent purchase attempt
                    ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");
                    List<ShoppingCart> carts = ShoppingCartVM.ShoppingCartList.Where(u => u.Product.InStock == false).ToList();
                    foreach (var cart in carts)
                    {
                        _unitOfWork.ShoppingCart.Remove(cart);
                        _unitOfWork.Save();
                    }
                    TempData["warning"] = "A product in your cart was sold out while you were browsing and has been removed!";
                    return RedirectToAction(nameof(Summary));
                }

                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    cart.Price = GetPriceBasedOnQuantity(cart);
                    ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                }

                if (applicationUser.CompanyId.GetValueOrDefault() == 0)
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                }
                else
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
                }

                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                _unitOfWork.Save();

                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    OrderDetail orderDetail = new()
                    {
                        ProductId = cart.ProductId,
                        OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                        Price = cart.Price,
                        Count = cart.Count
                    };

                    _unitOfWork.OrderDetail.Add(orderDetail);
                    _unitOfWork.Save();
                }

                if (applicationUser.CompanyId.GetValueOrDefault() == 0)
                {
                    var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                    var options = new SessionCreateOptions
                    {
                        SuccessUrl = domain + $"user/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                        CancelUrl = domain + "user/cart/index",
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                    };

                    foreach (var item in ShoppingCartVM.ShoppingCartList)
                    {
                        var sessionLineItem = new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(item.Price * 100),
                                Currency = "eur",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Name
                                }
                            },
                            Quantity = item.Count
                        };
                        options.LineItems.Add(sessionLineItem);
                    }

                    var service = new SessionService();
                    Session session = service.Create(options);
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                    _unitOfWork.Save();
                    Response.Headers.Add("Location", session.Url);

                    return new StatusCodeResult(303);
                }
            }
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
            //return RedirectToAction(nameof(Summary)/*, new { id = ShoppingCartVM.OrderHeader.Id }*/);
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                ////Sell out products
                List<ShoppingCart> carts = _unitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUser.Id, includeProperties: "Product")
                    .Where(u => u.Product.InStock == true).ToList();
                foreach (var cart in carts)
                {
                    var categoriesFromDb = _unitOfWork.Category.Get(u => u.Name.ToLower() == "accessories");
                    if (cart.Product.CategoryId != categoriesFromDb.Id)
                    {
                        cart.Product.InStock = false;
                        _unitOfWork.ShoppingCart.Update(cart);
                        _unitOfWork.Save();
                    }
                }

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }

                HttpContext.Session.Clear();
            }

            //Email sender implementation
            //_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Petrof Music Store", $"<p>New Order Created - {orderHeader.Id}</p>");

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);
            if (cartFromDb.Count <= 1)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private decimal GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            //if (shoppingCart.Count <= 50)
            //{
            //    return shoppingCart.Product.Price;
            //}
            //else if (shoppingCart.Count <= 100)
            //{
            //    return shoppingCart.Product.Price50;
            //}
            //else
            //{
            //    return shoppingCart.Product.Price100;
            //}
            return shoppingCart.Product.Price;
        }
    }
}