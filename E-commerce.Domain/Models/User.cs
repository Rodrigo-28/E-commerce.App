namespace E_commerce.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null;
        public string Email { get; set; } = null;
        public string Password { get; set; } = null;
        public int RoleId { get; set; }
        public Role role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
