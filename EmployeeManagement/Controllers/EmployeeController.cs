using System;
using System.Collections.Generic;
using System.IO;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IWebHostEnvironment webHostEnvironment;

        //constructor injection
        //to get the physical path of the wwwroot folder, we use IWebHostEnvironment 
        public EmployeeController(IEmployeeRepository employeeRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this._employeeRepo = employeeRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        
        public ViewResult Index()
        {
            IEnumerable<Employee> empList = _employeeRepo.GetAllEmployee();
            ViewBag.Title = "Employee Index Page";
            return View(empList);
        }

        public IActionResult Details(int id)
        {
            Employee emp = _employeeRepo.GetEmployee(id);
            if(emp == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            return View(emp);
        }

        public ViewResult About()
        {
            EmployeeAboutViewModel employeeAboutViewModel = new EmployeeAboutViewModel()
            {
                Employee = _employeeRepo.GetEmployee(1),
                PageTitle = "Employee About Page",
            };
            return View(employeeAboutViewModel);
        }

        [HttpGet]
        
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            string uniqueFileName = ProcessUploadPhoto(model);
            if (ModelState.IsValid)
            {
                //assign data from viewModel to Employee model
                Employee newEmp = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName,
                };

                //get the function to add into Database
                _employeeRepo.AddNewEmployee(newEmp);
                return RedirectToAction("Details", new { id = newEmp.Id });
            }
            return View(model);
        }

        private string ProcessUploadPhoto(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                //WebRootPath --> provide the physical path of wwwroot
                //as we need to get to the images of wwwroot, we use combine() to combine the full path
                string uploadFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                //we copy the photo images from view to our images folder using filePath to locate the location and upload to server using FileMode.Create
                //after this block of using() code has been executed sucessfully, the filestream will be disposed 
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public IActionResult Delete(int id)
        {
             _employeeRepo.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        
        public ViewResult Edit(int id)
        {
            Employee emp = _employeeRepo.GetEmployee(id);
            EmployeeEditViewModel empToEdit = new EmployeeEditViewModel
            {
                Id = emp.Id,
                Name = emp.Name,
                Email = emp.Email,
                Department = emp.Department,
                ExistingPhotoPath = emp.PhotoPath
            };
            return View(empToEdit);
        }

        [HttpPost]
        
        public IActionResult Edit(EmployeeEditViewModel empModel)
        {
            //find the employee by id first
            Employee employee = _employeeRepo.GetEmployee(empModel.Id);
            employee.Name = empModel.Name;
            employee.Email = empModel.Email;
            employee.Department = empModel.Department;
            if(empModel.Photo != null)
            {
                //check if the employee has photo already, and delete it first if has.
                if(empModel.ExistingPhotoPath != null)
                {
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath,
                        "images", empModel.ExistingPhotoPath);
                    System.IO.File.Delete(filePath);
                }
                //upload a new selected photo
                employee.PhotoPath = ProcessUploadPhoto(empModel);
            }
            _employeeRepo.UpdateEmployee(employee);
            return RedirectToAction("Details", new { id = employee.Id });
        }

        [Route("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var StatusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Error Request has been found";
                    ViewBag.Path = StatusCodeResult.OriginalPath;
                    ViewBag.QueryString = StatusCodeResult.OriginalQueryString;
                    break;
            }

            return View("Error");
        }
    }
} 
