using ALBERT.Models;
using System.ComponentModel.DataAnnotations;

namespace ALBERT.ViewModels
{
    public class UpdateOrderDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int TableId { get; set; }

        [Required]
        public int WaiterId { get; set; }

        [Required]
        public List<UpdateOrderItemDto> Items { get; set; } = new();

        [Required]
        public OrderStatus Status { get; set; }
    }

}
