using ALBERT.Models;
using ALBERT.ViewModels;
using Microsoft.Data.SqlClient;

public class MenuRepository
{
    private readonly string _connectionString;

    public MenuRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<MenuDto> GetAllMenus()
    {
        var menus = new List<MenuDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, Name FROM Menus";
            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    menus.Add(new MenuDto
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
        }
        return menus;
    }

    public List<MenuItem> GetMenuItemsByMenuId(int menuId)
    {
        List<MenuItem> menuItems = new();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM MenuItems WHERE MenuId = @MenuId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MenuId", menuId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menuItems.Add(new MenuItem
                        {
                            Id = (int)reader["Id"],
                            MenuId = (int)reader["MenuId"],
                            Name = reader["Name"].ToString(),
                            Price = (decimal)reader["Price"],
                            Category = reader["Category"].ToString(),
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }
        }
        return menuItems;
    }


    public MenuDto GetMenuById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, Name FROM Menus WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MenuDto
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                    }
                }
            }
        }
        return null;
    }

    public void CreateMenu(CreateMenuDto dto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Menus (Name) VALUES (@Name)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", dto.Name);
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateMenu(int id, CreateMenuDto dto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE Menus SET Name = @Name WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", dto.Name);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteMenu(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Menus WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
