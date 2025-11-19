using System.ComponentModel.DataAnnotations;

namespace cl_be.Models.Dto.ProductDto
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } =string.Empty;

        [Required]
        [StringLength(25)]
        public string ProductNumber { get; set; } =string.Empty;

        [Range(0, double.MaxValue)]
        public decimal StandardCost { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ListPrice { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductModelId { get; set; }

        [StringLength(15)]
        public string Color { get; set; } =string.Empty;

        [StringLength(5)]
        public string Size { get; set; } =string.Empty;

        [Range(0, double.MaxValue)]
        public decimal? Weight { get; set; }

        [DataType(DataType.Date)]
        public DateTime SellStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SellEndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DiscontinuedDate { get; set; }
    }


}
//public byte[] ThumbNailPhoto { get; set; }
