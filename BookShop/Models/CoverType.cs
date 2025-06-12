using System.ComponentModel.DataAnnotations;

namespace BookShop.Models;

public class CoverType
{
    [Key]
    public int CoverTypeId { get; set; }
    
    [Display(Name = "Cover Type Name")]
    [Required, MaxLength(100)]
    public string CoverTypeName { get; set; } = string.Empty;
}