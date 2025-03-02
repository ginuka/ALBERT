using System.ComponentModel.DataAnnotations;

namespace ALBERT.ViewModels
{
    public class CreateOrderDto
    {
        [Required]
        public int TableId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int WaiterId { get; set; }

        [Required]
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }

}
