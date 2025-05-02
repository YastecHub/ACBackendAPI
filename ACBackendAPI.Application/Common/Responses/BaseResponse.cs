namespace ACBackendAPI.Application.Common.Responses
{
    namespace ACBackendAPI.Application.Common.Responses
    {
        public class BaseResponse<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public T? Data { get; set; }
            public int StatusCode { get; set; }
            public List<string>? Errors { get; set; }
        }
    }
}
