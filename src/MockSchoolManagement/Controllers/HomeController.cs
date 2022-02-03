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

        
        public IActionResult Index()
        {

            return View(_studentRepository.GetStudents());
        }

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
                if (model.Photos != null)
                {
                    string imageFolder = _webHostEnvironment.WebRootPath + "/images/";
                    photoPath = Guid.NewGuid().ToString() + "_" + model.Photos.FileName;

                    string filePath = Path.Combine(imageFolder + photoPath);
                    model.Photos.CopyTo(new FileStream(filePath, FileMode.Create));
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

        public IActionResult Details(int id)
        {
            //return Json(_studentRepository.GetStudent(1), new JsonSerializerOptions()
            //{
            //    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
            //});

            return View(_studentRepository.GetStudent(id));
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
