namespace ALBERT.Models
{
    public class Menu: AggregateRoot
    {
        public List<MenuItem> Items { get; set; } = new();
    }


}
