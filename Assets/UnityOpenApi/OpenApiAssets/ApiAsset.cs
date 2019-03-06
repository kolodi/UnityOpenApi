using HttpMono;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityOpenApi
{
    [CreateAssetMenu(menuName = "Unity Open API/API Assets/API", fileName = "API asset")]
    public class ApiAsset : ScriptableObject
    {
        public HttpAsset Http;
        [SerializeField]
        [HideInInspector]
        OAServer _currentServer;
        public OAServer CurrentServer { get => _currentServer; set => _currentServer = value; }
        [SerializeField]
        [HideInInspector]
        List<OAServer> _servers;
        public List<OAServer> Servers { get => _servers; }
        [SerializeField]
        OAInfo _info;
        [SerializeField]
        OAExternalDocs _externalDocs;

        public List<PathItemAsset> pathItemAssets;

        public void UpdateWithApiDocument(OpenApiDocument openApiDocument)
        {
            _servers = openApiDocument.Servers.Select(s => new OAServer(s)).ToList();

            _info = new OAInfo(openApiDocument.Info);

            _externalDocs = new OAExternalDocs(openApiDocument.ExternalDocs);
        }

        internal void ExecutePathOperation(OAOperation operation, Action<HttpRequestResult> response)
        {

            operation.ParametersValues.ForEach(p =>
            {
                if(p.parameter.Required && string.IsNullOrEmpty(p.value))
                {
                    throw new Exception("Reuired parameter value missed");
                }
            });

            var paramsWithValues = operation.ParametersValues.Where(p => !string.IsNullOrEmpty(p.value));

            var queryParams = paramsWithValues.Where(p => p.parameter.In == OAParameterLocation.Query)
                .ToDictionary(p => p.parameter.Name, p => p.value);

            var pathParams = paramsWithValues.Where(p => p.parameter.In == OAParameterLocation.Path)
                .ToDictionary(p => p.parameter.Name, p => p.value);

            string operationPath = BuildPathWithParams(operation.pathAsset.Path, pathParams);

            StringBuilder urlSb = new StringBuilder(BaseUrl);
            urlSb.Append(operationPath);
            urlSb.Append(Http.BuildQueryString(queryParams));

            string url = urlSb.ToString();

            switch (operation.OperationType)
            {
                case AOOperationType.Get:
                    Http.HttpMono.Get(url, null, response);
                    break;
                case AOOperationType.Put:
                    break;
                case AOOperationType.Post:
                    break;
                case AOOperationType.Delete:
                    break;
                case AOOperationType.Options:
                    break;
                case AOOperationType.Head:
                    break;
                case AOOperationType.Patch:
                    break;
                case AOOperationType.Trace:
                    break;
                default:
                    break;
            }
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

        #region Testing
        [ContextMenu("Test Base Url")]
        void TestBaseUrl()
        {
            Debug.Log(BaseUrl);
        }
        #endregion
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
            var _servers = apiAsset.Servers.Select(s => s.Url.Replace('/', '+')).ToArray();
            GUILayout.Label("Current server: ");
            _currentServerIndex = EditorGUILayout.Popup(_currentServerIndex, _servers);

            apiAsset.CurrentServer = apiAsset.Servers[_currentServerIndex];

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