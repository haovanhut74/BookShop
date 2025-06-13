using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models;

public class Product
{
    public int ProductId { get; set; }
    [Required, MaxLength(500)] public string? ProductName { get; set; }

    [Required, MaxLength(4000)] public string? Description { get; set; }

    [Required, MaxLength(13)] public string? ISBN { get; set; }
    [Required, MaxLength(100)] public string? Author { get; set; }

    [Required] [Range(1, 1000000)] public double Price { get; set; }

    [Range(0, 1000000)] public double DiscountPrice { get; set; } = 0;

    [MaxLength(2000)] public string? ImageUrl { get; set; }

    [Required] public int CategoryId { get; set; }
    [ForeignKey("CategoryId")] public Category? Category { get; set; }

    [Required] public int CoverTypeId { get; set; }
    [ForeignKey("CoverTypeId")] public CoverType? CoverType { get; set; }
}