namespace cl_be.Models.Dto.ProductDto.Admin
{
    public class AdminProductCreateDto
    {
        // General
        public int ProductCategoryId { get; set; }
        public int? ProductModelId { get; set; }
        public string ProductNumber { get; set; } = null!;
        public string Name { get; set; } = null!;

        // Pricing
        public decimal ListPrice { get; set; }
        public decimal StandardCost { get; set; }

        // Attributes
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }

        // Availability
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
    }
}
