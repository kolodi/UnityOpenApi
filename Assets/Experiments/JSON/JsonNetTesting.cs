using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class JsonNetTesting : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] private TextAsset output = null;
    
    [ContextMenu("try1")]
    void Try1()
    {
        var ob = new JsontTestObject()
        {
            Name = "Kolodi"
        };

        var jsonString = JsonConvert.SerializeObject(ob, Formatting.Indented);

        Debug.Log(jsonString);

        File.WriteAllText(AssetDatabase.GetAssetPath(output), jsonString);
        EditorUtility.SetDirty(output);

        var ob1 = JsonConvert.DeserializeObject<JsontTestObject>(jsonString);

        Debug.Log(ob1.ExpirationDate.ToString("G", System.Globalization.CultureInfo.GetCultureInfo("it-IT")));
        
    }
#endif

}
