using ALBERT.Models;
using Microsoft.Data.SqlClient;

namespace ALBERT.Repositories
{
    public class PaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddPayment(Payment payment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                INSERT INTO Payments (OrderId, Amount, PaymentMethod, Status, TransactionId) 
                VALUES (@OrderId, @Amount, @PaymentMethod, @Status, @TransactionId)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", payment.OrderId);
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@PaymentMethod", (int)payment.PaymentMethod);
                    command.Parameters.AddWithValue("@Status", (int)payment.Status);
                    command.Parameters.AddWithValue("@TransactionId", payment.TransactionId ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Payment> GetPaymentsByOrderId(int orderId)
        {
            var payments = new List<Payment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, OrderId, Amount, PaymentMethod, Status, TransactionId FROM Payments WHERE OrderId = @OrderId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment
                            {
                                Id = reader.GetInt32(0),
                                OrderId = reader.GetInt32(1),
                                Amount = reader.GetDecimal(2),
                                PaymentMethod = (PaymentMethod)reader.GetInt32(3),
                                Status = (PaymentStatus)reader.GetInt32(4),
                                TransactionId = reader.IsDBNull(5) ? null : reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return payments;
        }

        public void UpdatePaymentStatus(int paymentId, PaymentStatus status)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Payments SET Status = @Status WHERE Id = @PaymentId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PaymentId", paymentId);
                    command.Parameters.AddWithValue("@Status", (int)status);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Payment> GetAllPayments()
        {
            var payments = new List<Payment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, OrderId, Amount, PaymentMethod, Status, TransactionId FROM Payments";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment
                            {
                                Id = reader.GetInt32(0),
                                OrderId = reader.GetInt32(1),
                                Amount = reader.GetDecimal(2),
                                PaymentMethod = (PaymentMethod)reader.GetInt32(3),
                                Status = (PaymentStatus)reader.GetInt32(4),
                                TransactionId = reader.IsDBNull(5) ? null : reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return payments;
        }

    }
}
