using ALBERT.ViewModels;
using Microsoft.Data.SqlClient;


public class MenuItemRepository
{
    private readonly string _connectionString;

    public MenuItemRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<MenuItemDto> GetAllMenuItems()
    {
        var menuItems = new List<MenuItemDto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, Name, Price, Category, Description, MenuId FROM MenuItems";
            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    menuItems.Add(new MenuItemDto
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Category = reader.GetString(3),
                        Description = reader.GetString(4),
                        MenuId = reader.GetInt32(5)
                    });
                }
            }
        }
        return menuItems;
    }

    public MenuItemDto GetMenuItemById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, Name, Price, Category, Description, MenuId FROM MenuItems WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MenuItemDto
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Category = reader.GetString(3),
                            Description = reader.GetString(4),
                            MenuId = reader.GetInt32(5)
                        };
                    }
                }
            }
        }
        return null;
    }

    public void CreateMenuItem(CreateMenuItemDto dto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO MenuItems (Name, Price, Category, Description, MenuId) VALUES (@Name, @Price, @Category, @Description, @MenuId)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", dto.Name);
                command.Parameters.AddWithValue("@Price", dto.Price);
                command.Parameters.AddWithValue("@Category", dto.Category);
                command.Parameters.AddWithValue("@Description", dto.Description);
                command.Parameters.AddWithValue("@MenuId", dto.MenuId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateMenuItem(int id, CreateMenuItemDto dto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE MenuItems SET Name = @Name, Price = @Price, Category = @Category, Description = @Description, MenuId = @MenuId WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", dto.Name);
                command.Parameters.AddWithValue("@Price", dto.Price);
                command.Parameters.AddWithValue("@Category", dto.Category);
                command.Parameters.AddWithValue("@Description", dto.Description);
                command.Parameters.AddWithValue("@MenuId", dto.MenuId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteMenuItem(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM MenuItems WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
