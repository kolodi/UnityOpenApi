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

        public OAReference(OpenApiReference openApiReference)
        {
            if (openApiReference == null) return;

            IsPresent = true;
            ExternalResource = openApiReference.ExternalResource;
            Type = (OAReferenceType)openApiReference.Type;
            Id = openApiReference.Id;
            IsExternal = openApiReference.IsExternal;
            IsLocal = openApiReference.IsLocal;
            Reference = string.IsNullOrEmpty(openApiReference.ReferenceV2)
                ? openApiReference.ReferenceV3 : openApiReference.ReferenceV2;
        }
    }
}