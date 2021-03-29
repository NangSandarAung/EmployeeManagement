using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                    UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = model.RoleName,
                };

                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserRolesList", "Administration");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }

        public IActionResult UserRolesList()
        {
            var list = roleManager.Roles;
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var result = await roleManager.FindByIdAsync(id);

            var model = new EditRoleViewModel
            {
                Id = result.Id,
                RoleName = result.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, result.Name))
                {
                    model.Users.Add(user.UserName);
                }

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                role.Name = model.RoleName;

                //if (model.Users.Any())
                //{
                //    foreach(var user in model.Users)
                //    {
                //        var u = await userManager.FindByNameAsync(user);
                //        await userManager.AddToRoleAsync(u, role.Name);
                //    }
                //}

                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserRolesList", "Administration");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            ViewBag.roleId = id;
            var role = await roleManager.FindByIdAsync(id);

            //create an instance of List of UserRoleViewModel - because we use UserRoleViewModel to pass data to view
            List<UsersRoleViewModel> model = new List<UsersRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                //create an instance of UserRoleViewModel to assign each user's value to that model
                var userRole = new UsersRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                //check whether user is already in that role
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }

                model.Add(userRole);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UsersRoleViewModel> model,
                                                           string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            for(int i = 0; i < model.Count; i++)
            {
                //get user 
                var u = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsSelected == true && !(await userManager.IsInRoleAsync(u, role.Name)))
                {
                    result =  await userManager.AddToRoleAsync(u, role.Name);
                }
                else if(!model[i].IsSelected == true && await userManager.IsInRoleAsync(u, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(u, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { id = id });
                }
                
            }
            
            return RedirectToAction("EditRole", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> DeletRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("UserRolesList");
            }
            foreach(var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            return RedirectToAction("UserRolesList"); 
        }

        public IActionResult ListUser()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var userRoles = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user);

            var userVModel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Roles = userRoles,
                Claims = userClaims.Select(c => c.Value).ToList()
            };

            return View(userVModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);

                }
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var result =  await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUser");
            }
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);

            }
            return View("ListUser");
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRole(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var model = new List<User_RoleViewModel>();

            foreach (var role in roleManager.Roles)
            { 
                var userRole = new User_RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,   
                };
               
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }

                model.Add(userRole);
            }
            ViewBag.UserId = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRole(List<User_RoleViewModel> model,
                                                          string id)
        {
            var user = await userManager.FindByIdAsync(id);

            for (int i = 0; i < model.Count; i++)
            {
                //get role 
                var role = await roleManager.FindByIdAsync(model[i].RoleId);
                IdentityResult result = null;

                if (model[i].IsSelected == true && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected == true && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditUser", new { id = id });
                }

            }

            return RedirectToAction("EditUser", new { id = id });
        }
    }
}
