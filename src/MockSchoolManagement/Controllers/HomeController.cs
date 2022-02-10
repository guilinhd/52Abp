using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MockSchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.DataRepositories;
using System.Text.Json;
using System.Text.Unicode;
using MockSchoolManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace MockSchoolManagement.Controllers
{
    
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ILogger<HomeController> logger, 
            IStudentRepository studentRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {

            return View(_studentRepository.GetStudents());
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult  Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string photoPath = null;
                string imageFolder = _webHostEnvironment.WebRootPath + "/images/";

                if (model.Photos != null)
                {
                    foreach (var item in model.Photos)
                    {
                        photoPath = Guid.NewGuid().ToString() + "_" + item.FileName;

                        string filePath = Path.Combine(imageFolder + photoPath);
                        item.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    
                }


                Student student = new Student()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Major = model.Major,
                    PhotoPath = photoPath
                };
                _studentRepository.Insert(student);
                return RedirectToAction("Details", "Home", new { id = student.Id });
            }

            return View();
        }

        
        public IActionResult Edit(int id)
        {
            StudentUpdateViewModel model = new StudentUpdateViewModel();

            Student student = _studentRepository.GetStudent(id);
            if (student != null)
            {
                model.Id = student.Id;
                model.Name = student.Name;
                model.Email = student.Email;
                model.Major = student.Major;
                model.ExistingPhotoPath = student.PhotoPath;
            }
            else
            {
                Response.StatusCode = 404;
                return View("StudentNoFound", id);
            }
            return View(model);
        }

        //通过模型绑定，作为操作方法的参数
        //StudentEditViewModel 会接收来自Post请求的Edit表单数据
        
        [HttpPost]
        public IActionResult Edit(StudentUpdateViewModel model)
        {

            //检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid)
            {
                //从数据库中查询正在编辑的学生信息
                Student student = _studentRepository.GetStudent(model.Id);
                //用模型对象中的数据更新student对象
                student.Name = model.Name;
                student.Email = model.Email;
                student.Major = model.Major;

                //如果用户想要更改照片，可以上传新照片它会被模型对象上的Photo属性接收
                //如果用户没有上传照片，那么我们会保留现有的照片信息
                //因为兼容了多图上传所有这里的！=null判断修改判断Photos的总数是否大于0
                if (model.Photos.Count > 0)
                {
                    //如果上传了新的照片，则必须显示新的照片信息
                    //因此我们会检查当前学生信息中是否有照片，有的话，就会删除它。
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    //我们将保存新的照片到 wwwroot/images/avatars  文件夹中，并且会更新
                    //Student对象中的PhotoPath属性，然后最终都会将它们保存到数据库中
                    student.PhotoPath = ProcessUploadedFile(model);
                }

                //调用仓储服务中的Update方法，保存studnet对象中的数据，更新数据库表中的信息。
                Student updatedstudent = _studentRepository.Update(student);

                return RedirectToAction("index");
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            //return Json(_studentRepository.GetStudent(1), new JsonSerializerOptions()
            //{
            //    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
            //});

            //throw new Exception("Details 视图出错了!");
            Student student = _studentRepository.GetStudent(id);
            if (student == null)
            {
                ViewBag.ErrorMessage = $"学生Id:{id} 的信息不存在!";
                return View("NoFound");
            }
            else
            {
                return View(_studentRepository.GetStudent(id));
            }

            //_logger.LogTrace("Trace Log");
            //_logger.LogInformation("Information Log");
            //_logger.LogDebug("Debug Log");
            //_logger.LogWarning("Warning Log");
            //_logger.LogError("Error Log");
            //_logger.LogCritical("Critical Log");

            //return View(_studentRepository.GetStudent(id));
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 将照片保存到指定的路径中，并返回唯一的文件名
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    //必须将图像上传到wwwroot中的images/avatars文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的webHostEnvironment服务
                    //通过webHostEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    //因为使用了非托管资源，所以需要手动进行释放
                    var fileStream = new FileStream(filePath, FileMode.Create);

                    //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images/avatars 文件夹
                    photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
