using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ALBERT.Models;

public class CustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public string GetCustomerName(int customerId)
    {
        string customerName = null;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            string query = "SELECT Name FROM Customers WHERE Id = @CustomerId";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                var result = cmd.ExecuteScalar();
                customerName = result?.ToString(); // Null check
            }
        }

        return customerName ?? "Walk-in Customer"; // Default if not found
    }

    public List<Customer> GetAllCustomers()
    {
        var customers = new List<Customer>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Customers";

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }
        }
        return customers;
    }

    public Customer GetCustomerById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Customers WHERE Id = @Id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Customer
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                    }
                }
            }
        }
        return null;
    }

    public void CreateCustomer(Customer customer)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Customers (Name, Phone, Email) VALUES (@Name, @Phone, @Email)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Phone", customer.Phone);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateCustomer(int id, Customer customer)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE Customers SET Name = @Name, Phone = @Phone, Email = @Email WHERE Id = @Id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Phone", customer.Phone);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteCustomer(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Customers WHERE Id = @Id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Customer> SearchCustomers(string query)
    {
        var customers = new List<Customer>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sql = @"
                    SELECT * FROM Customers 
                    WHERE Name LIKE @Query OR Phone LIKE @Query OR Email LIKE @Query";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Query", $"%{query}%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        });
                    }
                }
            }
        }
        return customers;
    }
}
