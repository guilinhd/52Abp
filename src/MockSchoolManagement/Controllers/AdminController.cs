using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using MockSchoolManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace MockSchoolManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(
            ILogger<AdminController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RoleCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.Name
                };

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色Id:{id}的信息不存在,请重试!";
                return RedirectToAction("NoFound", "Error");
            }

            RoleUpdateViewModel model = new RoleUpdateViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Users = new List<UserRoleViewModel>()
            };

            
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                bool isSelected = await _userManager.IsInRoleAsync(user, role.Name);
                model.Users.Add(new UserRoleViewModel()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    IsSelected = isSelected
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleUpdateViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色Id:{model.Id}的信息不存在,请重试!";
                return RedirectToAction("NoFound", "Error");
            }

            ModelState.AddModelError(string.Empty, "角色信息更新成功!");

            #region 更新角色
            role.Name = model.Name;
            await _roleManager.UpdateAsync(role);
            #endregion

            #region 更新用户角色
            for (int i = 0; i < model.Users.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model.Users[i].Id);

                
                IdentityResult result = null;
                //检查当前的userid，是否被选中，如果被选中了则添加到角色中。

                if (model.Users[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }//对于没有选中的则从userroles表中移除。
                else if (!model.Users[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                { //对于其他情况不做处理，继续新的循环。
                    continue;
                }

                if (result.Succeeded)
                {   //判断当前用户是否为最后一个用户，如果是则跳转回EditRole视图，如果不是则进入下一个循环
                    if (i < (model.Users.Count - 1))
                        continue;
                    else
                        return View(model);
                }
            }
            #endregion

            return View(model);
        }

        
    }
}
