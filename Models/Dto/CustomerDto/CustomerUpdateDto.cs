namespace cl_be.Models.Dto.CustomerDto
{
    public class CustomerUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? Phone { get; set; }
    }
}
