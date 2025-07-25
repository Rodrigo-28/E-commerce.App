namespace E_commerce.application.Dtos.Requests
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public bool IsExpress { get; set; }
        public List<OrderItemRequest> Items { get; set; }
    }
}
