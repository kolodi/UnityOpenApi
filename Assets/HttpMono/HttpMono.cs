using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HttpMono
{
    public static class UnityWebRequestExtensions
    {
        public static void AddHeaders(this UnityWebRequest webRequest, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var h in headers)
                {
                    webRequest.SetRequestHeader(h.Key, h.Value);
                }
            }
        }
    }

    public class HttpMono : MonoBehaviour
    {

        public void Get(string uri, Dictionary<string, string> headers = null, Action<HttpRequestResult> resultCallback = null)
        {
            StartCoroutine(HttpGet(uri, headers, resultCallback));
        }

        public IEnumerator HttpGet(string url, Dictionary<string, string> headers = null, Action<HttpRequestResult> resultCallback = null)
        {
            Debug.Log("GET: " + url);

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                webRequest.AddHeaders(headers);

                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                var result = new HttpRequestResult(webRequest);

                if(result.Ok == false)
                {
                    Debug.LogError(result.Error.Message);
                }

                resultCallback?.Invoke(result);
            }
        }

        public void Post(string url, string postData, Dictionary<string, string> headers = null, Action<HttpRequestResult> resultCallback = null)
        {
            StartCoroutine(HttpPost(url, postData, headers, resultCallback));
        }

        public IEnumerator HttpPost(string url, string postData, Dictionary<string, string> headers = null, Action<HttpRequestResult> resultCallback = null)
        {
            Debug.Log("POST: " + url);

            Debug.Log("BODY: " + postData);

            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                webRequest.AddHeaders(headers);
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                UploadHandler jsonUploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(postData));
                jsonUploadHandler.contentType = "application/json";
                webRequest.uploadHandler = jsonUploadHandler;

                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                var result = new HttpRequestResult(webRequest);

                if (result.Ok == false)
                {
                    Debug.LogError(result.Error.Message);
                }
                if(result.HasText)
                {
                    Debug.Log(result.Text);
                }

                resultCallback?.Invoke(result);
            }
        }

        
    }
}