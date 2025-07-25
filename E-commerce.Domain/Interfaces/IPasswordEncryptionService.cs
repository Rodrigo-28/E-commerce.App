namespace E_commerce.Domain.Interfaces
{
    public interface IPasswordEncryptionService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
