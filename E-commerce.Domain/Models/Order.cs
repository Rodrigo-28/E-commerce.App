using E_commerce.Domain.Enums;

namespace E_commerce.Domain.Models
{
    // Orden que funciona también como carrito (Status Draft / Placed)

    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }    // Draft | Placed | Shipped
        public bool IsExpress { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderItem> Items { get; set; }

        public decimal GetTotalAmount()
        {
            return Items.Sum(i => i.Quantity * i.UnitPrice);
        }
    }
}
