using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using HttpMono;
using UnityOpenApi.ApiModels.OpenApi;

namespace UnityOpenApi
{

    [CreateAssetMenu(menuName = "Unity Open API/API Resource", fileName = "_api_res_")]
    public class ApiResource : ScriptableObject
    {
        public ApiConfig apiConfig;
        public ApiAuthorizer apiAuthorizer;
        public OpenApi3HttpWords httpWord;
        public string path;
        public OpenApi3Method method;
        public List<ApiResourceParameterValue> parametersValues = new List<ApiResourceParameterValue>();
  

        [Space]
        [SerializeField]
        [Multiline(10)]
        private string lastText;

        public string LastText { get => lastText;}

        public void SetParameters(List<ApiResourceParameter> parameters)
        {
            foreach (var p in parameters)
            {
                SetParameter(p.parameterName, p.parameterValue);
            }
        }

        public void SetParameter(string parameterName, string val)
        {

            var parameter = parametersValues.FirstOrDefault(p => p.parameter.name == parameterName);
            if (parameter == default(ApiResourceParameterValue))
            {
                throw new Exception("Error in api resource " + name + ": no parameter is found with name: " + parameterName);
            }
            else
            {
                parameter.parametrValue = val;
            }

        }

        private void OnEnable()
        {
            lastText = string.Empty;
        }

        public void Fetch(Action<HttpRequestResult> callback)
        {
            apiConfig.CallApi(this, result =>
            {
                lastText = result.Text;
                callback.Invoke(result);
            });
        }

        public void FetchData<T>(Action<T> success, Action<Exception> error = null, bool tryFromCache = false)
        {
            if(tryFromCache && string.IsNullOrEmpty(LastText))
            {
                try
                {
                    T parsed = JsonConvert.DeserializeObject<T>(LastText);
                    success.Invoke(parsed);
                }
                catch (Exception e)
                {
                    if(error != null) error.Invoke(e);
                }
                return;
            }

            apiConfig.CallApi(this, result =>
            {
                if (result.Ok)
                {
                    try
                    {
                        T parsed = JsonConvert.DeserializeObject<T>(result.Text);
                        lastText = result.Text;
                        success.Invoke(parsed);
                    }
                    catch (Exception e)
                    {
                        if (error != null) error.Invoke(e);
                    }
                }
                else
                {
                    if (error != null) error.Invoke(new Exception(result.Text));
                }
            });
        }

        public void ResetParametersValues()
        {
            if (method.parameters != null)
            {
                parametersValues = method.parameters.Select(par =>
                {
                    return new ApiResourceParameterValue()
                    {
                        parameter = par,
                        parametrValue = ""
                    };
                }).ToList();
            }
        }
    }

    [System.Serializable]
    public class ApiResourceParameterValue
    {
        public OpenApi3MethodParameter parameter;
        public string parametrValue;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ApiResource))]
    public class ApiResourceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ApiResource apiResource = (ApiResource)target;
            if (GUILayout.Button("Fetch"))
            {
                apiResource.Fetch(result =>
                {
                    if (result != null)
                        Debug.Log(result.Text);
                    else Debug.Log("Nothing returned");
                });
            }
        }
    }
#endif
}
