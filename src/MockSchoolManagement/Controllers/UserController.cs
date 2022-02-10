using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using MockSchoolManagement.Models;
using Microsoft.AspNetCore.Identity;
using MockSchoolManagement.ViewModels;

namespace MockSchoolManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ILogger<UserController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user =  await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{id} 的信息不存在!";
                return RedirectToAction("NoFound");
            }

            UserUpdateViewModel model = new UserUpdateViewModel()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                City = user.City
            };

            //增加claims
            var claims = await _userManager.GetClaimsAsync(user);
            model.Claims = claims.Select(c => c.Value).ToList();

            //增加角色
            var roles = await _userManager.GetRolesAsync(user);
            model.Roles = roles.ToList();
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{model.Id} 的信息不存在!";
                return RedirectToAction("NoFound");
            }

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.UserName = model.Name;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
