namespace PetrofWeb.Areas.Admin.Controllers
{
    using Petrof.DataAccess.Repository.IRepository;
    using Petrof.Models;
    using Petrof.Utility;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BrandControllers : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandControllers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Brand> objCategoryList = _unitOfWork.Brand.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Brand.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Brand");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Brand? brandFromDb = _unitOfWork.Brand.Get(u => u.Id == id);
            if (brandFromDb == null)
            {
                return NotFound();
            }
            return View(brandFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Brand obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Brand.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Brand updated successfully";
                return RedirectToAction("Index", "Brand");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Brand? brandFromDb = _unitOfWork.Brand.Get(u => u.Id == id);
            if (brandFromDb == null)
            {
                return NotFound();
            }
            return View(brandFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Brand? obj = _unitOfWork.Brand.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Brand.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Brand deleted successfully";
            return RedirectToAction("Index", "Brand");
        }
    }
}