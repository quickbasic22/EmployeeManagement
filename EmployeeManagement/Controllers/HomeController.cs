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
        private readonly EmployeeManagementContext _DbContext;

        public HomeController(EmployeeManagementContext Dbcontext)
        {
            this._DbContext = Dbcontext;
        }
        public IActionResult Index()
        {
            var model = _DbContext.Employees.ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            Employee model = _DbContext.Employees.Find(id);
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var employee = _DbContext.Employees.Find(id);

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
                Employee newEmployee = new Employee();
                newEmployee = employee;
                _DbContext.Employees.Add(newEmployee);
                _DbContext.SaveChanges();
            }
            return View(employee);
        }

    }
}
