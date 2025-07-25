namespace E_commerce.application.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException()
        {

        }

        public UnauthorizedException(string message) : base(message)
        {

        }
    }
}
