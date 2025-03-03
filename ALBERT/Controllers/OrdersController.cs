using ALBERT.Models;
using ALBERT.Repositories;
using ALBERT.ViewModels;
using Humanizer;
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

        // List all orders
        public IActionResult Index()
        {
            var orders = _orderRepository.GetAllOrders();
            return View(orders);
        }

        //Show order details with items
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

        // create Order (POST)
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

            _tableRepository.UpdateTableStatus(dto.TableId, TableStatus.Occupied);

            return RedirectToAction("Index");
        }

        // Edit Order (GET)
        public IActionResult Edit(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.TableNumber = _tableRepository.GetTableNumber(order.TableId);
            ViewBag.CustomerName = _customerRepository.GetCustomerName(order.CustomerId);
            ViewBag.Waiters = _employeeRepository.GetAllEmployeesByType(EmployeeRole.Waiter);

            // Ensure menu items list is always initialized
            ViewBag.MenuItems = _menuItemRepository.GetAllMenuItems();

            return View(order);
        }



        // Edit Order (POST)
        [HttpPost]
        public IActionResult Edit(int id, UpdateOrderDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var order = _orderRepository.GetOrderById(id);
            if (order == null) return RedirectToAction("Index");

            order.WaiterId = dto.WaiterId;
            order.Status = dto.Status;

            decimal totalAmount = 0;

            // Update Order Items
            order.Items.Clear();
            foreach (var itemDto in dto.Items)
            {
                var menuItem = _menuItemRepository.GetMenuItemById(itemDto.MenuItemId);
                order.Items.Add(new OrderItem
                {
                    OrderId = id,
                    MenuItemId = itemDto.MenuItemId,
                    Quantity = itemDto.Quantity,
                    Subtotal = menuItem.Price * itemDto.Quantity
                });

                totalAmount += menuItem.Price * itemDto.Quantity;
            }
            order.TotalAmount = totalAmount;
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

        public IActionResult CreateByTable(int tableId)
        {
            ViewBag.TableId = tableId;
            ViewBag.Customers = _customerRepository.GetAllCustomers();
            ViewBag.Waiters = _employeeRepository.GetAllEmployeesByType(EmployeeRole.Waiter);
            ViewBag.MenuItems = _menuItemRepository.GetAllMenuItems();

            return PartialView("_CreateOrder");
        }

        [HttpPost]
        public IActionResult Save(CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }



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
            var orderId = _orderRepository.CreateOrder(order);

            _tableRepository.UpdateTableStatus(dto.TableId, TableStatus.Occupied);

            var orderDetails = _orderRepository.GetOrderById(orderId);

            return PartialView("_OrderDetails", orderDetails);
        }

        public IActionResult EditByTable(int tableId)
        {
            var order = _orderRepository.GetOrderByTableId(tableId);
            if (order == null) return NotFound();

            ViewBag.TableNumber = _tableRepository.GetTableNumber(order.TableId);
            ViewBag.CustomerName = _customerRepository.GetCustomerName(order.CustomerId);
            ViewBag.Waiters = _employeeRepository.GetAllEmployeesByType(EmployeeRole.Waiter);
            ViewBag.MenuItems = _menuItemRepository.GetAllMenuItems();

            return PartialView("_EditOrder", order);
        }

        [HttpPost]
        public IActionResult EditOrder(int id, Order dto)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            // Update order details here
            order.WaiterId = dto.WaiterId;
            order.Status = dto.Status;

            decimal totalAmount = 0;

            // Update Order Items
            order.Items.Clear();
            foreach (var itemDto in dto.Items)
            {
                var menuItem = _menuItemRepository.GetMenuItemById(itemDto.MenuItemId);
                order.Items.Add(new OrderItem
                {
                    OrderId = id,
                    MenuItemId = itemDto.MenuItemId,
                    Quantity = itemDto.Quantity,
                    Subtotal = menuItem.Price * itemDto.Quantity
                });

                totalAmount += menuItem.Price * itemDto.Quantity;
            }
            order.TotalAmount = totalAmount;
            _orderRepository.UpdateOrder(id, order);

            var orderDetails = _orderRepository.GetOrderById(id);

            return PartialView("_OrderDetails", orderDetails);
        }

    }
}
