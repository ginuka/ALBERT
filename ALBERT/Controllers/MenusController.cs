using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ALBERT.Data;
using ALBERT.Models;
using ALBERT.ViewModels;

namespace ALBERT.Controllers
{
    public class MenusController : Controller
    {
        private readonly MenuRepository _menuRepository;
        private readonly IConfiguration _configuration;
        public MenusController(IConfiguration configuration)
        {
            _configuration = configuration;
            _menuRepository = new MenuRepository(_configuration.GetConnectionString("DefaultConnection"));
        }

        public ActionResult Index()
        {
            var menus = _menuRepository.GetAllMenus();
            return View(menus);
        }

        public ActionResult Details(int id)
        {
            var menu = _menuRepository.GetMenuById(id);
            if (menu == null) return RedirectToAction("Index");

            var menuItems = _menuRepository.GetMenuItemsByMenuId(id); // Fetch related items
            ViewBag.MenuItems = menuItems; // Pass to view

            return View(menu);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateMenuDto dto)
        {
            _menuRepository.CreateMenu(dto);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var menu = _menuRepository.GetMenuById(id);
            if (menu == null) RedirectToAction("Index");
            return View(menu);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateMenuDto dto)
        {
            _menuRepository.UpdateMenu(id, dto);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var menu = _menuRepository.GetMenuById(id);
            if (menu == null) RedirectToAction("Index");
            return View(menu);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _menuRepository.DeleteMenu(id);
            return RedirectToAction("Index");
        }
    }
}
