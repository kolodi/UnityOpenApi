using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using RSG;
using Proyecto26;
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

        public OAOperation GetOperation(string operationId)
        {
            return Operations.First(o => o.OperationId == operationId);
        }

        public OAOperation GetOperation(AOOperationType operationType)
        {
            return Operations.First(o => o.OperationType == operationType);
        }

        /// <summary>
        /// To be used only for string type responses, like JSON.
        /// The string response is cached by default.
        /// </summary>
        /// <param name="operation">API operation to execute</param>
        /// <returns>A promise with a string response</returns>
        public IPromise<string> ExecuteOperation(OAOperation operation)
        {
            var promise = new Promise<string>();

            if (operation.ignoreCache == false)
            {
                string data;
                if (operation.GetFromCache(out data))
                {
                    Debug.Log("From cahce");
                    promise.Resolve(data);
                    return promise;
                }
            }


            ApiAsset.ExecuteOperation(operation)
                .Then(res =>
                {
                    if (res.Error == null)
                    {
                        operation.Cache = res.Text;
                    }
                    promise.Resolve(res.Text);
                })
                .Catch(err => promise.Reject(err));

            return promise;
        }

        /// <summary>
        /// Use this for not JSON responses like binary textures or asset bundles
        /// </summary>
        /// <param name="operation">API operation to execute</param>
        /// <returns>A promise with complete response wrapper containing UnityWebRequest with all data</returns>
        public IPromise<ResponseHelper> ExecuteOperationRaw(OAOperation operation)
        {
            return ApiAsset.ExecuteOperation(operation);
        }

        /// <summary>
        /// Use this method for JSON data response only.
        /// The string response is cached by default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">API operation to execute</param>
        /// <returns>A promise with automatically parsed data from JSON</returns>
        public IPromise<T> ExecuteOperation<T>(OAOperation operation)
        {
            var promise = new Promise<T>();

            ExecuteOperation(operation)
                .Then(res => promise.Resolve(JsonConvert.DeserializeObject<T>(res)))
                .Catch(err => promise.Reject(err));

            return promise;
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
                    pathItemAsset.ExecuteOperation(operation)
                    .Then(response =>
                    {
                            Debug.Log(response);
                    });
                }
            });
            GUILayout.EndHorizontal();
        }
    }
#endif
}
