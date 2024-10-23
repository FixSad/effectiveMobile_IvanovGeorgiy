namespace DeliveryService.Domain.Response
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string? Description { get; set; }

        public bool IsSuccess { get; set; }

        public T? Data { get; set; }
    }

    public interface IBaseResponse<T>
    {
        public string Description { get; }
        public bool IsSuccess { get; }
        public T Data { get; }
    }
}
