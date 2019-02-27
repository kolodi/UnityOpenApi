using Microsoft.OpenApi.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityOpenApi
{
    [CreateAssetMenu(menuName = "Unity Open API/API Assets/API", fileName = "API asset")]
    public class ApiAsset : ScriptableObject
    {
        public OpenApiDocument openApiDocument;
    }
}