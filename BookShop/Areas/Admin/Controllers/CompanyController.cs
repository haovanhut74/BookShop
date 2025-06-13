using BookShop.Models;
using BookShop.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Admin.Controllers;

[Area("Admin")]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Upsert(int? id)
    {
        Company Company = new Company();
        if (id == null)
        {
            return View(Company);
        }
        Company = _unitOfWork.Company.GetById(id.GetValueOrDefault());
        return View(Company);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company Company)
    {
        if (ModelState.IsValid)
        {
            if (Company.CompanyId == 0)
            {
                _unitOfWork.Company.Add(Company);
            }
            else
            {
                _unitOfWork.Company.Update(Company);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(Company);
    }
    

    #region API CALL

    [HttpGet]
    public IActionResult GetAll()
    {
        object allObj = _unitOfWork.Company.GetAll().ToList();
        return Json(new { data = allObj });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var obj = _unitOfWork.Company.GetById(id);
        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete success" });
    }
    
    #endregion
}