using System.ComponentModel.DataAnnotations;

namespace BookShop.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Display(Name = "Category Name")]
    [Required, MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;
}