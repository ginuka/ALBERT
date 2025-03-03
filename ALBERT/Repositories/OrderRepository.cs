using ALBERT.Models;
using Microsoft.Data.SqlClient;

namespace ALBERT.Repositories
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 🔹 Get All Orders
        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT o.Id, o.TableId, o.CustomerId, o.TotalAmount, o.Status, o.OrderDate, o.WaiterId, " +
                               "t.TableNumber, c.Name AS CustomerName, e.Name AS WaiterName " +
                               "FROM Orders o " +
                               "JOIN Tables t ON o.TableId = t.Id " +
                               "JOIN Customers c ON o.CustomerId = c.Id " +
                               "JOIN Employees e ON o.WaiterId = e.Id";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                Id = reader.GetInt32(0),
                                TableId = reader.GetInt32(1),
                                CustomerId = reader.GetInt32(2),
                                TotalAmount = reader.GetDecimal(3),
                                Status = (OrderStatus)reader.GetInt32(4),
                                OrderDate = reader.GetDateTime(5),
                                WaiterId = reader.GetInt32(6),
                                Table = new Table { Id = reader.GetInt32(1), TableNumber = reader.GetInt32(7) },
                                Customer = new Customer { Id = reader.GetInt32(2), Name = reader.GetString(8) },
                                Waiter = new Employee { Id = reader.GetInt32(6), Name = reader.GetString(9) }
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }



        // 🔹 Get Order By Id (with items)
        public Order GetOrderById(int orderId)
        {
            Order order = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT o.Id, o.TableId, o.CustomerId, o.TotalAmount, o.Status, o.OrderDate, o.WaiterId,
                   t.TableNumber, c.Name AS CustomerName, e.Name AS WaiterName
            FROM Orders o
            JOIN Tables t ON o.TableId = t.Id
            JOIN Customers c ON o.CustomerId = c.Id
            JOIN Employees e ON o.WaiterId = e.Id
            WHERE o.Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", orderId);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new Order
                            {
                                Id = reader.GetInt32(0),
                                TableId = reader.GetInt32(1),
                                CustomerId = reader.GetInt32(2),
                                TotalAmount = reader.GetDecimal(3),
                                Status = (OrderStatus)reader.GetInt32(4),
                                OrderDate = reader.GetDateTime(5),
                                WaiterId = reader.GetInt32(6),
                                Table = new Table { Id = reader.GetInt32(1), TableNumber = reader.GetInt32(7) },
                                Customer = new Customer { Id = reader.GetInt32(2), Name = reader.GetString(8) },
                                Waiter = new Employee { Id = reader.GetInt32(6), Name = reader.GetString(9) }
                            };
                        }
                    }
                }
            }

            // Fetch Order Items
            if (order != null)
            {
                order.Items = GetOrderItems(orderId);
            }

            return order;
        }


        // 🔹 Get Order Items
        private List<OrderItem> GetOrderItems(int orderId)
        {
            var items = new List<OrderItem>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT oi.Id, oi.MenuItemId, oi.Quantity, oi.Subtotal, m.Name, m.Price " +
                               "FROM OrderItems oi " +
                               "JOIN MenuItems m ON oi.MenuItemId = m.Id " +
                               "WHERE oi.OrderId = @OrderId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new OrderItem
                            {
                                Id = reader.GetInt32(0),
                                MenuItemId = reader.GetInt32(1),
                                Quantity = reader.GetInt32(2),
                                Subtotal = reader.GetDecimal(3),
                                MenuItem = new MenuItem
                                {
                                    Id = reader.GetInt32(1),
                                    Name = reader.GetString(4),
                                    Price = reader.GetDecimal(5)
                                }
                            };
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        // 🔹 Create Order
        public int CreateOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Orders (TableId, CustomerId, TotalAmount, Status, OrderDate, WaiterId) " +
                               "VALUES (@TableId, @CustomerId, @TotalAmount, @Status, @OrderDate, @WaiterId); " +
                               "SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableId", order.TableId);
                    command.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                    command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    command.Parameters.AddWithValue("@Status", (int)order.Status);
                    command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@WaiterId", order.WaiterId);

                    connection.Open();
                    int orderId = Convert.ToInt32(command.ExecuteScalar());

                    // Insert Order Items
                    foreach (var item in order.Items)
                    {
                        InsertOrderItem(orderId, item);
                    }

                    return orderId;
                }
            }
        }

        // 🔹 Insert Order Item
        private void InsertOrderItem(int orderId, OrderItem item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO OrderItems (OrderId, MenuItemId, Quantity, Subtotal) " +
                               "VALUES (@OrderId, @MenuItemId, @Quantity, @Subtotal)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@MenuItemId", item.MenuItemId);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
                    command.Parameters.AddWithValue("@Subtotal", item.Subtotal);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteOrderItem(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string deleteOrderItems = "DELETE FROM OrderItems WHERE OrderId = @OrderId";

                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(deleteOrderItems, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        // 🔹 Update Order
        public void UpdateOrder(int orderId, Order order)
        {
            DeleteOrderItem(orderId);
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Orders SET Status = @Status,TotalAmount =@TotalAmount, WaiterId = @WaiterId WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", orderId);
                    command.Parameters.AddWithValue("@Status", (int)order.Status);
                    command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    command.Parameters.AddWithValue("@WaiterId", order.WaiterId);

                    connection.Open();
                    command.ExecuteNonQuery();

                    
                }
            }

            foreach (var item in order.Items)
            {

                InsertOrderItem(orderId, item);
            }
        }

        // 🔹 Delete Order
        public void DeleteOrder(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string deleteOrderItems = "DELETE FROM OrderItems WHERE OrderId = @OrderId";
                string deleteOrder = "DELETE FROM Orders WHERE Id = @OrderId";

                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(deleteOrderItems, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SqlCommand(deleteOrder, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        // 🔹 Get Order By TableId (Latest Active Order)
        public Order GetOrderByTableId(int tableId)
        {
            Order order = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT TOP 1 o.Id, o.TableId, o.CustomerId, o.TotalAmount, o.Status, o.OrderDate, o.WaiterId,
                      t.TableNumber, c.Name AS CustomerName, e.Name AS WaiterName
        FROM Orders o
        JOIN Tables t ON o.TableId = t.Id
        JOIN Customers c ON o.CustomerId = c.Id
        JOIN Employees e ON o.WaiterId = e.Id
        WHERE o.TableId = @TableId AND o.Status != @CompletedStatus
        ORDER BY o.OrderDate DESC";  // Get the most recent active order

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableId", tableId);
                    command.Parameters.AddWithValue("@CompletedStatus", (int)OrderStatus.Completed);  // Assuming 'Completed' is an enum

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new Order
                            {
                                Id = reader.GetInt32(0),
                                TableId = reader.GetInt32(1),
                                CustomerId = reader.GetInt32(2),
                                TotalAmount = reader.GetDecimal(3),
                                Status = (OrderStatus)reader.GetInt32(4),
                                OrderDate = reader.GetDateTime(5),
                                WaiterId = reader.GetInt32(6),
                                Table = new Table { Id = reader.GetInt32(1), TableNumber = reader.GetInt32(7) },
                                Customer = new Customer { Id = reader.GetInt32(2), Name = reader.GetString(8) },
                                Waiter = new Employee { Id = reader.GetInt32(6), Name = reader.GetString(9) }
                            };
                        }
                    }
                }
            }

            // Fetch Order Items if order exists
            if (order != null)
            {
                order.Items = GetOrderItems(order.Id);
            }

            return order;
        }

    }
}
