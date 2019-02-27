using UnityEngine.Networking;

namespace HttpMono
{
    public class HttpResultError
    {
        public HttpResultErrorType Type { get; }
        public string Message { get; }
        public HttpResultError(UnityWebRequest r)
        {
            if (r.isNetworkError)
            {
                Type = HttpResultErrorType.NetworkError;
            }
            else if (r.isHttpError)
            {
                Type = HttpResultErrorType.HttpError;
            }
            else
            {
                Type = HttpResultErrorType.Unknown;
            }
            Message = r.error;
        }
    }

}