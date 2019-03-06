using Microsoft.OpenApi.Models;
using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OAParameter
    {
        public string Name;
        public bool Required;
        public bool AllowReserved;
        public bool Explode;
        //public ParameterStyle? Style;
        public bool AllowEmptyValue;
        public bool Deprecated;
        public string Description;
        public OAParameterLocation In;
        public OAReference Reference;
        public bool UnresolvedReference;
        //public IDictionary<string, OpenApiMediaType> Content;

        public OAParameter (OpenApiParameter openApiParameter)
        {
            Name = openApiParameter.Name;
            Required = openApiParameter.Required;
            AllowReserved = openApiParameter.AllowReserved;
            Explode = openApiParameter.Explode;
            AllowEmptyValue = openApiParameter.AllowEmptyValue;
            Deprecated = openApiParameter.Deprecated;
            Description = openApiParameter.Description;
            UnresolvedReference = openApiParameter.UnresolvedReference;

            In = (OAParameterLocation)openApiParameter.In;

            Reference = new OAReference(openApiParameter.Reference);
        }
    }
}