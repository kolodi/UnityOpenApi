using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityOpenApi
{

    public class JsonResponseVisualizer : MonoBehaviour
    {
        public void ShowJsonDiag(string json)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Result", json, "close");
#endif
        }

        public void ShowRestResult(ResponseHelper response)
        {
            ShowJsonDiag(response.Text);
        }
    }
}