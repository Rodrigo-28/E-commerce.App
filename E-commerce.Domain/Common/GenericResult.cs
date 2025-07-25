namespace E_commerce.Domain.Common
{
    public class GenericResult<T>
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public T Data { get; set; }

        public static GenericResult<T> SuccessResult()
        {
            return new GenericResult<T> { Success = true };
        }
        public static GenericResult<T> SuccessResult(T data)
        {
            return new GenericResult<T> { Success = true, Data = data };
        }
        public static GenericResult<T> FailureResult(List<string> messages)
        {
            return new GenericResult<T> { Success = false, Messages = messages };
        }
    }
}
