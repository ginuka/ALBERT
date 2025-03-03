using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ALBERT.Models;
using Microsoft.Extensions.Configuration;

namespace ALBERT.Repositories
{
    public class TableRepository
    {
        private readonly string _connectionString;

        public TableRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ✅ Get all tables
        public List<Table> GetAllTables()
        {
            var tables = new List<Table>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Tables", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new Table
                            {
                                Id = (int)reader["Id"],
                                TableNumber = (int)reader["TableNumber"],
                                Capacity = (int)reader["Capacity"],
                                Status = (TableStatus)reader["Status"]
                            });
                        }
                    }
                }
            }

            return tables;
        }

        // ✅ Get a table by ID
        public Table GetTableById(int id)
        {
            Table table = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Tables WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            table = new Table
                            {
                                Id = (int)reader["Id"],
                                TableNumber = (int)reader["TableNumber"],
                                Capacity = (int)reader["Capacity"],
                                Status = (TableStatus)reader["Status"]
                            };
                        }
                    }
                }
            }

            return table;
        }

        // ✅ Create a new table
        public void CreateTable(Table table)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "INSERT INTO Tables (TableNumber, Capacity, Status) VALUES (@TableNumber, @Capacity, @Status)", connection))
                {
                    command.Parameters.AddWithValue("@TableNumber", table.TableNumber);
                    command.Parameters.AddWithValue("@Capacity", table.Capacity);
                    command.Parameters.AddWithValue("@Status", table.Status);

                    command.ExecuteNonQuery();
                }
            }
        }

        // ✅ Update an existing table
        public void UpdateTable(int id, Table table)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "UPDATE Tables SET TableNumber = @TableNumber, Capacity = @Capacity, Status = @Status WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@TableNumber", table.TableNumber);
                    command.Parameters.AddWithValue("@Capacity", table.Capacity);
                    command.Parameters.AddWithValue("@Status", table.Status);

                    command.ExecuteNonQuery();
                }
            }
        }

        // ✅ Delete a table
        public void DeleteTable(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Tables WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Table> GetAvailableTables()
        {
            var availableTables = new List<Table>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT * FROM Tables 
                    WHERE Status = @AvailableStatus";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AvailableStatus", TableStatus.Available);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            availableTables.Add(new Table
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                TableNumber = reader.GetInt32(reader.GetOrdinal("TableNumber")),
                                Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                Status = (TableStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return availableTables;
        }

        public string GetTableNumber(int tableId)
        {
            string tableNumber = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT TableNumber FROM Tables WHERE Id = @TableId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TableId", tableId);
                    var result = cmd.ExecuteScalar();
                    tableNumber = result?.ToString(); // Null check
                }
            }

            return tableNumber ?? "Unknown Table"; // Default value if not found
        }

        internal void UpdateTableStatus(int tableId, TableStatus occupied)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "UPDATE Tables SET Status = @Status WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", tableId);
                    command.Parameters.AddWithValue("@Status", occupied);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
