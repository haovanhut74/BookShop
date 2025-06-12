using BookShop.Models;
using BookShop.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
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
        Category category = new Category();
        if (id == null)
        {
            return View(category);
        }
        category = _unitOfWork.Category.GetById(id.GetValueOrDefault());
        if(category == null)
            return NotFound();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Category category)
    {
        if (ModelState.IsValid)
        {
            if (category.CategoryId == 0)
            {
                _unitOfWork.Category.Add(category);
            }
            else
            {
                _unitOfWork.Category.Update(category);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }
    

    #region API CALL

    [HttpGet]
    public IActionResult GetAll()
    {
        object allObj = _unitOfWork.Category.GetAll().ToList();
        return Json(new { data = allObj });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var obj = _unitOfWork.Category.GetById(id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while delete" });
        }
        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete success" });
    }
    
    #endregion
}