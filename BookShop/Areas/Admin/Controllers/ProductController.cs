using BookShop.Models;
using BookShop.Repository.IRepository;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Upsert(int? id)
    {
        ProductViewModel productVM = new ProductViewModel()
        {
            Product = new Product(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.CategoryName,
                Value = i.CategoryId.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.CoverTypeName,
                Value = i.CoverTypeId.ToString()
            })
        };
        if (id == null)
        {
            return View(productVM);
        }

        productVM.Product = _unitOfWork.Product.GetById(id.GetValueOrDefault());
        return View(productVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductViewModel productVM)
    {
        if (ModelState.IsValid)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, "images", "products");

                if (!Directory.Exists(upload))
                {
                    Directory.CreateDirectory(upload);
                }

                var extension = Path.GetExtension(files[0].FileName);

                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                {
                    var oldFilePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                var filePath = Path.Combine(upload, fileName + extension);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }
            else
            {
                if (productVM.Product.ProductId != 0)
                {
                    productVM.Product.ImageUrl = _unitOfWork.Product.GetById(productVM.Product.ProductId).ImageUrl;
                }
            }

            if (productVM.Product.ProductId == 0)
            {
                _unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
        {
            Text = i.CategoryName,
            Value = i.CategoryId.ToString()
        });
        productVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
        {
            Text = i.CoverTypeName,
            Value = i.CoverTypeId.ToString()
        });

        if (productVM.Product.ProductId != 0)
        {
            productVM.Product = _unitOfWork.Product.GetById(productVM.Product.ProductId);
        }

        return View(productVM);
    }


    #region API CALL

    [HttpGet]
    public IActionResult GetAll()
    {
        object allObj = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return Json(new { data = allObj });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        
        var obj = _unitOfWork.Product.GetById(id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while delete" });
        }
        // Delete image file if exists
        if (!string.IsNullOrEmpty(obj.ImageUrl))
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            var oldFilePath = Path.Combine(webRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
        }
        _unitOfWork.Product.Remove(obj);
        
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete success" });
    }

    #endregion
}