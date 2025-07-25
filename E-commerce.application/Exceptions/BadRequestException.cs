namespace E_commerce.application.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException()
        {

        }

        public BadRequestException(string message) : base(message)
        {

        }
    }
}
