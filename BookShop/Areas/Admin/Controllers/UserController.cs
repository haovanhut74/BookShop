using BookShop.Data;
using BookShop.Models;
using BookShop.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _dbcontext;

    public UserController(ApplicationDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }


    #region API CALL

    [HttpGet]
    public IActionResult GetAll()
    {
        var userList = _dbcontext.Users.Include(i => i.Company).ToList();
        var userRole = _dbcontext.UserRoles.ToList();
        var roles = _dbcontext.Roles.ToList();
        foreach (var user in userList)
        {
            var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
            user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            if (user.Company == null)
            {
                user.Company = new Company()
                {
                    CompanyName = "No Company",
                };
            }
        }

        return Json(new { data = userList });
    }

    [HttpPost]
    public IActionResult LockUnlock([FromBody] string id)
    {
        var user = _dbcontext.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return Json(new { success = true, message = "Error while lock/unlock" });
        }

        if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
        {
            user.LockoutEnd = DateTime.Now;
        }
        else
        {
            user.LockoutEnd = DateTime.Now.AddYears(1000);
        }

        _dbcontext.SaveChanges();
        return Json(new { success = true, message = "Lock/Unlock success" });
    }

    #endregion
}