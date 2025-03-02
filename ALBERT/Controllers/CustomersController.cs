using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ALBERT.Models;

namespace ALBERT.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerRepository _customerRepository;

        public CustomersController(IConfiguration configuration)
        {
            _customerRepository = new CustomerRepository(configuration.GetConnectionString("DefaultConnection"));
        }

        // ✅ List all customers
        public IActionResult Index()
        {
            var customers = _customerRepository.GetAllCustomers();
            return View(customers);
        }

        // ✅ Show customer details
        public IActionResult Details(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            if (customer == null) return RedirectToAction("Index");

            return View(customer);
        }

        // ✅ Show create form
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Handle create form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            _customerRepository.CreateCustomer(customer);
            return RedirectToAction("Index");
        }

        // ✅ Show edit form
        public IActionResult Edit(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            if (customer == null) return RedirectToAction("Index");

            return View(customer);
        }

        // ✅ Handle edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            _customerRepository.UpdateCustomer(id, customer);
            return RedirectToAction("Index");
        }

        // ✅ Show delete confirmation
        public IActionResult Delete(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            if (customer == null) return RedirectToAction("Index");

            return View(customer);
        }

        // ✅ Handle delete confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _customerRepository.DeleteCustomer(id);
            return RedirectToAction("Index");
        }


    }
}
