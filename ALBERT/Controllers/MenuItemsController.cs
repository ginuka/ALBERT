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
    public class MenuItemsController : Controller
    {
        private readonly MenuItemRepository _menuItemRepository;
        private readonly MenuRepository _menuRepository;
        private readonly IConfiguration _configuration;

        public MenuItemsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _menuItemRepository = new MenuItemRepository(_configuration.GetConnectionString("DefaultConnection"));
            _menuRepository = new MenuRepository(_configuration.GetConnectionString("DefaultConnection"));
        }

        private List<string> GetCategories()
        {
            return new List<string> { "Appetizer", "Main Course", "Dessert", "Beverage", "Sides", "Specials" };
        }


        public ActionResult Index()
        {
            var menuItems = _menuItemRepository.GetAllMenuItems();
            return View(menuItems);
        }

        public ActionResult Details(int id)
        {
            var menuItem = _menuItemRepository.GetMenuItemById(id);
            if (menuItem == null) RedirectToAction("Index");
            return View(menuItem);
        }

        public ActionResult Create()
        {
            ViewBag.Menus = _menuRepository.GetAllMenus(); // Passing menu list
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateMenuItemDto dto)
        {
            _menuItemRepository.CreateMenuItem(dto);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var menuItem = _menuItemRepository.GetMenuItemById(id);
            ViewBag.Menus = _menuRepository.GetAllMenus(); // Passing menu list
            ViewBag.Categories = GetCategories();
            if (menuItem == null) RedirectToAction("Index");
            return View(menuItem);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateMenuItemDto dto)
        {
            _menuItemRepository.UpdateMenuItem(id, dto);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var menuItem = _menuItemRepository.GetMenuItemById(id);
            if (menuItem == null) RedirectToAction("Index");
            return View(menuItem);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _menuItemRepository.DeleteMenuItem(id);
            return RedirectToAction("Index");
        }
    }
}
