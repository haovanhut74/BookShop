using BookShop.Models;
using BookShop.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Admin.Controllers;

[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
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
        CoverType coverType = new CoverType();
        if (id == null)
        {
            return View(coverType);
        }
        coverType = _unitOfWork.CoverType.GetById(id.GetValueOrDefault());
        return View(coverType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(CoverType coverType)
    {
        if (ModelState.IsValid)
        {
            if (coverType.CoverTypeId == 0)
            {
                _unitOfWork.CoverType.Add(coverType);
            }
            else
            {
                _unitOfWork.CoverType.Update(coverType);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(coverType);
    }
    

    #region API CALL

    [HttpGet]
    public IActionResult GetAll()
    {
        object allObj = _unitOfWork.CoverType.GetAll().ToList();
        return Json(new { data = allObj });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var obj = _unitOfWork.CoverType.GetById(id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while delete" });
        }
        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete success" });
    }
    
    #endregion
}