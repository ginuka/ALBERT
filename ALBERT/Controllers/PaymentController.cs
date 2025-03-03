using ALBERT.Data;
using ALBERT.Models;
using ALBERT.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ALBERT.Controllers
{
    public class PaymentController : Controller
    {

        private readonly PaymentRepository _paymentRepository;
        private readonly OrderRepository _orderRepository;
        private readonly TableRepository _tableRepository;
        public PaymentController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            _orderRepository = new OrderRepository(connectionString);
            _paymentRepository = new PaymentRepository(connectionString);
            _tableRepository = new TableRepository(connectionString);
        }

        public IActionResult Index()
        {
            var payments = _paymentRepository.GetAllPayments();
            return View(payments);
        }

        // Load the Payment Form (Partial View)
        public IActionResult AddPayment(int orderId)
        {
            var order = _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var model = new Payment
            {
                OrderId = orderId,
                Amount = order.TotalAmount
            };

            return PartialView("_AddPayment", model);
        }

        // Process Payment Submission
        [HttpPost]
        public IActionResult ProcessPayment(Payment payment)
        {
            ModelState.Remove("Order");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _paymentRepository.AddPayment(payment);

            var order = _orderRepository.GetOrderById(payment.OrderId);

            _tableRepository.UpdateTableStatus(order.TableId, TableStatus.NeedsCleaning);

            //return RedirectToAction("Details", "Order", new { id = payment.OrderId });

            var orderDetails = _orderRepository.GetOrderById(payment.OrderId);

            return Ok();
        }
    }
}
