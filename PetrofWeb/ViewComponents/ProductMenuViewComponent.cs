namespace PetrofWeb.ViewComponents
{
    using Petrof.DataAccess.Repository.IRepository;
    using Microsoft.AspNetCore.Mvc;
    using Petrof.Models;

    public class ProductMenuViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductMenuViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            categories = categories.OrderBy(u => u.DisplayOrder).ToList();
            return View(categories);
        }
    }
}