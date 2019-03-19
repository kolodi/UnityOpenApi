using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityOpenApi
{
    public class ApiConsumer : MonoBehaviour
    {
        public UnityEvent onStart;
        public UnityEvent onEnd;
        public UnityResponseHelperEvent onResult;
        public UnityStringEvent onResultText;
        public UnityFloatEvent onUploadUpdate;
        public UnityFloatEvent onDownloadUpdate;
        public UnityStringEvent onError;


        [SerializeField] PathItemAsset pathItem = null;
        public HttpWord currentOperationType = HttpWord.GET;
        public List<ParameterValue> parameters;
        public Operation Operation
        {
            get
            {
                return pathItem.GetOperation(currentOperationType);
            }
        }

        [ContextMenu("UpdateFromApi()")]
        public virtual void UpdateFromApi()
        {
            var op = Operation;
            var options = pathItem.ApiAsset.PrepareOperation(op);
            onStart?.Invoke();
            pathItem.ExecuteOperation(op, options)
                .Then(res =>
                {
                    onResult?.Invoke(res);
                    onResultText?.Invoke(res.Text);
                })
                .Catch(ex => 
                {
                    onError?.Invoke(ex.Message);
                })
                .Finally(() =>
                {
                    onEnd?.Invoke();
                });
        }

        private void ExposeParameters()
        {
            parameters = pathItem.GetOperation(currentOperationType).ParametersValues;
        }

        private void OnValidate()
        {
            try
            {
                var op = Operation;
            }
            catch (Exception)
            {
                Debug.LogWarning("No operation of type " + currentOperationType.ToString());
                currentOperationType = pathItem.Operations[0].OperationType;
            }
            ExposeParameters();
        }
    }
}