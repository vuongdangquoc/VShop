using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Enums;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(string? search,
             int? categoryId,
             bool? status,
             SortBy sortBy,
             bool isDescending,
             int page = 1)
        {
            int pageSize = 6;
            var list = await _productService.GetAllProductsInAdminAsync(search, categoryId, status, sortBy, isDescending);
            var categories = await _categoryService.GetAllCategory();
            ViewBag.Categories = categories;
            var num = list.Count;
            var count = Math.Ceiling( (decimal)num/pageSize);

            var listInPage = list.Skip((page - 1) * pageSize)
                               .Take(pageSize).ToList();

            ViewData["count"] = count;
            ViewData["search"] = search;
            ViewData["status"] = status;
            ViewData["categoryId"] = categoryId;
            ViewData["sortBy"] = sortBy;
            ViewData["isDescending"] = isDescending;
            ViewData["page"] = page;

           
            return View(listInPage);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryService.GetAllCategory();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateProductDTO productDTO,IFormFile? mainImageUpload, IFormFile[]? additionalImagesUpload)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Admin/Product");
            }

            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string baseDirectory = "UploadFiles\\Images";
            string newFolder = Path.Combine(webRootPath, baseDirectory, productDTO.Name);
            //tao folder chua image cua product
            Directory.CreateDirectory(newFolder);
            if(mainImageUpload!= null)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{mainImageUpload.FileName}";
                var filePath = Path.Combine(newFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    mainImageUpload.CopyTo(stream);
                }
                productDTO.Image = (baseDirectory+"\\"+productDTO.Name+"\\" + uniqueFileName).Replace("\\", "/");
            }
            else
            {
                return View(productDTO);
            }
            if(additionalImagesUpload != null)
            {
                var listAdditionalImages = new List<string>();
                for (int i = 0; i < additionalImagesUpload.Length; i++)
                {
                    string fileName = $"{Guid.NewGuid()}_{additionalImagesUpload[i].FileName}";
                    var filePath = Path.Combine (newFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        additionalImagesUpload[i].CopyTo(stream);
                    }
                    listAdditionalImages.Add((baseDirectory+"\\"+productDTO.Name+"\\"+fileName).Replace("\\","/"));
                }

                productDTO.ListImages = JsonConvert.SerializeObject(listAdditionalImages);
            }
            
            await _productService.CreateProduct(productDTO);

            return Redirect("/Admin/Product");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryService.GetAllCategory();
            ViewBag.Categories = categories;
            var product = await _productService.GetUpdateProductDTO(id);
            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryID,Quantity,Describe,Price,PurchasePrice,Discount,UpdatedBy,Status")] UpdateProductDTO productDTO, IFormFile? mainImageUpload, IFormFile[]? additionalImagesUpload)
        {
           if (id != productDTO.Id)
            {
                return NotFound("/Admin/Product");
            }

            if (ModelState.IsValid)
            {
                try
                {

                    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var product = await _productService.GetUpdateProductDTO(id);

                    if (product.Name != productDTO.Name)
                    {
                        string baseDirectory = "UploadFiles\\Images";
                        string oldProductName = product.Name;
                        string newProductName = productDTO.Name;

                        product.Name = productDTO.Name;

                        string oldPath = Path.Combine(webRootPath,baseDirectory, oldProductName);
                        string newPath = Path.Combine(webRootPath, baseDirectory, newProductName);

                        if (Directory.Exists(oldPath))
                        {
                            Directory.Move(oldPath, newPath);
                            Console.WriteLine($"Đã đổi tên thư mục: {oldPath} -> {newPath}");
                        }
                        else
                        {
                            Directory.CreateDirectory(newPath);
                            Console.WriteLine($"Tạo thư mục mới: {newPath}");
                        }
                    }

                    string uploadFolder = Path.Combine("UploadFiles", "Images", product.Name);

                    if (mainImageUpload != null)
                    {
                        string uniqueFileName = $"{Guid.NewGuid()}_{mainImageUpload.FileName}";

                        string filePath = Path.Combine(webRootPath, uploadFolder, uniqueFileName);

                        if (!string.IsNullOrEmpty(productDTO.Image))
                        {
                            string oldFilePath = Path.Combine(webRootPath,uploadFolder, productDTO.Image);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Đảm bảo thư mục tồn tại trước khi lưu file
                        string directoryPath = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // Lưu file mới
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            mainImageUpload.CopyTo(stream);
                        }

                        productDTO.Image = Path.Combine(uploadFolder, uniqueFileName).Replace("\\", "/");
                    }

                    //===========Save Galleries==========
                    if (additionalImagesUpload != null && additionalImagesUpload.Any())
                    {

                        // Đảm bảo thư mục tồn tại
                        string fullUploadPath = Path.Combine(webRootPath, uploadFolder);
                        if (!Directory.Exists(fullUploadPath))
                        {
                            Directory.CreateDirectory(fullUploadPath);
                        }
                        else
                        {
                            var listOldImages = product.ListImages.Split(",");
                            for (int i = 0; i < listOldImages.Length; i++)
                            {
                                listOldImages[i] = Path.Combine(webRootPath, listOldImages[i]).Replace("\\", "/");
                                System.IO.File.Delete(listOldImages[i]);
                            }
                        }

                        List<string> listImages = new List<string>();

                        foreach (var image in additionalImagesUpload)
                        {
                            // Tạo tên file mới tránh trùng lặp
                            string imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                            string filePath = Path.Combine(fullUploadPath, imageName);

                            // Lưu file mới
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                image.CopyTo(stream);
                            }

                            // Thêm đường dẫn mới vào danh sách
                            listImages.Add(Path.Combine(uploadFolder, imageName).Replace("\\", "/"));
                        }

                        // Cập nhật danh sách ảnh mới vào productDTO.ListImages
                        productDTO.ListImages = JsonConvert.SerializeObject(listImages);

                    }

                    //================================
                    await _productService.EditProduct(productDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return Redirect("/Admin/Product");
            }
            return Redirect("/Admin/Product/Edit/"+productDTO.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await  _productService.GetProductById(id);
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string baseDirectory = "UploadFiles\\Images";
            var folderProduct = Path.Combine(webRootPath, baseDirectory, product.Name);
            if (Directory.Exists(folderProduct))
            {
                // Xóa tất cả file trong thư mục
                foreach (var file in Directory.GetFiles(folderProduct))
                {
                    System.IO.File.Delete(file);
                }

                // Xóa tất cả thư mục con
                foreach (var dir in Directory.GetDirectories(folderProduct))
                {
                    Directory.Delete(dir, true);
                }

                // Xóa thư mục chính
                Directory.Delete(folderProduct, true);
            }
            await _productService.DeleteProduct(id);
            return Redirect("/Admin/Product");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _productService.ChangeStatusProduct(id, User.FindFirst(ClaimTypes.Name)?.Value);
            return Redirect("/admin/product");
        }
    }
}
