namespace cl_be.Models.Dto.ProductDto.Admin
{
    public class AdminProductDetailDto
    {
        public int ProductId { get; set; }

        // general:
        public string? ProductParentCategoryName { get; set; }
        public string? ProductCategoryName { get; set; }
        public string? ProductModelName { get; set; }
        public string? ProductNumber { get; set; }
        public string? Name { get; set; }

        // pricing:
        public decimal ListPrice { get; set; }
        public decimal StandardCost { get; set; }

        // attributes:
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }

        // availability:
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
    }
}
