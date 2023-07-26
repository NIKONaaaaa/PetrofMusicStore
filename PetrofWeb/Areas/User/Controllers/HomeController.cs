namespace PetrofWeb.Areas.Customer.Controllers
{
    using Petrof.DataAccess.Repository.IRepository;
    using Petrof.Models;
    using Petrof.Models.ViewModels;
    using Petrof.Utility;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Session;

    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            Guid guid = Guid.NewGuid();
            HttpContext.Session.SetString("sessionId", guid.ToString());
            string sessionId = HttpContext.Session.GetString("sessionId").ToString();
            return View();
        }

        public IActionResult ProductList(string? productType, string? searchParam)
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,Brand,ProductImages").Where(u => u.InStock == true);

            if (productType != null && productType != "sold")
            {
                productList = productList.Where(u => u.Category.Name == productType);

            }
            else if (productType == "sold")
            {
                productList = _unitOfWork.Product.GetAll(includeProperties: "Category,Brand,ProductImages").Where(u => u.InStock == false);
            }

            ViewData["CurrentFilter"] = searchParam;

            if (!string.IsNullOrEmpty(searchParam))
            {
                productList = productList.Where(u => u.Name.Contains(searchParam));
            }

            return View(productList);
        }             

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,Brand,ProductImages"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDatabase = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

            if (cartFromDatabase != null)
            {
                var product = _unitOfWork.Product.Get(u => u.Id == cartFromDatabase.ProductId);
                var categoriesFromDb = _unitOfWork.Category.Get(u => u.Name.ToLower() == "accessories");
                if (cartFromDatabase.ProductId != shoppingCart.ProductId || product.CategoryId == categoriesFromDb.Id)
                {
                    cartFromDatabase.Count += shoppingCart.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDatabase);
                    _unitOfWork.Save();
                }
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";

            return RedirectToAction(nameof(ProductList));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}