using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

[CreateAssetMenu(menuName ="JSON TESTING/testing")]
public class JsonSchemaTest : ScriptableObject
{
    public TextAsset openApi3Schema;
    public TextAsset jsonFile;

    public void Test()
    {
        var j = JsonConvert.DeserializeObject(jsonFile.text);
        Debug.Log(j);
    }
}

[CustomEditor(typeof(JsonSchemaTest))]
public class JsonSchemaTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var t = (JsonSchemaTest)target;
        if(GUILayout.Button("Test"))
        {
            t.Test();
        }
    }
}
