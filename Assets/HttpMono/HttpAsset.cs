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
    }
}
