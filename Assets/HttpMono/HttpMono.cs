using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HttpMono
{
    public class HttpMono : MonoBehaviour
    {

        public void Get(string uri, Dictionary<string, string> headers = null, Action<HttpRequestResult> resultCallback = null)
        {
            StartCoroutine(HttpGet(uri, headers, resultCallback));
        }

        public IEnumerator HttpGet(string uri, Dictionary<string, string> headers = null, Action<HttpRequestResult> resultCallback = null)
        {
            Debug.Log("GET: " + uri);

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                if (headers != null)
                {
                    foreach (var h in headers)
                    {
                        webRequest.SetRequestHeader(h.Key, h.Value);
                    }
                }

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

    }
}