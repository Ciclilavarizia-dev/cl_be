using cl_be.Models.Dto.AddressDto;

namespace cl_be.Models.Dto.OrderDto

{
    public class OrderDetailDto
    {
        public int SalesOrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public byte Status { get; set; }

        public int CustomerId { get; set; }

        public int? ShipToAddressId { get; set; }

        public int? BillToAddressId { get; set; }

        public decimal TotalDue { get; set; }


        public ShipToAddressDto? BillToAddress { get; set; }
        public ShipToAddressDto? ShipToAddress { get; set; }

        // Riga dettagli
        //public List<OrderLineDto> Lines { get; set; } = new();



        //public class OrderLineDto
        //{
        //    public int ProductId { get; set; }
        //    public string ProductName { get; set; } = "";
        //    public int Quantity { get; set; }
        //    public decimal UnitPrice { get; set; }
        //    public decimal LineTotal { get; set; }
        //}
    }
}
