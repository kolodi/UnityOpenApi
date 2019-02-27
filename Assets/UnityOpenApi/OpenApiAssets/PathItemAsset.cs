using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityOpenApi
{
    [CreateAssetMenu(menuName ="Unity Open API/API Assets/Path Item", fileName ="Path asset")]
    public class PathItemAsset : ScriptableObject
    {
        public ApiAsset ApiAsset { get; set; }
    }
}