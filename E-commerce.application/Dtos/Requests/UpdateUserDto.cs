namespace E_commerce.application.Dtos.Requests
{
    public class UpdateUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public int RoleId { get; set; }
    }
}
