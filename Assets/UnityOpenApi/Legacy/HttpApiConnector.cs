using HttpMono;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityOpenApi.ApiModels.OpenApi;

namespace UnityOpenApi
{

    public class HttpApiConnector : MonoBehaviour
    {

        /// <summary>
        /// Make a restful api request
        /// </summary>
        /// <param name="apiResource">Api resource asset</param>
        /// <param name="callback">Callback with the result</param>
        public void DoHttpRequest(ApiResource apiResource, string baseUri, Action<HttpRequestResult> callback)
        {
            StartCoroutine(DoingHttpRequest(apiResource, baseUri, callback));
        }

        IEnumerator DoingHttpRequest(ApiResource apiResource, string baseUri, Action<HttpRequestResult> callback)
        {
            HttpRequestHandler r = new HttpRequestHandler();

            StringBuilder stringBuilder = new StringBuilder(baseUri);
            stringBuilder.Append(apiResource.path);
            stringBuilder.Append('?');
            Dictionary<string, string> headers = new Dictionary<string, string>();

            apiResource.parametersValues.ForEach(p =>
            {
                switch (p.parameter.@in)
                {
                    case OpenApi3ParameterIn.query:
                        stringBuilder.AppendFormat("{0}={1}&", p.parameter.name, p.parametrValue);
                        break;
                    case OpenApi3ParameterIn.path:
                        stringBuilder.Replace("{" + p.parameter.name + "}", p.parametrValue);
                        break;
                    case OpenApi3ParameterIn.header:
                        headers.Add(p.parameter.name, p.parametrValue);
                        break;
                    case OpenApi3ParameterIn.cookie:
                        break;
                    default:
                        break;
                }
            });

            string fullUri = stringBuilder.ToString();

            if(apiResource.apiAuthorizer != null)
            {
                switch (apiResource.apiAuthorizer.securityScheme.@in)
                {
                    case OpenApi3ParameterIn.query:
                        break;
                    case OpenApi3ParameterIn.path:
                        break;
                    case OpenApi3ParameterIn.header:
                        ApiHeaderAuthorizer apiHeaderAuthorizer = (ApiHeaderAuthorizer)apiResource.apiAuthorizer;
                        if(string.IsNullOrEmpty(apiHeaderAuthorizer.Token))
                        {
                            throw new Exception("Missing authorization token for api resourse " + apiResource.name);
                        }
                        headers.Add(apiHeaderAuthorizer.securityScheme.name, apiHeaderAuthorizer.Token);
                        break;
                    case OpenApi3ParameterIn.cookie:
                        break;
                    default:
                        break;
                }
            }

            switch (apiResource.httpWord)
            {
                case OpenApi3HttpWords.get:
                    yield return r.Get(fullUri, headers);
                    break;
                case OpenApi3HttpWords.post:
                    break;
                case OpenApi3HttpWords.put:
                    break;
                case OpenApi3HttpWords.patch:
                    break;
                case OpenApi3HttpWords.delete:
                    break;
                case OpenApi3HttpWords.head:
                    break;
                case OpenApi3HttpWords.options:
                    break;
                case OpenApi3HttpWords.trace:
                    break;
                default:
                    break;
            }
            callback.Invoke(r.Result);
        }
    }
}