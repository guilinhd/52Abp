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

namespace MockSchoolManagement.Controllers
{
    
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentRepository _studentRepository;

        public HomeController(ILogger<HomeController> logger, IStudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
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
        public IActionResult  Create(Student model)
        {
            if (ModelState.IsValid)
            {
                Student student = _studentRepository.Add(model);
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
