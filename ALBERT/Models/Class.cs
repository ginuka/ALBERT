using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ALBERT.Models
{
    public class AggregateRoot
    {
        [Key]
        public int Id { get; set; }
    }

    public class Order : AggregateRoot
    {
        public int TableId { get; set; }
        public Table Table { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Preparing,
        Served,
        Completed,
        Canceled
    }


    public class OrderItem: AggregateRoot
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }


    public class Customer: AggregateRoot
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; set; } = new();
    }

    public abstract class Employee: AggregateRoot
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public EmployeeRole Role { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
    }

    public enum EmployeeRole
    {
        Manager,
        Chef,
        Waiter,
        Cashier,
        Cleaner
    }

    public class Waiter : Employee
    {
        public List<Table> AssignedTables { get; set; } = new();
        public List<Order> CurrentOrders { get; set; } = new();
    }

    public class Chef : Employee
    {
        public string Specialization { get; set; }
        public List<Order> AssignedOrders { get; set; } = new();
    }

    public class Payment: AggregateRoot
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
    }

    public enum PaymentMethod
    {
        Cash,
        Card,
        Online,
        UPI,
        Wallet
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }

    public class Reservation: AggregateRoot
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
        public DateTime ReservationTime { get; set; }
        public ReservationStatus Status { get; set; }
    }

    public enum ReservationStatus
    {
        Confirmed,
        Canceled,
        NoShow,
        Completed
    }




}
