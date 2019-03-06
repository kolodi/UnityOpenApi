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

        public OATag (OpenApiTag openApiTag)
        {
            Name = openApiTag.Name;
            Description = openApiTag.Description;
            ExternalDocs = new OAExternalDocs(openApiTag.ExternalDocs);
        }
    }
}