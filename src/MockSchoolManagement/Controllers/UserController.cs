﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using MockSchoolManagement.Models;
using Microsoft.AspNetCore.Identity;
using MockSchoolManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MockSchoolManagement.Controllers
{
    
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(ILogger<UserController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user =  await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{id} 的信息不存在!";
                return View("NoFound");
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
            model.Claims = claims.Where(c=>c.Value.ToLower() == "true").Select(c => c.Type).ToList();

            //增加角色
            var roles = await _userManager.GetRolesAsync(user);
            model.Roles = roles.ToList();
            
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{model.Id} 的信息不存在!";
                return View("NoFound");
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{id} 的信息不存在!";
                return View("NoFound");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Role(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{id} 的信息不存在!";
                return View("NoFound");
            }

            ViewBag.Id = id;
            List<UserRoleViewModel> models = new List<UserRoleViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                bool isSelected = await _userManager.IsInRoleAsync(user,role.Name);
                models.Add(new UserRoleViewModel() { 
                    Id = role.Id,
                    Name = role.Name,
                    IsSelected = isSelected
                });
            }

            return View(models);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Role(List<UserRoleViewModel> models, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{id} 的信息不存在!";
                return View("NoFound");
            }

            #region 更新用户角色
            for (int i = 0; i < models.Count; i++)
            {
                
                IdentityResult result = null;
                //检查当前的userid，是否被选中，如果被选中了则添加到角色中。
                string role = models[i].Name;
                if (models[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role)))
                {
                    result = await _userManager.AddToRoleAsync(user, role);
                }//对于没有选中的则从userroles表中移除。
                else if (!models[i].IsSelected && await _userManager.IsInRoleAsync(user, role))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role);
                }
                else
                { //对于其他情况不做处理，继续新的循环。
                    continue;
                }

                if (result.Succeeded)
                {   //判断当前用户是否为最后一个用户，如果是则跳转回EditRole视图，如果不是则进入下一个循环
                    if (i < (models.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Edit", new { id });
                }
            }
            #endregion

            return RedirectToAction("Edit", new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Claim(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{id} 的信息不存在!";
                return View("NoFound");
            }

            UserClaimViewModel model = new UserClaimViewModel()
            {
                Id = id,
                Claims = new List<UserRoleViewModel>()
            };

            var claims = await _userManager.GetClaimsAsync(user);

            foreach (var item in ClaimStore.AllClaims)
            {
                bool isSelected = claims.Where(f => f.Type == item.Type && f.Value.ToLower() == "true").Any();
                model.Claims.Add(new UserRoleViewModel()
                {
                    Name = item.Type,
                    IsSelected = isSelected
                });

            }


            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Claim(UserClaimViewModel model)
        {
            string id = model.Id;
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户Id：{id} 的信息不存在!";
                return View("NoFound");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (result.Succeeded)
            {
                foreach (var item in model.Claims)
                {
                    if (item.IsSelected)
                    {
                        await _userManager.AddClaimAsync(user, new Claim(item.Name, item.IsSelected.ToString()));
                    }
                }

                return RedirectToAction("Edit", new { id});
            }
            

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!(await _userManager.HasPasswordAsync(user)))
            {
                return View("AddPassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return View("ChangePasswordConfirm");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return View("AddPasswordConfirmation");
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
