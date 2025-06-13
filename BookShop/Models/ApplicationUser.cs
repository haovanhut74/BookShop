using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string UserName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int? CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    public Company Company { get; set; }
    
    [NotMapped]
    public string Role { get; set; }
}