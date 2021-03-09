using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;

        //constructor injection
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this._employeeRepo = employeeRepository;
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
            ViewData["emp"] = emp;
            return View();
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
        public IActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                Employee newEmp = _employeeRepo.AddNewEmployee(emp);
                return RedirectToAction("Details", new { id = newEmp.Id });
            }
            return View(emp);
        }
    }
} 
