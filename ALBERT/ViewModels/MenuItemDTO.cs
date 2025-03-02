namespace ALBERT.ViewModels
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int MenuId { get; set; }
    }

    public class CreateMenuItemDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int MenuId { get; set; }



    }
}
