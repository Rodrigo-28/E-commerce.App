using E_commerce.Domain.Enums;

namespace E_commerce.application.Dtos.Requests
{
    public class ChangeStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}
