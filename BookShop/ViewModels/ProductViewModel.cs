using BookShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.ViewModels;

public class ProductViewModel
{
    public Product Product { get; set; } = new();
    public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> CoverTypeList { get; set; } = new List<SelectListItem>();
}