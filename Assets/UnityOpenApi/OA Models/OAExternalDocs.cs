using Microsoft.OpenApi.Models;
using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OAExternalDocs
    {
        public string Description;
        public string Url;

        public OAExternalDocs(OpenApiExternalDocs openApiExternalDocs)
        {
            if (openApiExternalDocs == null) return;
            Description = openApiExternalDocs.Description;
            Url = openApiExternalDocs.Url.ToString();
        }
    }
}