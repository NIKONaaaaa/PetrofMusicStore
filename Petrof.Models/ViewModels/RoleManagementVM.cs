namespace Petrof.Models.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    public class RoleManagementVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}