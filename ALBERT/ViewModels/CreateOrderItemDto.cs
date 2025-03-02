using System.ComponentModel.DataAnnotations;

namespace ALBERT.ViewModels
{
    public class CreateOrderItemDto
    {
        [Required]
        public int MenuItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }

}
