namespace ALBERT.Models
{
    public class MenuItem: AggregateRoot
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }


}
