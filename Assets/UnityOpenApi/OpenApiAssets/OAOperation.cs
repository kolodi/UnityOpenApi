using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OAOperation
    {     
        //public IList<OpenApiSecurityRequirement> Security;      
        public bool Deprecated;          
        //public IDictionary<string, OpenApiCallback> Callbacks;
        //public OpenApiResponses Responses;      
        //public OpenApiRequestBody RequestBody;      
        //public IList<OpenApiParameter> Parameters;       
        public string OperationId;      
        //public OpenApiExternalDocs ExternalDocs;      
        public string Description;     
        public string Summary;     
        //public IList<OpenApiTag> Tags;     
        //public IList<OpenApiServer> Servers;
    }
}