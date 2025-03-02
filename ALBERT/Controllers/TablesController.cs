using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ALBERT.Models;
using ALBERT.Repositories;

namespace ALBERT.Controllers
{
    public class TablesController : Controller
    {
        private readonly TableRepository _tableRepository;

        public TablesController(IConfiguration configuration)
        {
            _tableRepository = new TableRepository(configuration.GetConnectionString("DefaultConnection"));
        }

        // ✅ GET: Tables
        public IActionResult Index()
        {
            var tables = _tableRepository.GetAllTables();
            return View(tables);
        }

        // ✅ GET: Tables/Details/5
        public IActionResult Details(int id)
        {
            var table = _tableRepository.GetTableById(id);
            if (table == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // ✅ GET: Tables/Create
        public IActionResult Create()
        {
            return View();
        }

        // ✅ POST: Tables/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Table table)
        {
            if (ModelState.IsValid)
            {
                _tableRepository.CreateTable(table);
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // ✅ GET: Tables/Edit/5
        public IActionResult Edit(int id)
        {
            var table = _tableRepository.GetTableById(id);
            if (table == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // ✅ POST: Tables/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Table table)
        {
            if (ModelState.IsValid)
            {
                _tableRepository.UpdateTable(id, table);
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // ✅ GET: Tables/Delete/5
        public IActionResult Delete(int id)
        {
            var table = _tableRepository.GetTableById(id);
            if (table == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // ✅ POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _tableRepository.DeleteTable(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
