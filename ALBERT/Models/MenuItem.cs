namespace ALBERT.Models
{
    public class MenuItem: AggregateRoot
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public TimeSpan PreparationTime { get; set; }
    }


}
