namespace ALBERT.Models
{
    public class Customer: AggregateRoot
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; set; } = new();
    }




}
