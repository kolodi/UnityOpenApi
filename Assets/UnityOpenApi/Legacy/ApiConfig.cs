using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HttpMono;
using UnityOpenApi.ApiModels.OpenApi;

namespace UnityOpenApi
{

    [CreateAssetMenu(menuName ="Unity Open API/API Config", fileName ="API Config")]
    public class ApiConfig : ScriptableObject
    {
        public OpenApi3 rawData;
        //public string baseUri;
        public List<ApiAuthorizer> authorizers;

        private HttpApiConnector connector;
        private HttpApiConnector Connector
        {
            get
            {
                if(connector == null)
                {
                    connector = FindObjectOfType<HttpApiConnector>();
                    if(connector == null)
                    {
                        connector = new GameObject("Http API Connector").AddComponent<HttpApiConnector>();
                    }
                }
                return connector;
            }
        }

        [System.Serializable]
        public struct ServerVariableValue
        {
            public string varName;
            public string varValue;
        }

        public List<ServerVariableValue> serverVariableValues = new List<ServerVariableValue>();

        public string Uri
        {
            get
            {
                OpenApi3Server server = rawData.servers[0];
                StringBuilder stringBuilder = new StringBuilder(server.url);
                serverVariableValues.ForEach(svv =>
                {
                    OpenApi3ServerVariable serverVar = server.variables[svv.varName];
                    string val = string.IsNullOrEmpty(svv.varValue) ? serverVar.@default : svv.varValue;
                    val = val.Trim(new char[] { '/' });
                    stringBuilder.Replace("{" + svv.varName + "}", val);
                });
                return stringBuilder.ToString();
            }
        }

        public ApiAuthorizer GetAuthorizer(string authName)
        {
            return authorizers.FirstOrDefault(a => a.authName == authName);
        }

        public void CallApi(ApiResource apiResource, Action<HttpRequestResult> c)
        {
            Connector.DoHttpRequest(apiResource, Uri, c);
        }
    }
}