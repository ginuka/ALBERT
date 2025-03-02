namespace ALBERT.Models
{
    public class Menu: AggregateRoot
    {
        public string Name { get; set; }
        public List<MenuItem> Items { get; set; } = new();
    }


}
