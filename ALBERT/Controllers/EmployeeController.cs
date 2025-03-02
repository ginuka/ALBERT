using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ALBERT.Models;
using ALBERT.Repositories;

namespace ALBERT.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _employeeRepository = new EmployeeRepository(connectionString);
        }

        // 🔹 List all employees
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAllEmployees();
            return View(employees);
        }

        // 🔹 Show employee details
        public IActionResult Details(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null) return RedirectToAction("Index");
            return View(employee);
        }

        // 🔹 Create Employee (GET)
        public IActionResult Create()
        {
            ViewBag.Roles = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
            return View();
        }

        // 🔹 Create Employee (POST)
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
                return View(employee);
            }

            _employeeRepository.CreateEmployee(employee);
            return RedirectToAction("Index");
        }

        // 🔹 Edit Employee (GET)
        public IActionResult Edit(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null) return RedirectToAction("Index");

            ViewBag.Roles = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
            return View(employee);
        }

        // 🔹 Edit Employee (POST)
        [HttpPost]
        public IActionResult Edit(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>();
                return View(employee);
            }

            _employeeRepository.UpdateEmployee(id, employee);
            return RedirectToAction("Index");
        }

        // 🔹 Delete Employee (GET)
        public IActionResult Delete(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null) return RedirectToAction("Index");

            return View(employee);
        }

        // 🔹 Delete Employee (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _employeeRepository.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
    }
}
