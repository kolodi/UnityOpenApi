using Microsoft.OpenApi.Models;
using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OAReference
    {
        public string Id;
        public bool IsPresent;
        public string ExternalResource;
        public OAReferenceType Type;
        public bool IsExternal;
        public bool IsLocal;
        public string Reference;
    }
}