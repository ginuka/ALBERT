using Microsoft.AspNetCore.Mvc;
using ALBERT.Models;
using ALBERT.Repositories;
using ALBERT.ViewModels;
using System;
using System.Collections.Generic;

namespace ALBERT.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly TableRepository _tableRepository;

        public ReservationsController(IConfiguration configuration)
        {
            _reservationRepository = new ReservationRepository(configuration.GetConnectionString("DefaultConnection"));
            _customerRepository = new CustomerRepository(configuration.GetConnectionString("DefaultConnection"));
            _tableRepository = new TableRepository(configuration.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Index()
        {
            var reservations = _reservationRepository.GetAllReservations();
            return View(reservations);
        }

        public IActionResult Details(int id)
        {
            var reservation = _reservationRepository.GetReservationById(id);
            if (reservation == null) return RedirectToAction("Index");

            return View(reservation);
        }

        public IActionResult Create()
        {
            ViewBag.Customers = _customerRepository.GetAllCustomers();
            ViewBag.Tables = _tableRepository.GetAvailableTables();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateReservationDto dto)
        {
            _reservationRepository.CreateReservation(dto);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var reservation = _reservationRepository.GetReservationById(id);
            if (reservation == null) return RedirectToAction("Index");

            ViewBag.Customers = _customerRepository.GetAllCustomers();
            ViewBag.Tables = _tableRepository.GetAvailableTables();
            return View(reservation);
        }

        [HttpPost]
        public IActionResult Edit(int id, CreateReservationDto dto)
        {
            _reservationRepository.UpdateReservation(id, dto);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var reservation = _reservationRepository.GetReservationById(id);
            if (reservation == null) return RedirectToAction("Index");

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _reservationRepository.DeleteReservation(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult SearchCustomer(string query)
        {
            var customers = _customerRepository.SearchCustomers(query);
            return Json(customers);
        }

        [HttpGet]
        public IActionResult GetCustomerByTable(int tableId)
        {
            var reservation = _reservationRepository.GetActiveReservationByTableId(tableId);
            if (reservation != null)
            {
                var customer = _customerRepository.GetCustomerById(reservation.CustomerId);
                if (customer != null)
                {
                    return Json(new { id = customer.Id, name = customer.Name });
                }
            }
            return Json(null);
        }
    }
}
