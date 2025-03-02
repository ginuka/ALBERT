using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ALBERT.Models;
using Microsoft.Extensions.Configuration;
using ALBERT.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ALBERT.Repositories
{
    public class ReservationRepository
    {
        private readonly string _connectionString;

        public ReservationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ✅ Get all reservations
        public List<Reservation> GetAllReservations()
        {
            var reservations = new List<Reservation>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
            SELECT r.*, c.Name AS CustomerName, c.Phone AS CustomerPhone, 
                   t.TableNumber
            FROM Reservations r
            JOIN Customers c ON r.CustomerId = c.Id
            JOIN Tables t ON r.TableId = t.Id";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(new Reservation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    Name = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    Phone = reader.GetString(reader.GetOrdinal("CustomerPhone"))
                                },
                                Table = new Table
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("TableId")),
                                    TableNumber = reader.GetInt32(reader.GetOrdinal("TableNumber"))
                                },
                                ReservationTime = reader.GetDateTime(reader.GetOrdinal("ReservationTime")),
                                Status = (ReservationStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return reservations;
        }


        // ✅ Get a reservation by ID
        public Reservation GetReservationById(int id)
        {
            Reservation reservation = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
            SELECT r.*, 
                   c.Name AS CustomerName, c.Phone AS CustomerPhone, c.Email AS CustomerEmail, 
                   t.TableNumber, t.Capacity, t.Status AS TableStatus
            FROM Reservations r
            JOIN Customers c ON r.CustomerId = c.Id
            JOIN Tables t ON r.TableId = t.Id
            WHERE r.Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reservation = new Reservation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    Name = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    Phone = reader.GetString(reader.GetOrdinal("CustomerPhone")),
                                    Email = reader.GetString(reader.GetOrdinal("CustomerEmail"))
                                },
                                Table = new Table
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("TableId")),
                                    TableNumber = reader.GetInt32(reader.GetOrdinal("TableNumber")),
                                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                    Status = (TableStatus)reader.GetInt32(reader.GetOrdinal("TableStatus"))
                                },
                                ReservationTime = reader.GetDateTime(reader.GetOrdinal("ReservationTime")),
                                Status = (ReservationStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }

            return reservation;
        }


        // ✅ Create a new reservation
        public void CreateReservation(CreateReservationDto reservation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "INSERT INTO Reservations (CustomerId, TableId, ReservationTime, Status) VALUES (@CustomerId, @TableId, @ReservationTime, @Status)", connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", reservation.CustomerId);
                    command.Parameters.AddWithValue("@TableId", reservation.TableId);
                    command.Parameters.AddWithValue("@ReservationTime", reservation.ReservationTime);
                    command.Parameters.AddWithValue("@Status", reservation.Status);

                    command.ExecuteNonQuery();
                }
                
                using (var tableCommand = new SqlCommand(
                    "UPDATE Tables SET Status = @Status WHERE Id = @TableId", connection))
                {
                    tableCommand.Parameters.AddWithValue("@TableId", reservation.TableId);
                    tableCommand.Parameters.AddWithValue("@Status", (int)TableStatus.Reserved);

                    tableCommand.ExecuteNonQuery();
                }
            }
        }

        // ✅ Update an existing reservation
        public void UpdateReservation(int id, CreateReservationDto reservation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "UPDATE Reservations SET TableId = @TableId, ReservationTime = @ReservationTime, Status = @Status WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@TableId", reservation.TableId);
                    command.Parameters.AddWithValue("@ReservationTime", reservation.ReservationTime);
                    command.Parameters.AddWithValue("@Status", reservation.Status);

                    command.ExecuteNonQuery();
                }
            }
        }

        // ✅ Delete a reservation
        public void DeleteReservation(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Reservations WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Reservation? GetActiveReservationByTableId(int tableId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
            SELECT TOP 1 Id, TableId, CustomerId, ReservationTime, Status 
            FROM Reservations 
            WHERE TableId = @TableId AND Status = @Status
            ORDER BY ReservationTime DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableId", tableId);
                    command.Parameters.AddWithValue("@Status", ReservationStatus.Confirmed);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Reservation
                            {
                                Id = reader.GetInt32(0),
                                TableId = reader.GetInt32(1),
                                CustomerId = reader.GetInt32(2),
                                ReservationTime = reader.GetDateTime(3),
                                Status = (ReservationStatus)reader.GetInt32(4)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
