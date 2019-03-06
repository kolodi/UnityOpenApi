using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HttpMono
{
    [CreateAssetMenu(menuName ="Unity Open API/Http/Http Asset", fileName ="HTTP")]
    public class HttpAsset : ScriptableObject
    {
        private HttpMono httpMono;
        public HttpMono HttpMono
        {
            get
            {
                if (httpMono == null)
                {
                    httpMono = FindObjectOfType<HttpMono>();
                    if (httpMono == null)
                    {
                        httpMono = new GameObject("HTTP").AddComponent<HttpMono>();
                    }
                }
                return httpMono;
            }
        }

        public string BuildQueryString(Dictionary<string, string> parameters)
        {
            if (parameters.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder("?");

            foreach (var par in parameters)
            {

                sb.AppendFormat("{0}={1}&", par.Key, par.Value);

            }

            string result = sb.ToString();
            int lastAnd = result.LastIndexOf('&');
            if (lastAnd > 3)
            {
                result = result.Remove(lastAnd);
            }

            return result;
        }
    }
}
