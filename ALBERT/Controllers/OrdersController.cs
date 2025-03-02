using ALBERT.Models;
using ALBERT.Repositories;
using ALBERT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ALBERT.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly TableRepository _tableRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly MenuItemRepository _menuItemRepository;
        private readonly EmployeeRepository _employeeRepository;

        public OrdersController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _orderRepository = new OrderRepository(connectionString);
            _tableRepository = new TableRepository(connectionString);
            _customerRepository = new CustomerRepository(connectionString);
            _menuItemRepository = new MenuItemRepository(connectionString);
            _employeeRepository = new EmployeeRepository(connectionString);
        }

        // 🔹 List all orders
        public IActionResult Index()
        {
            var orders = _orderRepository.GetAllOrders();
            return View(orders);
        }

        // 🔹 Show order details with items
        public IActionResult Details(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null) return RedirectToAction("Index");
            return View(order);
        }

        // 🔹 Create Order (GET)
        public IActionResult Create()
        {
            ViewBag.Tables = _tableRepository.GetAvailableTables();
            ViewBag.Customers = _customerRepository.GetAllCustomers();
            ViewBag.Waiter = _employeeRepository.GetAllEmployeesByType(EmployeeRole.Waiter);
            ViewBag.MenuItems = _menuItemRepository.GetAllMenuItems();
            return View();
        }

        // 🔹 Create Order (POST)
        [HttpPost]
        public IActionResult Create(CreateOrderDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var order = new Order
            {
                TableId = dto.TableId,
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.Now,
                WaiterId = dto.WaiterId,
                Status = OrderStatus.Pending,
                Items = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var itemDto in dto.Items)
            {
                var menuItem = _menuItemRepository.GetMenuItemById(itemDto.MenuItemId);
                if (menuItem != null)
                {
                    var orderItem = new OrderItem
                    {
                        MenuItemId = itemDto.MenuItemId,
                        Quantity = itemDto.Quantity,
                        Subtotal = menuItem.Price * itemDto.Quantity
                    };
                    totalAmount += orderItem.Subtotal;
                    order.Items.Add(orderItem);
                }
            }

            order.TotalAmount = totalAmount;
            _orderRepository.CreateOrder(order);

            return RedirectToAction("Index");
        }

        // 🔹 Edit Order (GET)
        public IActionResult Edit(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null) return RedirectToAction("Index");
            ViewBag.Tables = _tableRepository.GetAvailableTables();
            ViewBag.Customers = _customerRepository.GetAllCustomers();
            ViewBag.Waiter = _employeeRepository.GetAllEmployeesByType(EmployeeRole.Waiter);
            ViewBag.MenuItems = _menuItemRepository.GetAllMenuItems();
            return View(order);
        }

        // 🔹 Edit Order (POST)
        [HttpPost]
        public IActionResult Edit(int id, UpdateOrderDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var order = _orderRepository.GetOrderById(id);
            if (order == null) return RedirectToAction("Index");

            order.WaiterId = dto.WaiterId;
            order.Status = dto.Status;

            _orderRepository.UpdateOrder(id, order);
            return RedirectToAction("Index");
        }

        // 🔹 Delete Order (GET)
        public IActionResult Delete(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null) return RedirectToAction("Index");

            return View(order);
        }

        // 🔹 Delete Order (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _orderRepository.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}
