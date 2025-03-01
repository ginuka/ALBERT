namespace ALBERT.Models
{
    public class Table: AggregateRoot
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
    }

    public enum TableStatus
    {
        Available,
        Reserved,
        Occupied,
        NeedsCleaning
    }

}
