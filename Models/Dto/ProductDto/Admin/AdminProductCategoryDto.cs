namespace cl_be.Models.Dto.ProductDto.Admin
{
    public class AdminProductCategoryDto
    {
        public int ProductCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentProductCategoryId { get; set; }
    }
}
