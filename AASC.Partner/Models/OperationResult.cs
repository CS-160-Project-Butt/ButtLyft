namespace AASC.Partner.API.Models
{
    public enum OperationResult
    {
        Success,
        Failed,
        NotFound
    }

    public class OperationResult<T>
    {
        public T Data { get; set; }

        public OperationResult Status { get; set; }

        public string Message { get; set; }

    }
}