using Microsoft.Build.Framework;

namespace BookShop.Models;

public class Company
{
    public int CompanyId { get; set; }
    [Required] public string? CompanyName { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LogoImg { get; set; }
    public bool IsAuthorizedCompany { get; set; }
}