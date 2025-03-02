using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ALBERT.Models;
using Microsoft.Extensions.Configuration;

namespace ALBERT.Repositories
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 🔹 Get All Employees
        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Employees", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Role = (EmployeeRole)Enum.Parse(typeof(EmployeeRole), reader["Role"].ToString()),
                            Salary = (decimal)reader["Salary"],
                            HireDate = (DateTime)reader["HireDate"]
                        });
                    }
                }
            }
            return employees;
        }

        public List<Employee> GetAllEmployeesByType(EmployeeRole type)
        {
            var employees = new List<Employee>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Employees WHERE Role = @Role", connection))
                {
                    command.Parameters.AddWithValue("@Role", type);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Role = (EmployeeRole)Enum.Parse(typeof(EmployeeRole), reader["Role"].ToString()),
                                Salary = (decimal)reader["Salary"],
                                HireDate = (DateTime)reader["HireDate"]
                            });
                        }
                    }
                }
                    
                
            }
            return employees;
        }

        // 🔹 Get Employee by ID
        public Employee GetEmployeeById(int id)
        {
            Employee employee = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Role = (EmployeeRole)Enum.Parse(typeof(EmployeeRole), reader["Role"].ToString()),
                                Salary = (decimal)reader["Salary"],
                                HireDate = (DateTime)reader["HireDate"]
                            };
                        }
                    }
                }
            }
            return employee;
        }

        // 🔹 Create Employee
        public void CreateEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Employees (Name, Phone, Role, Salary, HireDate) VALUES (@Name, @Phone, @Role, @Salary, @HireDate)", connection))
                {
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);
                    command.Parameters.AddWithValue("@Role", employee.Role);
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@HireDate", employee.HireDate);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 🔹 Update Employee
        public void UpdateEmployee(int id, Employee employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Employees SET Name = @Name, Phone = @Phone, Role = @Role, Salary = @Salary, HireDate = @HireDate WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);
                    command.Parameters.AddWithValue("@Role", employee.Role.ToString());
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@HireDate", employee.HireDate);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 🔹 Delete Employee
        public void DeleteEmployee(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
