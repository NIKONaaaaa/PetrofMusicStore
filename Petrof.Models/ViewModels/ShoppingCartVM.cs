namespace Petrof.Models.ViewModels
{
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    public class ShoppingCartVM
    {
        [ValidateNever]
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
    }
}