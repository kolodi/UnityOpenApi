using System;

namespace UnityOpenApi
{
    [Serializable]
    public class Parameter
    {
        public string Name;
        public bool Required;
        public bool AllowReserved;
        public bool Explode;
        //public ParameterStyle? Style;
        public bool AllowEmptyValue;
        public bool Deprecated;
        public string Description;
        public ParameterLocation In;
        public Reference Reference;
        public bool UnresolvedReference;
        //public IDictionary<string, OpenApiMediaType> Content;
    }
}