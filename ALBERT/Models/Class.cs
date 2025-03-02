using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace ALBERT.Models
{

    public enum OrderStatus
    {
        Pending,
        Preparing,
        Served,
        Completed,
        Canceled
    }

    public enum EmployeeRole
    {
        Manager,
        Chef,
        Waiter,
        Cashier,
        Cleaner
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

    public enum ReservationStatus
    {
        Confirmed,
        Canceled,
        NoShow,
        Completed
    }




}
