using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityOpenApi
{
    [CreateAssetMenu(menuName ="Unity Open API/API Assets/Path Item", fileName ="Path asset")]
    public class PathItemAsset : ScriptableObject
    {
        [HideInInspector]
        public ApiAsset ApiAsset;

        
        public string Summary;
        public string Description;
        public List<OAOperation> Operations;
        public List<OAServer> Servers;
        public List<OAParameter> Parameters;

        public static string PrepareFileName(string path)
        {
            return path.Replace('/', '_');
        }
    }
}