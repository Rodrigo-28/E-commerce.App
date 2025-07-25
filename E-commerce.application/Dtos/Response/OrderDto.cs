using E_commerce.Domain.Enums;

namespace E_commerce.application.Dtos.Response
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsExpress { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
