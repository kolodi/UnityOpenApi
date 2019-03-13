using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

namespace HttpMono
{

    public class HttpRequestHandler
    {
        private HttpRequestResult result;
        public HttpRequestResult Result
        {
            get
            {
                return result;
            }
        }

        public IEnumerator Get(string uri, Dictionary<string, string> headers = null)
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
                result = new HttpRequestResult(webRequest);


                if (webRequest.isNetworkError)
                {
                    Debug.Log("Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                }


            }
        }
    }

}