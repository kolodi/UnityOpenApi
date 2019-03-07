using HttpMono;
using Microsoft.OpenApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityOpenApi
{
    [CreateAssetMenu(menuName = "Unity Open API/API Assets/Path Item", fileName = "Path asset")]
    public class PathItemAsset : ScriptableObject
    {
        [HideInInspector]
        public ApiAsset ApiAsset;

        public string Path;
        public string Summary;
        public string Description;
        public List<OAOperation> Operations;
        public List<OAServer> Servers;
        public List<OAParameter> Parameters;

        public void UpdateWithPathData(string path, OpenApiPathItem openApiPathItem)
        {
            Path = path;

            Summary = openApiPathItem.Summary;
            Description = openApiPathItem.Description;

            Parameters = openApiPathItem.Parameters.Select(p => new OAParameter(p)).ToList();

            Operations = openApiPathItem.Operations.Select(o => new OAOperation(o.Key, o.Value, this)).ToList();
            Servers = openApiPathItem.Servers.Select(s => new OAServer(s)).ToList();

        }

        public OAOperation GetOperation(string operationId)
        {
            return Operations.First(o => o.OperationId == operationId);
        }

        public OAOperation GetOperation(AOOperationType operationType)
        {
            return Operations.First(o => o.OperationType == operationType);
        }

        public void ExecuteOperation(OAOperation operation, Action<HttpRequestResult> response)
        {
            ApiAsset.ExecutePathOperation(operation, response);
        }

        public void ExecuteOperation<T>(OAOperation operation, Action<T> callbackWithData)
        {
            ApiAsset.ExecutePathOperation(operation, response =>
            {
                if (response.Ok)
                {
                    T data = JsonConvert.DeserializeObject<T>(response.Text);
                    callbackWithData.Invoke(data);
                }
            });
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PathItemAsset))]
    public class PathItemAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PathItemAsset pathItemAsset = (PathItemAsset)target;

            GUILayout.Space(10);
            GUILayout.Label("operations:");
            GUILayout.BeginHorizontal();
            pathItemAsset.Operations.ForEach(operation =>
            {
                if (GUILayout.Button("Test " + operation.OperationId))
                {
                    pathItemAsset.ExecuteOperation(operation, response =>
                    {
                        if (response.Ok)
                            Debug.Log(response.Text);
                        else
                            Debug.LogError(response.Error.Message);
                    });
                }
            });
            GUILayout.EndHorizontal();
        }
    }
#endif
}
