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
        OAServer _currentServer;
        public OAServer CurrentServer { get => _currentServer; set => _currentServer = value; }
        [SerializeField]
        [HideInInspector]
        public List<OAServer> servers;
        [SerializeField]
        public OAInfo info;
        [SerializeField]
        public OAExternalDocs externalDocs;
        public List<PathItemAsset> pathItemAssets;

        internal IPromise<ResponseHelper> ExecuteOperation(OAOperation operation)
        {
            var errorPromise = new Promise<ResponseHelper>();

            foreach(var p in operation.ParametersValues)
            {
                if (p.parameter.Required && string.IsNullOrEmpty(p.value))
                {
                    errorPromise.Reject(new Exception("Reuired parameter value missed"));
                    return errorPromise;
                }
            }

            var paramsWithValues = operation.ParametersValues.Where(p => !string.IsNullOrEmpty(p.value));

            var queryParams = paramsWithValues.Where(p => p.parameter.In == OAParameterLocation.Query)
                .ToDictionary(p => p.parameter.Name, p => p.value);

            var pathParams = paramsWithValues.Where(p => p.parameter.In == OAParameterLocation.Path)
                .ToDictionary(p => p.parameter.Name, p => p.value);

            var headerParams = paramsWithValues.Where(p => p.parameter.In == OAParameterLocation.Header)
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
            return RestClient.Request(requestOptions);
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