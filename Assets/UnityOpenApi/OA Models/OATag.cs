using Microsoft.OpenApi.Models;
using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OATag
    {
        public string Name;
        public string Description;
        public OAExternalDocs ExternalDocs;
    }
}