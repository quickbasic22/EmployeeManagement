using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Xml;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeManagementContext _employeeRepository;

        public HomeController(EmployeeManagementContext context)
        {
            this._employeeRepository = context;
        }
        public IActionResult Index()
        {
            var model = _employeeRepository.Employees.ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            Employee model = _employeeRepository.GetEmployee(id);
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var employee = _employeeRepository.GetEmployee(id);

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            homeDetailsViewModel.Employee = employee;

            homeDetailsViewModel.PageTitle = "Home Details View Model";
            
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
               
            }
            return View(employee);
        }

    }
}
