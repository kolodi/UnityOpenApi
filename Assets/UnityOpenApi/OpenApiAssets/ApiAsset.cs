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
        public HttpAsset Http { get; set; }
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


        public void UpdateWithApiDocument(OpenApiDocument openApiDocument)
        {
            _servers = openApiDocument.Servers.Select(s => new OAServer
            {
                Description = s.Description,
                Url = s.Url,
                Variables = s.Variables.ToDictionary(v => v.Key, v => new OAServerVariable()
                {
                    Name = v.Key,
                    Description = v.Value.Description,
                    Default = v.Value.Default,
                    Enum = new List<string>(v.Value.Enum),
                    Current = v.Value.Enum.IndexOf(v.Value.Default)
                }).Values.ToList()
            }).ToList();


            _info = new OAInfo(openApiDocument.Info);
        }

        public string BaseUrl
        {
            get
            {
                StringBuilder url = new StringBuilder(CurrentServer.Url);
                CurrentServer.Variables.ForEach(v =>
                {
                    url.Replace("{" + v.Name + "}", v.Enum[v.Current]);
                });
                return url.ToString();
            }
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