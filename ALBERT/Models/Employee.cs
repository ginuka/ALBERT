namespace ALBERT.Models
{
    public class Employee: AggregateRoot
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public EmployeeRole Role { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
    }




}
