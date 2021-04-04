namespace MavcaDetection.Response
{
    public class BaseResponse<T>
        where T : class
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
