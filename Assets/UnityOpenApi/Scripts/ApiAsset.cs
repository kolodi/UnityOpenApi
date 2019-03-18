using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;
using Proyecto26;
using RSG;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityOpenApi
{
    [CreateAssetMenu(menuName = "Unity Open API/API Assets/API", fileName = "API asset")]
    public class ApiAsset : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        Server _currentServer;
        public Server CurrentServer { get => _currentServer; set => _currentServer = value; }
        [SerializeField]
        [HideInInspector]
        public List<Server> servers;
        [SerializeField]
        public Info info;
        [SerializeField]
        public ExternalDocs externalDocs;
        public List<PathItemAsset> pathItemAssets;

        internal RequestHelper PrepareOperation(Operation operation)
        {
            foreach (var p in operation.ParametersValues)
            {
                if (p.parameter.Required && string.IsNullOrEmpty(p.value))
                {
                    throw new Exception("Reuired parameter value missed");
                }
            }

            var paramsWithValues = operation.ParametersValues.Where(p => !string.IsNullOrEmpty(p.value));

            var queryParams = paramsWithValues.Where(p => p.parameter.In == ParameterLocation.Query)
                .ToDictionary(p => p.parameter.Name, p => p.value);

            var pathParams = paramsWithValues.Where(p => p.parameter.In == ParameterLocation.Path)
                .ToDictionary(p => p.parameter.Name, p => p.value);

            var headerParams = paramsWithValues.Where(p => p.parameter.In == ParameterLocation.Header)
                .ToDictionary(p => p.parameter.Name, p => p.value);

            string operationPath = BuildPathWithParams(operation.pathAsset.Path, pathParams);

            StringBuilder urlSb = new StringBuilder(BaseUrl);
            urlSb.Append(operationPath);
            urlSb.Append(BuildQueryString(queryParams));

            string url = urlSb.ToString();

            RequestHelper requestOptions = new RequestHelper()
            {
                Method = operation.OperationType.ToString(),
                Headers = headerParams,
                Uri = url,
                EnableDebug = true
            };

            return requestOptions;
        }

        internal IPromise<ResponseHelper> ExecuteOperation(RequestHelper requestOptions)
        {
            return RestClient.Request(requestOptions);
        }

        internal IPromise<ResponseHelper> ExecuteOperation(Operation operation)
        {            
            return RestClient.Request(operation.Request);
        }
        
        public string BaseUrl
        {
            get
            {
                var d = CurrentServer.Variables.ToDictionary(v => v.Name, v => v.Enum[v.Current]);
                return BuildPathWithParams(CurrentServer.Url, d);
            }
        }

        public string BuildPathWithParams(string path, Dictionary<string, string> pathParams)
        {
            StringBuilder sb = new StringBuilder(path);
            foreach (var p in pathParams)
            {
                sb.Replace("{" + p.Key + "}", p.Value);
            }
            return sb.ToString();
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

#if UNITY_EDITOR
    [CustomEditor(typeof(ApiAsset))]
    public class ApiAssetEditor : Editor
    {
        int _currentServerIndex = 0;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var apiAsset = target as ApiAsset;
            var _servers = apiAsset.servers.Select(s => s.Url.Replace('/', '+')).ToArray();
            GUILayout.Label("Current server: ");
            _currentServerIndex = EditorGUILayout.Popup(_currentServerIndex, _servers);

            apiAsset.CurrentServer = apiAsset.servers[_currentServerIndex];

            if (apiAsset.CurrentServer.Variables != null)
            {
                GUILayout.Label("Variables: ");
                apiAsset.CurrentServer.Variables.ForEach(v =>
                {
                    v.Current = EditorGUILayout.Popup(v.Name, v.Current, v.Enum.ToArray());
                });
            }

            // Save the changes back to the object
            EditorUtility.SetDirty(target);
        }
    }
#endif
}