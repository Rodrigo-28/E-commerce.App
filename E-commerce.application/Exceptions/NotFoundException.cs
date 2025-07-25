namespace E_commerce.application.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }
    }
}
