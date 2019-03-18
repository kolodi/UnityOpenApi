using System;

namespace UnityOpenApi
{
    [Serializable]
    public class Reference
    {
        public string Id;
        public bool IsPresent;
        public string ExternalResource;
        public ReferenceType Type;
        public bool IsExternal;
        public bool IsLocal;
        public string reference;
    }
}