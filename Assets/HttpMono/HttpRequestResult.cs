using System.Collections.Generic;
using UnityEngine.Networking;

namespace HttpMono
{
    public class HttpRequestResult
    {
        public bool Ok { get; }
        public HttpResultError Error { get; }
        public KeyValuePair<long, string> HttpCodeResponse { get; }
        public string Text { get; }
        public HttpRequestResult(UnityWebRequest r)
        {
            Ok = true;

            if (r.isNetworkError || string.IsNullOrEmpty(r.error) == false) Ok = false;
            HttpCodeResponse = StandardHttpResponseCodes.GetCode(r.responseCode);
            if (HttpCodeResponse.Key < 200 || HttpCodeResponse.Key > 299) Ok = false;

            if (!Ok)
            {
                Error = new HttpResultError(r);
                return;
            }

            if (r.downloadHandler != null)
            {
                Text = r.downloadHandler.text;
            }
        }
    }

}