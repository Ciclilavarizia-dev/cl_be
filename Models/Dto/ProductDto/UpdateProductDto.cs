using System.ComponentModel.DataAnnotations;

namespace cl_be.Models.Dto.ProductDto
{
    public class UpdateProductDto
    {
        [Required]
        public int ProductId { get; set; }

        // General
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required, StringLength(25)]
        public string ProductNumber { get; set; } = null!;

        [Required]
        public int ProductCategoryId { get; set; }

        [Required]
        public int ProductModelId { get; set; }

        // Pricing
        [Range(0, 999999)]
        public decimal ListPrice { get; set; }

        [Range(0, 999999)]
        public decimal StandardCost { get; set; }

        // Attributes
        [StringLength(15)]
        public string? Color { get; set; }

        [StringLength(5)]
        public string? Size { get; set; }

        [Range (0, 999999)]
        public decimal? Weight { get; set; }

        // Availability
        [Required]
        public DateTime SellStartDate { get; set; }

        public DateTime? SellEndDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }
    }
}
