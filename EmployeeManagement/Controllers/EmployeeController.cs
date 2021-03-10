using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
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

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if(model.Photo != null)
            {
                //WebRootPath --> provide the physical path of wwwroot
                //as we need to get to the images of wwwroot, we use combine() to combine the full path
                string uploadFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                //we copy the photo images from view to our images folder using filePath to locate the location and upload to server using FileMode.Create
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }
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
            string replacePhotoPath = null;
            Employee empToUpdate = new Employee
            {
                Name = empModel.Name,
                Email = empModel.Email,
                Department = empModel.Department,
                PhotoPath = 
            };
            return RedirectToAction("Details", new { id = empToUpdate.Id });
        }
    }
} 
