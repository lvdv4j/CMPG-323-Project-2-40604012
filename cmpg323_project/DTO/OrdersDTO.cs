namespace cmpg323_project.DTO
{
    public class OrdersDTO
    {
        public short OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public short CustomerId { get; set; }
        public string? DeliveryAddress { get; set; }

        public List<OrderDetailsDTO> OrderDetails { get; set; }
    }
}
